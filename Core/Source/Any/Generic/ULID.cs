using System;
using System.Security.Cryptography;

namespace App.Core
{
    public class ULID
    {
        private static readonly string CrockfordBase32 = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        private static readonly DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ULID()
        {
            Ulid = NewULID();

            SetValues();
        }

        public ULID(string uLID)
        {
            Ulid = uLID;

            SetValues();
        }

        public string Ulid { get; set; }

        public string Base32Timestamp { get; set; }

        public string Base32Random { get; set; }

        public ulong Timestamp { get; set; }

        public int[] TimestampDecimal { get; set; }

        public string[] TimestampBinary { get; set; }

        public string TimestampHex { get; set; }

        public DateTime DateLocal { get; set; }

        public DateTime DateEST { get; set; }

        public DateTime DateUTC { get; set; }

        public override string ToString()
        {
            return Ulid;
        }

        private string NewULID()
        {
            // Data (128 bits = 16 bytes)
            byte[] data = new byte[16];

            long currentTimestampMs = (long)(DateTime.UtcNow - UnixDateTime).TotalMilliseconds;

            // Timestamp (48 bits = 6 bytes)
            for (int i = 5; i >= 0; i--)
            {
                data[i] = (byte)(currentTimestampMs & 0xFF);
                currentTimestampMs >>= 8;
            }

            // Random (80 bits = 10 bytes)
            byte[] randomness = new byte[10];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomness);
            }

            Array.Copy(randomness, 0, data, 6, 10);

            return Encode(data);
        }

        private void SetValues()
        {
            Base32Timestamp = Ulid.Substring(0, 10);
            Base32Random = Ulid.Substring(10, 16);

            Timestamp = Decode(Base32Timestamp);
            TimestampDecimal = EncodeToDecimal(Timestamp);

            string binary = Convert.ToString((long)Timestamp, 2).PadLeft(48, '0');

            int chuncksCount = (int)Math.Ceiling(binary.Length / 5.0);
            TimestampBinary = new string[chuncksCount];

            int index = chuncksCount - 1;
            for (int pos = binary.Length; pos > 0; pos -= 5)
            {
                int start = Math.Max(0, pos - 5);
                int length = pos - start;
                TimestampBinary[index--] = binary.Substring(start, length);
            }

            TimestampHex = Timestamp.ToString("X12");

            DateUTC = UnixDateTime.AddMilliseconds(Timestamp);
            DateLocal = DateUTC.ToLocalTime();

            // Eastern Time (New York/Washington)
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateEST = TimeZoneInfo.ConvertTimeFromUtc(DateUTC, easternZone);
        }

        #region Encode
        private int[] EncodeToDecimal(ulong value)
        {
            var minLength = 10;

            int[] result = new int[minLength];
            for (int i = minLength - 1; i >= 0; i--)
            {
                result[i] = (int)(value % 32);
                value /= 32;
            }

            return result;
        }

        private string Encode(byte[] data)
        {
            int minLength = 26;

            byte[] buffer = (byte[])data.Clone();
            string result = string.Empty;

            while (!IsAllZero(buffer))
            {
                int remainder = DivMod(buffer, 32);
                result = CrockfordBase32[remainder] + result;
            }

            if (result.Length == 0)
            {
                result = "0".PadLeft(minLength, '0');
            }

            return result.PadLeft(minLength, '0');
        }

        private int DivMod(byte[] buffer, int divisor)
        {
            int remainder = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                int value = (remainder << 8) + buffer[i];
                buffer[i] = (byte)(value / divisor);
                remainder = value % divisor;
            }

            return remainder;
        }

        private bool IsAllZero(byte[] buffer)
        {
            foreach (byte b in buffer)
            {
                if (b != 0)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Decode
        private ulong Decode(string input)
        {
            ulong result = 0;

            foreach (char c in input)
            {
                uint value = (uint)CrockfordBase32.IndexOf(c);
                if (value < 0)
                {
                    throw new ArgumentException("Caractere inválido: " + c);
                }

                result = (result << 5) | value;
            }

            return result;
        }
        #endregion
    }
}