using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace App.Core
{
    public static class Brotli
    {
        static Assembly assembly;

        static readonly string DllName = "Brotli.Core";
        static readonly string DllFile = "" + DllName + ".dll";

        static Type BrotliClass;

        static bool LoadAssembly()
        {
            if (assembly == null)
            {
                try
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var dllPath = Path.Combine(assemblyFolder, DllFile);

                    assembly = Assembly.LoadFrom(dllPath);
                    BrotliClass = assembly.GetType("Brotli.BrotliStream");
                }
                catch (Exception)
                {
                    throw new DllNotFoundException("DLL not found:\r\n" + DllFile);
                }
            }

            return assembly != null & BrotliClass != null;
        }

        public static byte[] Decompress(byte[] data)
        {
            if (LoadAssembly() == false) return null;

            var args = new object[] { new MemoryStream(data), CompressionMode.Decompress };
            using (var stream = (Stream)Activator.CreateInstance(BrotliClass, args))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    data = memoryStream.ToArray();

                    return data;
                }
                //using (var reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
            }
        }
    }
}