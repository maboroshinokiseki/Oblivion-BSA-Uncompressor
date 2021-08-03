using System.IO;
using System.Text;

namespace OblivionBSAUncompressor
{
    static class BinaryWriterExtensions
    {
        static public void WriteString(this BinaryWriter binaryWriter, string value, bool startWithSize, bool endWithNull)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var size = bytes.Length;
            if (endWithNull)
            {
                size++;
            }

            if (startWithSize)
            {
                binaryWriter.Write((byte)size);
            }

            binaryWriter.Write(bytes);

            if (endWithNull)
            {
                binaryWriter.Write((byte)0);
            }
        }
    }
}
