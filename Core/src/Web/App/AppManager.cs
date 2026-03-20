using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace App.Core.Web
{
    public static class AppManager
    {
        public static string Name
        {
            get { return Assembly.GetCallingAssembly().GetName().Name; }
        }

        public static string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        private static bool IsRunningLocal
        {
            get { return HttpContext.Current.Request.IsLocal; }
        }

        private static string URLHost
        {
            get
            {
                var request = HttpContext.Current.Request;
                return request.Url.Scheme + "://" + request.Url.Authority;
            }
        }

        private static string URLBaseFolder
        {
            get
            {
                if (IsRunningLocal)
                {
                    return HostingEnvironment.ApplicationVirtualPath;
                }

                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        public static string URLBuilder(string urlPage)
        {
            var pageUrl = Regex.Replace(URLBaseFolder + "/" + urlPage, "/{2,}", "/");
            var completeUrl = URLHost + pageUrl;
            return completeUrl;
        }

        public static string GetBaseFile(string filepath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath);
        }
    }
}