using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Brotli;

namespace App.Data.Compression
{
    public static class Brotli
    {
        public static byte[] Compress(byte[] data)
        {
            data = data ?? new byte[0];

            return CompressData(data);
        }

        public static byte[] Compress(string data)
        {
            data = data ?? string.Empty;

            byte[] inputBytes = Encoding.UTF8.GetBytes(data);

            return CompressData(inputBytes);
        }

        public static byte[] Compress(string[] data)
        {
            data = data ?? new string[1] { "" };
            var dataString = string.Join(Environment.NewLine, data);

            byte[] inputBytes = Encoding.UTF8.GetBytes(dataString);

            return CompressData(inputBytes);
        }

        public static byte[] Decompress(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return new byte[0];
            }

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

        private static byte[] CompressData(byte[] data)
        {
            byte[] compressedBytes;

            using (var outputStream = new MemoryStream())
            {
                using (var stream = new BrotliStream(outputStream, CompressionMode.Compress))
                {
                    stream.Write(data, 0, data.Length);
                }

                compressedBytes = outputStream.ToArray();

                return compressedBytes;
            }
        }
    }
}