using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Core;

namespace App.File
{
    internal static class JsonAny
    {
        private static readonly string DllName = "Newtonsoft.Json";
        private static readonly string DllFile = DllName + ".dll";

        private static Assembly assembly;

        private static Type jsonClass;
        private static Type enumFormatting;
        private static Type serializerSettings;
        private static MethodInfo serializeObjectMethod;
        private static MethodInfo deserializeObjectMethod;

        public static T DeserializeObject<T>(string value) where T : class
        {
            if (LoadAssembly() == false)
            {
                return null;
            }

            deserializeObjectMethod = jsonClass.GetMethods()
                    .FirstOrDefault(m => m.Name == "DeserializeObject" && m.IsGenericMethod).MakeGenericMethod(typeof(T));

            var result = deserializeObjectMethod.Invoke(null, new object[] { value });

            return (T)result;
        }

        public static string SerializeObject(object value, bool indented = true)
        {
            if (LoadAssembly() == false)
            {
                return null;
            }

            var formatting = Enum.ToObject(enumFormatting, indented.ToInt());
            var settings = JsonSettings.Get();
            var result = (string)serializeObjectMethod.Invoke(null, new object[] { value, formatting, settings });

            return result;
        }

        public static bool Load<T>(ref T obj, string path) where T : class
        {
            if (System.IO.File.Exists(path))
            {
                var jsonData = System.IO.File.ReadAllText(path);
                obj = DeserializeObject<T>(jsonData);
                return true;
            }

            return false;
        }

        public static bool Save(object value, string path, bool indented = true, bool backslashReplace = false)
        {
            var jsonData = SerializeObject(value, indented);

            if (backslashReplace)
            {
                jsonData = jsonData.Replace(@"\\", "/");
            }

            var dir = Path.GetDirectoryName(path);
            var created = dir != string.Empty ? Directory.CreateDirectory(dir) : null;

            System.IO.File.WriteAllText(path, jsonData, Encoding.UTF8);
            return true;
        }

        internal static bool LoadAssembly()
        {
            if (assembly == null)
            {
                try
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var dllPath = Path.Combine(assemblyFolder, DllFile);

                    assembly = Assembly.LoadFrom(dllPath);
                    jsonClass = assembly.GetType(DllName + ".JsonConvert");
                    enumFormatting = assembly.GetType(DllName + ".Formatting");
                    serializerSettings = assembly.GetType(DllName + ".JsonSerializerSettings");
                    serializeObjectMethod = jsonClass.GetMethod("SerializeObject", new Type[] { typeof(object), enumFormatting, serializerSettings });
                }
                catch (Exception)
                {
                    throw new DllNotFoundException("DLL not found:\r\n" + DllFile);
                }
            }

            return assembly != null & jsonClass != null & enumFormatting != null;
        }
    }
}