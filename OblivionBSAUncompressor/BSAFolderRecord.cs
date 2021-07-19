﻿using System;

namespace OblivionBSAUncompressor
{
    class BSAFolderRecord: IComparable<BSAFolderRecord>
    {
        public BSAFolderRecord(string name)
        {
            SetNameWithHash(name);
        }

        public BSAFolderRecord(ulong nameHash, uint fileCount, uint fileRecordsOffset, uint totalFileNameLength)
        {
            NameHash = nameHash;
            FileCount = fileCount;
            FileRecordsOffset = fileRecordsOffset;
            TotalFileNameLength = totalFileNameLength;
        }

        public ulong NameHash { get; internal set; }
        public string Name { get; internal set; }
        public uint FileCount { get; internal set; }
        public uint FileRecordsOffset { get; internal set; }
        public uint RealFileRecordsOffset
        {
            get => FileRecordsOffset - TotalFileNameLength;
            internal set => FileRecordsOffset = value + TotalFileNameLength;
        }
        public uint TotalFileNameLength { get; internal set; }
        public BSAFileRecord[] FileRecords { get; internal set; }

        public int CompareTo(BSAFolderRecord other)
        {
            return NameHash.CompareTo(other.NameHash);
        }

        public void SetNameWithHash(string name)
        {
            Name = name;
            NameHash = BSAUtilities.GetHash(name, string.Empty);
        }
    }
}
