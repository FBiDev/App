using System;

namespace App.Core
{
    public static class BoolExtension
    {
        public static byte ToInt(this bool b)
        {
            return Convert.ToByte(b);
        }
    }
}