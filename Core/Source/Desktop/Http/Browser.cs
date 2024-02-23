using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace App.Core.Desktop
{
    public static class Browser
    {
        public static int MaxConnections { get { return ServicePointManager.DefaultConnectionLimit; } }
        public static bool UseProxy { get; set; }
        public static WebProxy DefaultProxy { get; set; }

        public static WebProxy Proxy
        {
            get
            {
                if (UseProxy == false) { return new WebProxy(); }
                return DefaultProxy;
            }
        }

        public static void Load()
        {
            DefaultProxy = new WebProxy();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.DefaultConnectionLimit = 128;

            //var j = Json.DeserializeObject<object>("{\"LoadJsonDLL\":1}");
        }

        static readonly Random rand = new Random();
        public async static Task<string> DownloadString(string url, bool addRandomNumber = false)
        {
            return await Task.Run(async () =>
            {
                string data = string.Empty;

                using (var client = new WebClientExtend())
                {
                    url = addRandomNumber ? url +=
                          (url.IndexOf("?", StringComparison.Ordinal) < 0 ? "?" : "&") + "random=" + rand.Next()
                        : (url);

                    data = await client.DownloadString(url);

                    if (client.Error)
                    {
                        MessageBox.Show(client.ErrorMessage);
                    }
                    //if (client.HeaderValue("X-Cache") != "HIT") { var a = 1; }
                }

                return data;
            });
        }
    }
}