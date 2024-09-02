using System;
using App.Core.Desktop;

namespace App.File.Desktop
{
    public static class Json
    {
        public static T DeserializeObject<T>(string value) where T : class, new()
        {
            try
            {
                return JsonAny.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                ExceptionManager.Resolve(ex);
            }

            return new T();
        }

        public static bool Load<T>(ref T obj, string path) where T : class
        {
            try
            {
                return JsonAny.Load(ref obj, path);
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