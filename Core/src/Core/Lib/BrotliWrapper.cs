using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace App.Core
{
    public static class BrotliWrapper
    {
        private static readonly string DllName = "App.Data.Compression.Brotli";
        private static readonly string DllFile = DllName + ".dll";
        private static readonly byte[] CompressEmpty = new byte[1] { 59 };
        private static readonly byte[] DecompressEmpty = new byte[0];

        private static Assembly assembly;

        private static Type brotliClass;

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
            if (LoadAssembly() == false)
            {
                return null;
            }

            if (data == null || data.Length == 0)
            {
                return DecompressEmpty;
            }

            var methodInfo = brotliClass.GetMethod("Decompress");
            var decompressedBytes = methodInfo.Invoke(brotliClass, new object[] { data });

            return (byte[])decompressedBytes;
        }

        private static byte[] CompressData(byte[] data)
        {
            if (LoadAssembly() == false)
            {
                return data;
            }

            var methodInfo = brotliClass.GetMethod("Compress", new Type[] { data.GetType() });
            var compressedBytes = methodInfo.Invoke(brotliClass, new object[] { data });

            return (byte[])compressedBytes;
        }

        private static bool LoadAssembly()
        {
            if (assembly == null)
            {
                try
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var dllPath = Path.Combine(assemblyFolder, DllFile);

                    assembly = Assembly.LoadFrom(dllPath);
                    brotliClass = assembly.GetType("App.Data.Compression.Brotli");
                }
                catch (Exception)
                {
                    throw new DllNotFoundException("DLL not found:\r\n" + DllFile);
                }
            }

            return assembly != null & brotliClass != null;
        }
    }
}