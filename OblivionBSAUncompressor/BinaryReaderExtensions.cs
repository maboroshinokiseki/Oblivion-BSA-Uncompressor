using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OblivionBSAUncompressor
{
    static class BinaryReaderExtensions
    {
        static public string ReadString(this BinaryReader binaryReader, bool startWithSize, bool endWithNull)
        {
            int length;
            byte[] bytes;
            if (startWithSize)
            {
                length = binaryReader.ReadByte();
                bytes = binaryReader.ReadBytes(length);
                if (endWithNull)
                {
                    length--;
                }
            }
            else
            {
                byte b;
                var bytesList = new List<byte>();
                while ((b = binaryReader.ReadByte()) != 0)
                {
                    bytesList.Add(b);
                }

                length = bytesList.Count;
                bytes = bytesList.ToArray();
            }

            if (length > 0)
            {
                return Encoding.UTF8.GetString(bytes, 0, length);
            }

            return string.Empty;
        }
    }
}
