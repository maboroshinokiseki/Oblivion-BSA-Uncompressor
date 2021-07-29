using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OblivionBSAUncompressor
{
    class BSAArchive
    {
        public uint Version { get; set; }
        public BSAArchiveFlags ArchiveFlags { get; private set; }
        public uint HeaderSize { get; private set; }
        public bool Compressed { get; private set; }
        public bool EmbedFileName { get; private set; }
        public uint FolderCount { get; private set; }
        public uint TotalFileCount { get; private set; }
        public uint TotalFolderNameLength { get; private set; }
        public uint TotalFileNameLength { get; private set; }
        public BSAContentFlags ContentFlags { get; private set; }
        public BSAFolderRecord[] FolderRecords { get; private set; }

        public BSAArchive(Stream input)
        {
            var binaryReader = new BinaryReader(input);

            ReadHeader(binaryReader);
            ReadFolderRecords(binaryReader);
            ReadFileRecords(binaryReader);
            ReadFileNames(binaryReader);
        }

        public BSAArchive(Stream output, uint version, BSAArchiveFlags archiveFlags, IEnumerable<string> files)
        {
            this.writable = true;
            var offsetsOfFolderOffsetField = new Dictionary<string, long>();
            this.offsetsOfFileOffsetField = new Dictionary<string, long>();

            GenerateHeader(version, archiveFlags, files);
            GenerateContentFlag(files);
            GenerateFolderFileRecords(files);

            binaryWriter = new BinaryWriter(output);

            binaryWriter.Write(bsaMagicNumber);
            binaryWriter.Write(Version);
            binaryWriter.Write(HeaderSize);
            binaryWriter.Write((uint)ArchiveFlags);
            binaryWriter.Write(FolderCount);
            binaryWriter.Write(TotalFileCount);
            binaryWriter.Write(TotalFolderNameLength);
            binaryWriter.Write(TotalFileNameLength);
            binaryWriter.Write((uint)ContentFlags);

            foreach (var folder in FolderRecords)
            {
                binaryWriter.Write(folder.NameHash);
                binaryWriter.Write(folder.FileCount);

                // Save current position so we can write offset later
                offsetsOfFolderOffsetField[folder.Name] = binaryWriter.BaseStream.Position;
                binaryWriter.Write(0U);
            }

            foreach (var folder in FolderRecords)
            {
                var current = binaryWriter.BaseStream.Position;
                binaryWriter.BaseStream.Position = offsetsOfFolderOffsetField[folder.Name];
                folder.RealFileRecordsOffset = (uint)current;
                binaryWriter.Write(folder.FileRecordsOffset);
                binaryWriter.BaseStream.Position = current;

                binaryWriter.Flush();
                if (archiveFlags.HasFlag(BSAArchiveFlags.HasDirectoryNames))
                {
                    binaryWriter.Write((byte)(folder.Name.Length + 1));
                    binaryWriter.Write(Encoding.UTF8.GetBytes(folder.Name));
                    binaryWriter.Write((byte)0);
                }
                binaryWriter.Flush();

                foreach (var file in folder.FileRecords)
                {
                    binaryWriter.Write(file.NameHash);

                    // Write file size and data offset later
                    this.offsetsOfFileOffsetField[file.FullPath] = binaryWriter.BaseStream.Position;
                    binaryWriter.Write(0);
                    binaryWriter.Write(0);
                }
            }

            if (archiveFlags.HasFlag(BSAArchiveFlags.HasFileNames))
            {
                foreach (var file in GetFileRecords())
                {
                    binaryWriter.Write(Encoding.UTF8.GetBytes(file.Name));
                    binaryWriter.Write((byte)0);
                }
            }
        }

        public IEnumerable<BSAFolderRecord> GetFolderRecords()
        {
            return FolderRecords;
        }

        public IEnumerable<BSAFileRecord> GetFileRecords()
        {
            foreach (var folder in FolderRecords)
            {
                foreach (var file in folder.FileRecords)
                {
                    yield return file;
                }
            }
        }

        public void WriteFileData(byte[] data, int compressLevel = 0)
        {
            if (currentFolderIndex == -1)
            {
                throw new InvalidOperationException("Already added all files");
            }

            if (compressLevel < 0)
            {
                compressLevel = 0;
            }

            if (compressLevel > 9)
            {
                compressLevel = 9;
            }

            var dataToWrite = data;

            var currentFileRecord = FolderRecords[currentFolderIndex].FileRecords[currentFileIndex];
            var startOfFileData = binaryWriter.BaseStream.Position;
            if (Version == version104 && ArchiveFlags.HasFlag(BSAArchiveFlags.EmbedFileName))
            {
                binaryWriter.Write((byte)currentFileRecord.FullPath.Length);
                binaryWriter.Write(Encoding.UTF8.GetBytes(currentFileRecord.FullPath));
            }

            currentFileRecord.InvertedCompression = currentFileRecord.BSACompressed ^ (compressLevel != 0);

            if (compressLevel > 0)
            {
                var deflater = new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(compressLevel);
                deflater.SetInput(data);
                deflater.Finish();
                var compressedData = new byte[data.Length * 2];
                var compressedSize = deflater.Deflate(compressedData);
                Array.Resize(ref compressedData, compressedSize);
                dataToWrite = compressedData;
                binaryWriter.Write(compressedSize);
            }

            binaryWriter.Write(dataToWrite);
            var endOfFileData = binaryWriter.BaseStream.Position;
            currentFileRecord.RealFileSize = (uint)(endOfFileData - startOfFileData);
            currentFileRecord.FileDataOffset = (uint)startOfFileData;
            binaryWriter.BaseStream.Position = this.offsetsOfFileOffsetField[currentFileRecord.FullPath];
            binaryWriter.Write(currentFileRecord.FileSize);
            binaryWriter.Write(currentFileRecord.FileDataOffset);
            binaryWriter.BaseStream.Position = endOfFileData;

            StepCurrentIndices();
        }

        public BSAFileRecord GetCurrentFileRecord()
        {
            if (currentFolderIndex == -1)
            {
                return null;
            }

            return FolderRecords[currentFolderIndex].FileRecords[currentFileIndex];
        }

        public bool IsDoneAdding { get => currentFolderIndex == -1; }

        // "BSA\0"
        private const uint bsaMagicNumber = 0x00415342;

        private readonly BinaryWriter binaryWriter = null;
        // used in Oblivion
        private const uint version103 = 103;
        // used in Fallout 3, Fallout NV and Skyrim
        private const uint version104 = 104;
        private const uint defaultHeaderSize = 36;
        private int currentFolderIndex = 0;
        private int currentFileIndex = 0;
        private bool writable = false;
        private Dictionary<string, long> offsetsOfFileOffsetField;

        private void ReadHeader(BinaryReader binaryReader)
        {
            var magicNumber = binaryReader.ReadUInt32();
            if (magicNumber != bsaMagicNumber)
            {
                throw new NotSupportedException("File was not a valid BSA archive");
            }

            Version = binaryReader.ReadUInt32();
            if (Version != version103 && Version != version104)
            {
                throw new NotSupportedException("Unknow BSA version");
            }

            HeaderSize = binaryReader.ReadUInt32();
            ArchiveFlags = (BSAArchiveFlags)binaryReader.ReadUInt32();
            if (ArchiveFlags.HasFlag(BSAArchiveFlags.BigEndian))
            {
                throw new NotSupportedException("Not support big-endian (like xbox 360) yet");
            }

            Compressed = ArchiveFlags.HasFlag(BSAArchiveFlags.Compressed);
            EmbedFileName = Version == version104 && ArchiveFlags.HasFlag(BSAArchiveFlags.EmbedFileName);
            FolderCount = binaryReader.ReadUInt32();
            TotalFileCount = binaryReader.ReadUInt32();
            TotalFolderNameLength = binaryReader.ReadUInt32();
            TotalFileNameLength = binaryReader.ReadUInt32();
            ContentFlags = (BSAContentFlags)binaryReader.ReadUInt32();
        }

        private void ReadFolderRecords(BinaryReader binaryReader)
        {
            FolderRecords = new BSAFolderRecord[FolderCount];
            for (int i = 0; i < FolderRecords.Length; i++)
            {
                var nameHash = binaryReader.ReadUInt64();
                var fileCount = binaryReader.ReadUInt32();
                var offset = binaryReader.ReadUInt32();
                FolderRecords[i] = new BSAFolderRecord(nameHash, fileCount, offset, TotalFileNameLength);
            }
        }

        private void ReadFileRecords(BinaryReader binaryReader)
        {
            foreach (var folder in FolderRecords)
            {
                var folderName = string.Empty;
                if (ArchiveFlags.HasFlag(BSAArchiveFlags.HasDirectoryNames))
                {
                    var length = binaryReader.ReadByte();
                    if (length > 0)
                    {
                        folderName = binaryReader.ReadString(length - 1);

                        // skip \0
                        binaryReader.ReadByte();
                    }
                }

                folder.Name = folderName;
                if (folder.NameHash != BSAUtilities.GetHash(folderName, string.Empty))
                {
                    throw new InvalidDataException("Folder name hash not matching folder name");
                }

                var fileRecords = new BSAFileRecord[folder.FileCount];

                for (uint i = 0; i < fileRecords.Length; i++)
                {
                    var nameHash = binaryReader.ReadUInt64();
                    var fileSize = binaryReader.ReadUInt32();
                    var offset = binaryReader.ReadUInt32();
                    fileRecords[i] = new BSAFileRecord(nameHash, fileSize, offset, Compressed, EmbedFileName);
                }

                folder.FileRecords = fileRecords;
            }
        }

        private void ReadFileNames(BinaryReader binaryReader)
        {
            if (!ArchiveFlags.HasFlag(BSAArchiveFlags.HasFileNames))
            {
                return;
            }

            foreach (var folder in FolderRecords)
            {
                foreach (var file in folder.FileRecords)
                {
                    var name = binaryReader.ReadNullTerminatedString();
                    if (file.NameHash != BSAUtilities.GetHash(name))
                    {
                        throw new InvalidDataException("File name hash not matching file name");
                    }

                    file.Name = name;
                    file.FolderName = folder.Name;
                }
            }
        }

        private void GenerateHeader(uint version, BSAArchiveFlags archiveFlags, IEnumerable<string> files)
        {
            Version = version;
            HeaderSize = defaultHeaderSize;
            ArchiveFlags = archiveFlags;
            Compressed = archiveFlags.HasFlag(BSAArchiveFlags.Compressed);
            EmbedFileName = version == version104 && archiveFlags.HasFlag(BSAArchiveFlags.EmbedFileName);

            var folderSet = new HashSet<string>();
            uint fileCount = 0;
            int totalFolderNameLength = 0;
            int totalFileNameLength = 0;
            foreach (var file in files)
            {
                var directoryName = Path.GetDirectoryName(file);
                if (folderSet.Add(directoryName))
                {
                    totalFolderNameLength += directoryName.Length + 1;
                }

                fileCount++;
                var fileName = Path.GetFileName(file);
                totalFileNameLength += fileName.Length + 1;
            }

            FolderCount = (uint)folderSet.Count;
            TotalFileCount = fileCount;
            TotalFolderNameLength = (uint)totalFolderNameLength;
            TotalFileNameLength = (uint)totalFileNameLength;
        }

        private void GenerateContentFlag(IEnumerable<string> files)
        {
            BSAContentFlags flags = 0;
            foreach (var file in files)
            {
                switch (Path.GetExtension(file))
                {
                    case ".nif":
                        flags |= BSAContentFlags.Meshes;
                        break;
                    case ".dds":
                        flags |= BSAContentFlags.Textures;
                        break;
                    case ".xml":
                        flags |= BSAContentFlags.Menus;
                        break;
                    case ".wav":
                        flags |= BSAContentFlags.Sounds;
                        break;
                    case ".mp3":
                        flags |= BSAContentFlags.Voices;
                        break;
                    case ".txt":
                    case ".html":
                    case ".bat":
                    case ".scc":
                        flags |= BSAContentFlags.Shaders;
                        break;
                    case ".spt":
                        flags |= BSAContentFlags.Trees;
                        break;
                    case ".tex":
                    case ".fon":
                        flags |= BSAContentFlags.Fonts;
                        break;
                    case ".ctl":
                        flags |= BSAContentFlags.Miscellaneous;
                        break;
                }
            }

            ContentFlags = flags;
        }

        private void GenerateFolderFileRecords(IEnumerable<string> files)
        {
            var folderToFolderRecordMap = new Dictionary<string, BSAFolderRecord>();
            var folderToFileRecordsMap = new Dictionary<string, List<BSAFileRecord>>();

            foreach (var file in files.Select(f => f.ToLowerInvariant()))
            {
                var directoryName = Path.GetDirectoryName(file);
                var fileName = Path.GetFileName(file);

                if (!folderToFolderRecordMap.TryGetValue(directoryName, out var folderRecord))
                {
                    folderRecord = new BSAFolderRecord(directoryName)
                    {
                        TotalFileNameLength = TotalFileNameLength
                    };
                    folderToFolderRecordMap[directoryName] = folderRecord;
                    folderToFileRecordsMap[directoryName] = new List<BSAFileRecord>();
                }

                var fileRecords = folderToFileRecordsMap[directoryName];
                var fileRecord = new BSAFileRecord(directoryName, fileName, Compressed, EmbedFileName);
                fileRecords.Add(fileRecord);
            }

            foreach (var folderRecordPair in folderToFileRecordsMap)
            {
                var folder = folderRecordPair.Key;
                var fileRecords = folderRecordPair.Value;
                fileRecords.Sort();
                var folderRecord = folderToFolderRecordMap[folder];
                folderRecord.FileRecords = fileRecords.ToArray();
                folderRecord.FileCount = (uint)fileRecords.Count;
            }

            FolderRecords = folderToFolderRecordMap.Values.ToArray();
            Array.Sort(FolderRecords);
        }

        private void StepCurrentIndices()
        {
            currentFileIndex++;
            if (currentFileIndex < FolderRecords[currentFolderIndex].FileRecords.Length)
            {
                return;
            }

            currentFileIndex = 0;
            while (true)
            {
                currentFolderIndex++;
                if (currentFolderIndex >= FolderRecords.Length)
                {
                    currentFolderIndex = -1;
                    break;
                }

                if (currentFileIndex < FolderRecords[currentFolderIndex].FileRecords.Length)
                {
                    break;
                }
            }
        }
    }
}
