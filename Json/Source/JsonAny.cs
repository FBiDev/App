using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Core;

namespace App.Json
{
    static class JsonAny
    {
        static Assembly assembly;

        static readonly string DllName = "Newtonsoft.Json";
        static readonly string DllFile = "" + DllName + ".dll";

        static Type JsonClass;
        static Type EnumFormatting;
        static Type SerializerSettings;
        static MethodInfo SerializeObjectMethod;
        static MethodInfo DeserializeObjectMethod;

        internal static bool LoadAssembly()
        {
            if (assembly == null)
            {
                try
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var dllPath = Path.Combine(assemblyFolder, DllFile);

                    assembly = Assembly.LoadFrom(dllPath);
                    JsonClass = assembly.GetType(DllName + ".JsonConvert");
                    EnumFormatting = assembly.GetType(DllName + ".Formatting");
                    SerializerSettings = assembly.GetType(DllName + ".JsonSerializerSettings");
                    SerializeObjectMethod = JsonClass.GetMethod("SerializeObject", new Type[] { typeof(object), EnumFormatting, SerializerSettings });
                }
                catch (Exception)
                {
                    throw new DllNotFoundException("DLL not found:\r\n" + DllFile);
                }
            }

            return assembly != null & JsonClass != null & EnumFormatting != null;
        }

        public static T DeserializeObject<T>(string value) where T : class
        {
            if (LoadAssembly() == false) return null;

            DeserializeObjectMethod = JsonClass.GetMethods()
                    .FirstOrDefault(m => m.Name == "DeserializeObject" && m.IsGenericMethod).MakeGenericMethod(typeof(T));

            var result = DeserializeObjectMethod.Invoke(null, new object[] { value });

            return (T)result;
        }

        public static string SerializeObject(object value, bool indented = true)
        {
            if (LoadAssembly() == false) return null;

            var formatting = Enum.ToObject(EnumFormatting, indented.ToInt());
            var settings = JsonSettings.Get();
            var result = (string)SerializeObjectMethod.Invoke(null, new object[] { value, formatting, settings });

            return result;
        }

        public static bool Load<T>(ref T obj, string path) where T : class
        {
            if (File.Exists(path))
            {
                var jsonData = File.ReadAllText(path);
                obj = DeserializeObject<T>(jsonData);
                return true;
            }
            return false;
        }

        public static bool Save(object value, string path, bool indented = true, bool backslashReplace = false)
        {
            var jsonData = SerializeObject(value, indented);

            if (backslashReplace) jsonData = jsonData.Replace(@"\\", "/");

            File.WriteAllText(path, jsonData, Encoding.UTF8);
            return true;
        }
    }
}