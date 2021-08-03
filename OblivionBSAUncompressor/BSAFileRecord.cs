using System;
using System.IO;

namespace OblivionBSAUncompressor
{
    class BSAFileRecord : IComparable<BSAFileRecord>
    {
        public BSAFileRecord(string folderName, string fileName, bool bsaCompressed, bool isFileNameEmbedded, bool invertedCompression = false, bool isChecked = false)
        {
            FolderName = folderName;
            SetNameWithHash(fileName);
            BSACompressed = bsaCompressed;
            IsFileNameEmbedded = isFileNameEmbedded;
            InvertedCompression = invertedCompression;
            Checked = isChecked;
        }

        public BSAFileRecord(ulong nameHash, uint fileSize, uint fileDataOffset, bool bsaCompressed, bool isFileNameEmbedded)
        {
            NameHash = nameHash;
            FileSize = fileSize;
            FileDataOffset = fileDataOffset;
            BSACompressed = bsaCompressed;
            IsFileNameEmbedded = isFileNameEmbedded;
        }

        public ulong NameHash { get; internal set; }
        public uint FileSize
        {
            get => RealFileSize | (Checked ? CheckedBit : 0) | (InvertedCompression ? invertedCompressionBit : 0);
            internal set
            {
                RealFileSize = value & ~invertedCompressionBit & ~CheckedBit;
                Checked = (value & CheckedBit) != 0;
                InvertedCompression = (value & invertedCompressionBit) != 0;
            }
        }
        public uint RealFileSize { get; internal set; }
        public bool InvertedCompression { get; internal set; }
        public bool Checked { get; internal set; }
        public uint FileDataOffset { get; internal set; }
        public string Name { get; internal set; }
        public string FolderName { get; internal set; }
        public string FullPath { get => Path.Combine(FolderName, Name); }
        public bool BSACompressed { get; internal set; }
        public bool SelfCompressed { get => InvertedCompression ? !BSACompressed : BSACompressed; }
        public bool IsFileNameEmbedded { get; internal set; }

        public uint GetUncompressedSize(Stream inputStream)
        {
            GetDataInfo(inputStream, out var _, out var _, out var uncompressedSize);

            return uncompressedSize;
        }

        public void GetDataInfo(Stream inputStream, out uint dataOffset, out uint dataSize, out uint uncompressedSize)
        {
            uncompressedSize = 0;
            dataSize = RealFileSize;
            dataOffset = FileDataOffset;
            if (IsFileNameEmbedded)
            {
                inputStream.Seek(dataOffset, SeekOrigin.Begin);
                uint length = (uint)inputStream.ReadByte() + 1;
                // skip size and string
                dataOffset += length;
                dataSize -= length;
            }

            if (SelfCompressed)
            {
                inputStream.Seek(dataOffset, SeekOrigin.Begin);
                var binaryReader = new BinaryReader(inputStream);
                uncompressedSize = binaryReader.ReadUInt32();
                dataSize -= 4;
                dataOffset += 4;
            }

            if (uncompressedSize == 0)
            {
                uncompressedSize = dataSize;
            }
        }

        public byte[] GetRawFileData(Stream inputStream)
        {
            return GetRawFileData(inputStream, out _);
        }

        public byte[] GetRawFileData(Stream inputStream, out uint uncompressedSize)
        {
            GetDataInfo(inputStream, out var dataOffset, out var dataSize, out uncompressedSize);

            var data = new byte[dataSize];

            GetRawFileData(inputStream, dataOffset, dataSize, data);

            return data;
        }

        public void GetRawFileData(Stream inputStream, uint fileDataOffset, uint fileDataSize, byte[] destination, int offset = 0)
        {
            BSAUtilities.Ensures<ArgumentOutOfRangeException>(offset >= 0, "Offset must not be less than 0.");

            BSAUtilities.Ensures<ArgumentException>(destination.Length - offset >= fileDataSize, "Not enough space in destination buffer.");

            inputStream.Seek(fileDataOffset, SeekOrigin.Begin);
            inputStream.Read(destination, offset, (int)fileDataSize);
        }

        public byte[] GetUncompressedFileData(Stream inputStream)
        {
            if (!SelfCompressed)
            {
                return GetRawFileData(inputStream);
            }

            var compressedData = GetRawFileData(inputStream, out var UncompressedSize);

            var uncompressedData = BSAUtilities.DecompressData(compressedData, (int)UncompressedSize);
            return uncompressedData;
        }

        public void SetNameWithHash(string name)
        {
            Name = name;
            NameHash = BSAUtilities.GetHash(name);
        }

        public int CompareTo(BSAFileRecord other)
        {
            return NameHash.CompareTo(other.NameHash);
        }

        private const uint invertedCompressionBit = (1U << 30);
        private const uint CheckedBit = (1U << 31);
    }
}
