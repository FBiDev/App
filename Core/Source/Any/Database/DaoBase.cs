using System.Collections.Generic;

namespace App.Core
{
    public class DaoBase
    {
        public static KeyValuePair<string, object> P<T>(string key, T value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}