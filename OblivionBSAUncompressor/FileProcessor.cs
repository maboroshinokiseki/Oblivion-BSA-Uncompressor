using LiquidEngine.Tools;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OblivionBSAUncompressor
{
    class FileProcessor
    {
        private bool loadToMemory;
        private bool multithreaded;
        private bool useMemoryStream;
        private bool sameAsOrigin;
        private string destinationFolder;
        private readonly int maxTaskCount;
        private readonly uint splitSize;
        private readonly bool dummyEsp;
        private readonly CurrentGame currentGame;

        // preserved space for header.
        private const int preservedSpace = 1024 * 1024 * 8;
        private static readonly ArrayPool<byte> arrayPool = ArrayPool<byte>.Create(1024 * 1024 * 32, 16);

        public FileProcessor(bool loadToMemory,
                             bool multithreaded,
                             bool useMemoryStream,
                             bool sameAsOrigin,
                             string destinationFolder,
                             uint splitSize,
                             bool dummyEsp,
                             CurrentGame currentGame)
        {
            this.loadToMemory = loadToMemory;
            this.multithreaded = multithreaded;
            this.useMemoryStream = useMemoryStream;
            this.sameAsOrigin = sameAsOrigin;
            this.destinationFolder = destinationFolder;
            this.splitSize = splitSize;
            this.dummyEsp = dummyEsp;
            this.currentGame = currentGame;

            maxTaskCount = Environment.ProcessorCount - 2;
            if (maxTaskCount < 1 || !multithreaded)
            {
                maxTaskCount = 1;
            }
        }

        public async Task StartProcessAsync(string filePath, Action<int> callback, CancellationToken cancellationToken)
        {
            // pre setup
            var fileName = Path.GetFileName(filePath);
            var destinationFolder = GetDestinationFolder(filePath);
            using var inputStream = await GetInputStreamAsync(filePath);

            var inputBsa = new BSAArchive(inputStream);

            var filesInBsa = inputBsa.GetFileRecords().ToDictionary(f => f.FullPath);

            var outputs = OutputGenerator(fileName, destinationFolder, inputBsa, inputStream);

            using var newDataEvent = new AutoResetEvent(false);

            using var originalFileDataList = new BlockingCollection<FileBlockData>(maxTaskCount);
            var readingTask = Task.Run(() =>
            {
                foreach (var outputData in outputs)
                {
                    foreach (var outputFileRecord in outputData.BSAArchive.GetFileRecords())
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var inputFileRecord = filesInBsa[outputFileRecord.FullPath];
                        inputFileRecord.GetDataInfo(inputStream, out var fileOffset, out var fileSize, out var uncompressedSize);
                        var originalData = arrayPool.Rent((int)fileSize);
                        inputFileRecord.GetRawFileData(inputStream, fileOffset, fileSize, originalData);
                        originalFileDataList.Add(new FileBlockData
                        {
                            InputFileRecord = inputFileRecord,
                            UncompressedSize = uncompressedSize,
                            RawDataSize = fileSize,
                            RawData = originalData
                        }, cancellationToken);
                    }
                }

                originalFileDataList.CompleteAdding();
            }, cancellationToken);

            var decompressedDataMap = new ConcurrentDictionary<string, FileBlockData>();
            var DecompressTasks = new List<Task>(maxTaskCount);
            for (int i = 0; i < maxTaskCount; i++)
            {
                DecompressTasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (originalFileDataList.TryTake(out var item, 64))
                        {
                            item.UncompressedData = item.RawData;
                            if (item.InputFileRecord.SelfCompressed)
                            {
                                var uncompressedData = arrayPool.Rent((int)item.UncompressedSize);
                                BSAUtilities.DecompressData(item.RawData, 0, (int)item.RawDataSize, (int)item.UncompressedSize, uncompressedData);
                                item.UncompressedData = uncompressedData;
                            }

                            decompressedDataMap.TryAdd(item.InputFileRecord.FullPath, item);
                            newDataEvent.Set();
                        }

                        if (originalFileDataList.IsCompleted)
                        {
                            break;
                        }
                    }
                }, cancellationToken));
            }

            var writingTask = Task.Run(() =>
            {
                var processedCount = 0;
                foreach (var outputData in outputs)
                {
                    var outputBsa = outputData.BSAArchive;
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var fileRecord = outputBsa.GetCurrentFileRecord();
                        if (fileRecord == null)
                        {
                            break;
                        }

                        FileBlockData fileBlockData;

                        while (!decompressedDataMap.TryRemove(fileRecord.FullPath, out fileBlockData))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            newDataEvent.WaitOne(64);
                        }

                        outputBsa.WriteFileData(fileBlockData.UncompressedData, 0, (int)fileBlockData.UncompressedSize);
                        arrayPool.Return(fileBlockData.RawData);
                        if (fileBlockData.RawData != fileBlockData.UncompressedData)
                        {
                            arrayPool.Return(fileBlockData.UncompressedData);
                        }

                        callback?.Invoke(++processedCount);
                    }
                }
            });

            var allTasks = new List<Task>(maxTaskCount + 2)
                {
                    readingTask,
                    writingTask
                };
            allTasks.AddRange(DecompressTasks);

            try
            {
                await Task.WhenAll(allTasks);

                if (useMemoryStream)
                {
                    foreach (var outputData in outputs)
                    {
                        using var fileStream = File.Create(outputData.TempFilePath);
                        outputData.OutputStream.Position = 0;
                        await outputData.OutputStream.CopyToAsync(fileStream);
                        outputData.OutputStream.Close();
                    }
                }

                inputStream.Close();

                if (sameAsOrigin)
                {
                    File.Delete(filePath);
                }

                int nameAppend = 0;
                foreach (var outputData in outputs)
                {
                    outputData.OutputStream.Close();
                    var finalFilePath = Path.Combine(destinationFolder, fileName);
                    if (!File.Exists(finalFilePath))
                    {
                        File.Move(outputData.TempFilePath, finalFilePath);
                    }
                    else
                    {
                        var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                        var fileExtension = Path.GetExtension(fileName);
                        while (true)
                        {
                            var finalfileNameNoExt = $"{fileNameOnly} - {++nameAppend}";
                            var finalFileName = $"{finalfileNameNoExt}{fileExtension}";
                            finalFilePath = Path.Combine(destinationFolder, finalFileName);
                            if (!File.Exists(finalFilePath))
                            {
                                File.Move(outputData.TempFilePath, finalFilePath);
                                if (dummyEsp)
                                {
                                    using var dummy = currentGame switch
                                    {
                                        CurrentGame.Oblivion => this.GetType().Assembly.GetManifestResourceStream("OblivionBSAUncompressor.dummyEsp.DummyOblivion.esp"),
                                        CurrentGame.Fallout3 => this.GetType().Assembly.GetManifestResourceStream("OblivionBSAUncompressor.dummyEsp.DummyFallout3.esp"),
                                        CurrentGame.FalloutNV => this.GetType().Assembly.GetManifestResourceStream("OblivionBSAUncompressor.dummyEsp.DummyFalloutNV.esp"),
                                        CurrentGame.Skyrim => this.GetType().Assembly.GetManifestResourceStream("OblivionBSAUncompressor.dummyEsp.DummySkyrim.esp"),
                                        _ => this.GetType().Assembly.GetManifestResourceStream("OblivionBSAUncompressor.dummyEsp.DummyOblivion.esp"),
                                    };
                                    var dummyPath = Path.Combine(destinationFolder, $"{finalfileNameNoExt}.esp");
                                    if (!File.Exists(dummyPath))
                                    {
                                        using var dummyFS = File.Create(dummyPath);
                                        dummy.CopyTo(dummyFS);
                                    }
                                }
                                break;
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                foreach (var outputData in outputs)
                {
                    if (File.Exists(outputData.TempFilePath))
                    {
                        outputData.OutputStream.Close();
                        File.Delete(outputData.TempFilePath);
                    }
                }

                throw;
            }
        }

        private async Task<Stream> GetInputStreamAsync(string filePath)
        {
            Stream inputStream = File.OpenRead(filePath);
            if (loadToMemory)
            {
                var tempStream = new MemoryTributary();
                await inputStream.CopyToAsync(tempStream);
                tempStream.Position = 0;
                inputStream.Dispose();
                inputStream = tempStream;
            }

            return inputStream;
        }

        private Stream GetOutputStream(string outputPath)
        {
            Stream outputStream = null;
            if (useMemoryStream)
            {
                outputStream = new MemoryTributary();
            }
            else
            {
                outputStream = File.Create(outputPath);
            }

            return outputStream;
        }

        private string GetTempFileName(string fileName, string destinationFolder, HashSet<string> takenNames)
        {
            var tempFileName = "Decompressed - " + fileName;
            if (File.Exists(Path.Combine(destinationFolder, tempFileName)) || takenNames.Contains(tempFileName))
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    tempFileName = "Decompressed - " + fileName + " - " + i.ToString();
                    if (!File.Exists(Path.Combine(destinationFolder, tempFileName)) && !takenNames.Contains(tempFileName))
                    {
                        break;
                    }
                }
            }

            return tempFileName;
        }

        private string GetDestinationFolder(string filePath)
        {
            string destinationFolder;
            if (sameAsOrigin)
            {
                destinationFolder = Path.GetDirectoryName(filePath);
            }
            else
            {
                destinationFolder = this.destinationFolder;
            }

            return destinationFolder;
        }

        private List<OutputBSAData> OutputGenerator(string fileName, string destinationFolder, BSAArchive inputBsa, Stream inputStream)
        {
            var groupedFilesInBsa = new List<List<string>>
            {
                new List<string>()
            };

            uint currentSize = preservedSpace;
            foreach (var record in inputBsa.GetFileRecords())
            {
                record.GetDataInfo(inputStream, out var _, out var _, out var uncompressedSize);
                if (currentSize + uncompressedSize > splitSize)
                {
                    groupedFilesInBsa.Add(new List<string>());
                    currentSize = preservedSpace;
                }

                currentSize += uncompressedSize;
                groupedFilesInBsa.Last().Add(record.FullPath);
            }

            var outputArchiveFlags = inputBsa.ArchiveFlags & ~BSAArchiveFlags.Compressed;
            var result = new List<OutputBSAData>();
            var takenNames = new HashSet<string>();
            foreach (var files in groupedFilesInBsa)
            {
                var tempFileName = GetTempFileName(fileName, destinationFolder, takenNames);
                takenNames.Add(tempFileName);
                var tempFilePath = Path.Combine(destinationFolder, tempFileName);
                var outputStream = GetOutputStream(tempFilePath);
                var outputBsa = new BSAArchive(outputStream, inputBsa.Version, outputArchiveFlags, files);

                result.Add(new OutputBSAData
                {
                    BSAArchive = outputBsa,
                    TempFilePath = tempFilePath,
                    OutputStream = outputStream,
                });
            }

            return result;
        }

        private class FileBlockData
        {
            public BSAFileRecord InputFileRecord;
            public uint UncompressedSize;
            public uint RawDataSize;
            public byte[] RawData;
            public byte[] UncompressedData;
        }

        private class OutputBSAData
        {
            public BSAArchive BSAArchive;
            public string TempFilePath;
            public Stream OutputStream;
        }
    }
}
