using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace App.Core.Desktop
{
    public static class Archive
    {
        public static bool Exists(params string[] paths)
        {
            return File.Exists(Path.Combine(paths));
        }

        public static IEnumerable<string> GetFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.Exists(path) ? Directory.EnumerateFiles(path, searchPattern, searchOption) : Enumerable.Empty<string>();
        }

        public static bool IsLocked(string path)
        {
            if (!File.Exists(path)) { return false; }

            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        public static string RelativePath(string fileName)
        {
            var info = new FileInfo(fileName);
            string path = ".\\" + info.DirectoryName.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty) + "\\";
            return path;
        }

        public static string LastUpdate(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.GetLastWriteTime(fileName).ToString();
            }

            return string.Empty;
        }

        public static string MakeValidFileName(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, "_");
        }

        public static string FormatSize(double _bytes)
        {
            string unitSimbol = _bytes < 1024 ? "B" :
                _bytes < 1048576 ? "KB" :
                _bytes < 1073741824 ? "MB" :
                _bytes < 1099511627776 ? "GB" : "TB";

            double unitSize = _bytes < 1024 ? _bytes :
                _bytes < 1048576 ? _bytes / 1024 :
                _bytes < 1073741824 ? _bytes / 1024 / 1024 :
                _bytes < 1099511627776 ? _bytes / 1024 / 1024 / 1024 :
                _bytes / 1024 / 1024 / 1024 / 1024;

            if (unitSize < 10)
            {
                return (Math.Floor(unitSize * 100) / 100).ToString("n2") + " " + unitSimbol;
            }

            if (unitSize < 100)
            {
                return (Math.Floor(unitSize * 10) / 10).ToString("n1") + " " + unitSimbol;
            }

            return Math.Floor(unitSize) + " " + unitSimbol;
        }

        public static long CalculateSize(string size)
        {
            var units = new Dictionary<string[], long>
            {
                { new[] { "TiB", "TB" }, 1024L * 1024 * 1024 * 1024 },
                { new[] { "GiB", "GB" }, 1024L * 1024 * 1024 },
                { new[] { "MiB", "MB" }, 1024L * 1024 },
                { new[] { "KiB", "KB" }, 1024L },
                { new[] { "B" }, 1L }
            };

            foreach (var kvp in units)
            {
                foreach (var unit in kvp.Key)
                {
                    if (!size.Contains(unit)) { continue; }

                    var newSize = size.Replace(unit, string.Empty).Trim();
                    var value = Cast.ToDouble(newSize);
                    return Convert.ToInt64(value * kvp.Value);
                }
            }

            return 0;
        }

        public static string ReadAll(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        public static List<string> RemoveDuplicates(List<string> list)
        {
            var files = list.Select(f =>
            {
                using (FileStream fs = new FileStream(f, FileMode.Open, FileAccess.Read))
                {
                    // var crc32 = BitConverter.ToString(CRC32.Create().ComputeHash(fs));
                    // fs.Position = 0;
                    var md5 = BitConverter.ToString(MD5.Create().ComputeHash(fs));

                    return new
                    {
                        FileName = f,
                        MD5 = md5,
                        ////FileHash = sha1,
                    };
                }
            });

            files = files.Distinct();
            return files.Select(f => f.FileName).ToList();
        }

        public static List<string> RemoveImageSize(List<string> list, Size size)
        {
            var files = list.Where(f =>
            {
                var pic = new Picture(f);

                return pic.Size != size;
            });

            return files.ToList();
        }
    }
}