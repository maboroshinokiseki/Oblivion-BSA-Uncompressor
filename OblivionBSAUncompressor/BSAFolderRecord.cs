using System;

namespace OblivionBSAUncompressor
{
    class BSAFolderRecord : IComparable<BSAFolderRecord>
    {
        public BSAFolderRecord(string name, uint totalFileNameLength)
        {
            SetName(name);
            TotalFileNameLength = totalFileNameLength;
        }

        public BSAFolderRecord(ulong nameHash, uint fileCount, uint fileRecordsOffset, uint totalFileNameLength)
        {
            NameHash = nameHash;
            FileCount = fileCount;
            FileRecordsOffset = fileRecordsOffset;
            TotalFileNameLength = totalFileNameLength;
        }

        public ulong NameHash { get; private set; }
        public string Name { get; private set; }
        public uint FileCount { get; internal set; }
        public uint FileRecordsOffset { get; internal set; }
        public uint RealFileRecordsOffset
        {
            get => FileRecordsOffset - TotalFileNameLength;
            internal set => FileRecordsOffset = value + TotalFileNameLength;
        }
        public uint TotalFileNameLength { get; private set; }
        public BSAFileRecord[] FileRecords { get; internal set; }

        public int CompareTo(BSAFolderRecord other)
        {
            return NameHash.CompareTo(other.NameHash);
        }

        public void SetName(string name, bool setHash = true)
        {
            Name = name;
            if (setHash)
            {
                NameHash = BSAUtilities.GetHash(name, string.Empty);
            }
        }
    }
}
