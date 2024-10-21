using System;
using System.IO;
using System.Linq;

namespace App.Core
{
    public class HexFile
    {
        public HexFile(byte[] fileContent)
        {
            Content = fileContent;
        }

        private byte[] Content { get; set; }

        public void Replace(string oldValue, string newValue)
        {
            var strHex = ConvertToString();
            Content = ConvertToByteArray(strHex.Replace(oldValue, newValue));
        }

        public void Save(string newFileName)
        {
            File.WriteAllBytes(newFileName, Content);
        }

        private string ConvertToString()
        {
            return BitConverter.ToString(Content).Replace("-", string.Empty);
        }

        private byte[] ConvertToByteArray(string strHex)
        {
            return Enumerable.Range(0, strHex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(strHex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}