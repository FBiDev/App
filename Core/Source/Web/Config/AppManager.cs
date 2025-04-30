using System;
using System.IO;
using System.Web;

namespace App.Core.Web
{
    public static class AppManager
    {
        public static string Name
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

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