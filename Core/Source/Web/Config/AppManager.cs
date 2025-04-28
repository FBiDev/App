using System;
using System.IO;

namespace App.Core.Web
{
    public static class AppManager
    {
        public static string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string GetBaseFile(string filepath)
        {
             return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath); 
        }
    }
}