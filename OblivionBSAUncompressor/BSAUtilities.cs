using System;
using System.IO;

namespace OblivionBSAUncompressor
{
    static class BSAUtilities
    {
        public static ulong GetHash(string name)
        {
            return GetHash(Path.GetFileNameWithoutExtension(name), Path.GetExtension(name));
        }

        public static ulong GetHash(string name, string ext)
        {
            name = name.ToLowerInvariant();
            name = name.Replace('/', '\\');
            ext = ext.ToLowerInvariant();
            var hashBytes = new byte[]
            {
                (byte)(name.Length == 0 ? '\0' : name[name.Length - 1]),
                (byte)(name.Length < 3 ? '\0' : name[name.Length - 2]),
                (byte)name.Length,
                (byte)(name.Length == 0 ? '\0' : name[0])
            };
            var hash1 = BitConverter.ToUInt32(hashBytes, 0);
            switch (ext)
            {
                case ".kf":
                    hash1 |= 0x80;
                    break;
                case ".nif":
                    hash1 |= 0x8000;
                    break;
                case ".dds":
                    hash1 |= 0x8080;
                    break;
                case ".wav":
                    hash1 |= 0x80000000;
                    break;
            }

            uint hash2 = 0;
            for (var i = 1; i < name.Length - 2; i++)
            {
                hash2 = hash2 * 0x1003f + (byte)name[i];
            }

            uint hash3 = 0;
            for (var i = 0; i < ext.Length; i++)
            {
                hash3 = hash3 * 0x1003f + (byte)ext[i];
            }

            return (((ulong)(hash2 + hash3)) << 32) + hash1;
        }

        public static byte[] DecompressData(byte[] compressedData, int uncompressedSize)
        {
            var uncompressedData = new byte[uncompressedSize];
            var inflater = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();
            inflater.SetInput(compressedData);
            var bytesWritten = inflater.Inflate(uncompressedData);
            var extBytes = inflater.Inflate(tempMem);
            if (extBytes > 0)
            {
                throw new ArgumentException("Incorrect uncompressedSize ");
            }

            return uncompressedData;
        }

        internal static void Requires<T>(bool condition, string message) where T: Exception, new()
        {
            if (!condition)
            {
                throw new BSAException(message, new T());
            }
        }

        // used to determine if inflater is finished
        static private byte[] tempMem = new byte[8];
    }
}
