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
        private static readonly string EmptyJson = string.Empty;

        private static Assembly assembly;

        private static Type jsonClass;
        private static Type enumFormatting;
        private static Type serializerSettings;
        private static MethodInfo serializeObjectMethod;
        private static MethodInfo deserializeObjectMethod;

        public static T DeserializeObject<T>(string value)
        {
            if (LoadAssembly() == false)
            {
                return default(T);
            }

            deserializeObjectMethod = jsonClass.GetMethods()
                .FirstOrDefault(m =>
                {
                    return m.Name == "DeserializeObject"
                        && m.IsGenericMethod
                        && m.GetParameters().Any(p => p.ParameterType == serializerSettings);
                }).MakeGenericMethod(typeof(T));

            var settings = JsonSettings.Get();
            var result = deserializeObjectMethod.Invoke(null, new object[] { value, settings });

            if (result == null)
            {
                result = Activator.CreateInstance<T>();
            }

            return (T)result;
        }

        public static string SerializeObject(object value, bool indented = true)
        {
            if (LoadAssembly() == false || value == null)
            {
                return EmptyJson;
            }

            var formatting = Enum.ToObject(enumFormatting, indented.ToInt());
            var settings = JsonSettings.Get();
            var result = (string)serializeObjectMethod.Invoke(null, new object[] { value, formatting, settings });

            if (string.IsNullOrWhiteSpace(result))
            {
                return EmptyJson;
            }

            return result;
        }

        public static bool Load<T>(T obj, string path)
        {
            if (string.IsNullOrWhiteSpace(path) || System.IO.File.Exists(path) == false || obj == null)
            {
                return false;
            }

            var jsonData = System.IO.File.ReadAllText(path);

            T result = DeserializeObject<T>(jsonData);

            obj.CloneProperties(result);

            return true;
        }

        public static bool Save(object value, string path, bool indented = true, bool backslashReplace = false)
        {
            var jsonData = SerializeObject(value, indented);

            if (jsonData == EmptyJson || string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

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