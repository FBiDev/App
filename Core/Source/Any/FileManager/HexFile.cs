using System;
using System.Linq;
using System.IO;

namespace App.Core
{
    public class HexFile
    {
        byte[] Content { get; set; }

        public HexFile(byte[] fileContent)
        {
            Content = fileContent;
        }

        string ConvertToString()
        {
            return BitConverter.ToString(Content).Replace("-", "");
        }

        byte[] ConvertToByteArray(string strHex)
        {
            return Enumerable.Range(0, strHex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(strHex.Substring(x, 2), 16))
                             .ToArray();
        }

        public void Replace(string oldValue, string newValue)
        {
            var strHex = ConvertToString();
            Content = ConvertToByteArray(strHex.Replace(oldValue, newValue));
        }

        public void Save(string newFileName)
        {
            File.WriteAllBytes(newFileName, Content);
        }
    }
}