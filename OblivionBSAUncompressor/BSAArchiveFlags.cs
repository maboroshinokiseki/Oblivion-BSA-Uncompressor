using System;

namespace OblivionBSAUncompressor
{
    [Flags]
    enum BSAArchiveFlags
    {
        HasDirectoryNames = 0x0001,
        HasFileNames = 0x0002,
        Compressed = 0x0004,
        RetainDirectoryNames = 0x0008,          // NotSure
        RetainFileNames = 0x0010,               // NotSure
        RetainFileNameOffsets = 0x0020,         // NotSure
        BigEndian = 0x0040,
        Unknown1For103 = 0x0080,                // unknown in 103
        Unknown2For103 = 0x0100,                // unknown in 103 but must be set
        Unknown3For103 = 0x0200,                // unknown in 103 but must be set
        Unknown4For103 = 0x0400,                // unknown in 103 but must be set
        RetainStringsDuringStartup = 0x0080,    // 104 only
        EmbedFileName = 0x0100,                 // 104 only
        XMemCodec = 0x0200,                     // 104 only
    }
}
