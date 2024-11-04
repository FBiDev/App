using System;
using App.Core.Desktop;

namespace App.File.Desktop
{
    public static class Json
    {
        public static string SerializeObject<T>(T value, bool indented = true)
        {
            try
            {
                return JsonAny.SerializeObject(value, indented);
            }
            catch (Exception ex)
            {
                ExceptionManager.Resolve(ex);
            }

            return string.Empty;
        }

        public static T DeserializeObject<T>(string value)
        {
            try
            {
                if (typeof(T) == typeof(JToken))
                {
                    var obj = new JToken();

                    Newtonsoft.Json.Linq.JToken token = JsonAny.DeserializeObject<Newtonsoft.Json.Linq.JToken>(value);

                    if (token != null)
                    {
                        foreach (Newtonsoft.Json.Linq.JProperty item in token)
                        {
                            obj.Add(new JProperty(item.Name, item.Value));
                        }
                    }

                    return (T)(object)obj;
                }

                return JsonAny.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                ExceptionManager.Resolve(ex);
            }

            return default(T);
        }

        public static bool Load<T>(T obj, string path)
        {
            try
            {
                return JsonAny.Load(obj, path);
            }
            catch (Exception ex)
            {
                ExceptionManager.Resolve(ex);
            }

            return false;
        }

        public static bool Save(object value, string path, bool indented = true, bool backslashReplace = false)
        {
            try
            {
                return JsonAny.Save(value, path, indented, backslashReplace);
            }
            catch (Exception ex)
            {
                ExceptionManager.Resolve(ex);
            }

            return false;
        }
    }
}