using System.Collections.Generic;

namespace App.Core
{
    public class DaoBase
    {
        public static KeyValuePair<string, object> P(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}