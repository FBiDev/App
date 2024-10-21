using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace App.Core
{
    public static class Brotli
    {
        private static readonly string DllName = "Brotli.Core";

        private static readonly string DllFile = DllName + ".dll";

        private static Assembly assembly;

        private static Type brotliClass;

        public static byte[] Decompress(byte[] data)
        {
            if (LoadAssembly() == false)
            {
                return null;
            }

            var args = new object[] { new MemoryStream(data), CompressionMode.Decompress };
            using (var stream = (Stream)Activator.CreateInstance(brotliClass, args))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    data = memoryStream.ToArray();

                    return data;
                }

                // using (var reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
            }
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
                    brotliClass = assembly.GetType("Brotli.BrotliStream");
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