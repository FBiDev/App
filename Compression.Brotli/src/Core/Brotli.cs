using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Brotli;

namespace App.Compression
{
    public static class Brotli
    {
        private static readonly byte[] CompressEmpty = new byte[1] { 59 };
        private static readonly byte[] DecompressEmpty = new byte[0];

        public static byte[] Compress(byte[] data)
        {
            if (data == null)
            {
                return CompressEmpty;
            }

            return CompressData(data);
        }

        public static byte[] Compress(string data)
        {
            if (data == null)
            {
                return CompressEmpty;
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(data);

            return CompressData(inputBytes);
        }

        public static byte[] Compress(string[] data)
        {
            if (data == null)
            {
                return CompressEmpty;
            }

            var dataString = string.Join(Environment.NewLine, data);

            byte[] inputBytes = Encoding.UTF8.GetBytes(dataString);

            return CompressData(inputBytes);
        }

        public static byte[] Decompress(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return DecompressEmpty;
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