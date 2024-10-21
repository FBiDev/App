using System.IO;

namespace System.Security.Cryptography
{
    public class CRC32
    {
        private static readonly CRC32 Instance = new CRC32();

        private readonly uint[] checksumTable;
        private readonly uint polynomial = 0xEDB88320;

        protected CRC32()
        {
            checksumTable = new uint[0x100];

            for (uint index = 0; index < 0x100; ++index)
            {
                uint item = index;
                for (int bit = 0; bit < 8; ++bit)
                {
                    item = ((item & 1) != 0) ? (polynomial ^ (item >> 1)) : (item >> 1);
                }

                checksumTable[index] = item;
            }
        }

        public static CRC32 Create()
        {
            return Instance;
        }

        public byte[] ComputeHash(Stream stream)
        {
            uint result = 0xFFFFFFFF;

            int current;
            while ((current = stream.ReadByte()) != -1)
            {
                result = checksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);
            }

            ////Back FileStream to begin
            ////stream.Position = 0;

            var hash = BitConverter.GetBytes(~result);
            Array.Reverse(hash);
            return hash;
        }

        public byte[] ComputeHash(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return ComputeHash(stream);
            }
        }
    }
}