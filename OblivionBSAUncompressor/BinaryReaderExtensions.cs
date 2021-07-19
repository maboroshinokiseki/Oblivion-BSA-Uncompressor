using System.IO;
using System.Text;

namespace OblivionBSAUncompressor
{
    static class BinaryReaderExtensions
    {
        static public string ReadNullTerminatedString(this BinaryReader binaryReader)
        {
            var sb = new StringBuilder();
            char c;
            while ((c = binaryReader.ReadChar()) != '\0')
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        static public string ReadString(this BinaryReader br, int length)
        {
            var bytes = br.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
