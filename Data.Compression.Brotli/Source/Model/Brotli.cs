using System.IO;
using System.IO.Compression;
using System.Text;
using Brotli;

namespace App.Data.Compression
{
    public static class Brotli
    {
        public static byte[] Compress(string data)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(data);

            byte[] compressedBytes;

            using (var outputStream = new MemoryStream())
            {
                using (var stream = new BrotliStream(outputStream, CompressionMode.Compress))
                {
                    stream.Write(inputBytes, 0, inputBytes.Length);
                }

                compressedBytes = outputStream.ToArray();

                return compressedBytes;
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var inputStream = new MemoryStream(data))
            {
                using (var stream = new BrotliStream(inputStream, CompressionMode.Decompress))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        stream.CopyTo(outputStream);
                        data = outputStream.ToArray();

                        return data;
                    }
                }
            }
        }
    }
}