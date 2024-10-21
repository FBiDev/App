using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Desktop
{
    public class WebClientExtend : WebClient
    {
        private const string GZipExtension = ".gz";

        private bool _gZipEnable;

        private string _errorMessage;

        public WebClientExtend()
        {
            GZipEnable = true;

            Encoding = Encoding.UTF8;
            Proxy = Browser.Proxy;

            CustomErrorMessages = new Dictionary<string, string>();
            CookieContainer = new CookieContainer();
        }

        public static Dictionary<string, string> CustomErrorMessages { get; set; }

        public bool GZipEnable
        {
            get
            {
                return _gZipEnable;
            }

            set
            {
                _gZipEnable = value;

                if (_gZipEnable)
                {
                    Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
                }
                else
                {
                    Headers[HttpRequestHeader.AcceptEncoding] = string.Empty;
                }
            }
        }

        public bool Error { get; private set; }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                var link = CustomErrorMessages.SingleOrDefault(x => value.Contains(x.Key));

                if (link.Value != null)
                {
                    _errorMessage += link.Value;
                }
                else
                {
                    _errorMessage = value;
                }
            }
        }

        public CookieContainer CookieContainer { get; private set; }

        private bool IsGZipContent
        {
            get { return HeaderExist("Content-Encoding") && ResponseHeaders[HttpResponseHeader.ContentEncoding] == "gzip"; }
        }

        private bool IsBrotliContent
        {
            get { return HeaderExist("Content-Encoding") && ResponseHeaders[HttpResponseHeader.ContentEncoding] == "br"; }
        }

        private long GZipSize { get; set; }

        private long GZipSizeUncompressed { get; set; }

        private DownloadFile FileDownloaded { get; set; }

        public bool HeaderExist(string headerName)
        {
            return ResponseHeaders != null && ResponseHeaders.AllKeys.Any(h => h.ToLower() == headerName.ToLower());
        }

        public string HeaderValue(string headerName)
        {
            return HeaderExist(headerName) ? ResponseHeaders.AllKeys.Single(h => h.ToLower() == headerName.ToLower()) : string.Empty;
        }

        public new Task DownloadFileTaskAsync(string address, string fileName)
        {
            return DownloadFileTaskAsync(new Uri(address), fileName);
        }

        public new Task DownloadFileTaskAsync(Uri address, string fileName)
        {
            FileDownloaded = new DownloadFile(address.ToString(), fileName);

            if (string.IsNullOrWhiteSpace(FileDownloaded.Name))
            {
                return null;
            }

            fileName = SetTempFile(fileName);
            FileDownloaded = new DownloadFile(address.ToString(), fileName);

            return base.DownloadFileTaskAsync(address, fileName);
        }

        public new async Task<string> UploadValuesTaskAsync(Uri address, NameValueCollection data)
        {
            byte[] response = default(byte[]);
            string responseString = string.Empty;
            try
            {
                response = await UploadValuesTaskAsync(address, "POST", data);

                if (IsGZipContent)
                {
                    response = DecodeGZip(response);
                }
                else if (IsBrotliContent)
                {
                    response = Brotli.Decompress(response);
                }

                responseString = Encoding.UTF8.GetString(response);
            }
            catch (WebException we)
            {
                Error = true;
                ErrorMessage = we.Message;
            }

            return responseString;
        }

        public async new Task<string> DownloadString(string address)
        {
            string msg = string.Empty;
            byte[] data = await DownloadData(address);

            if (Error)
            {
                return msg;
            }

            if (IsBrotliContent)
            {
                data = Brotli.Decompress(data);
            }

            if (ResponseHeaders == null)
            {
                return msg;
            }

            string contentType = ResponseHeaders[HttpResponseHeader.ContentType];

            if (contentType.IndexOf("ISO-8859-1", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;

                byte[] isoBytes = data;
                var utfBytes = Encoding.Convert(iso, utf8, isoBytes);
                msg = utf8.GetString(utfBytes);
            }
            else if (contentType.IndexOf("image/", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                msg = "data:" + contentType + ";base64," + Convert.ToBase64String(data);
            }
            else
            {
                msg = Encoding.GetString(data);
            }

            return msg;
        }

        public async new Task<byte[]> DownloadData(string address)
        {
            byte[] data = new byte[0];

            try
            {
                Error = false;
                data = await DownloadDataTaskAsync(new Uri(address));
            }
            catch (WebException we)
            {
                if (Error)
                {
                    return data;
                }

                Error = true;

                if (we.Response == null)
                {
                    ErrorMessage = "Download Error: \r\n\r\n" + "Status: " + we.Status + "\r\n\r\n" + we.Message + "\r\n\r\n" + we.InnerException.Message;
                }
                else
                {
                    var response = we.Response as HttpWebResponse;
                    ErrorMessage = "Download Error: \r\n\r\n" + "Status Code: " + (int)response.StatusCode + " " + response.StatusDescription;
                }
            }

            if (IsGZipContent)
            {
                GetGZipSize(data);
                data = DecodeGZip(data);
            }

            return data;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            request.CookieContainer = CookieContainer;

            if (FileDownloaded == null)
            {
                // request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return request;
        }

        // For DownloadString
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return base.GetWebResponse(request);
        }

        // For DownloadFile
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            HttpWebResponse response = null;
            try
            {
                Error = false;
                response = base.GetWebResponse(request, result) as HttpWebResponse;
            }
            catch (WebException we)
            {
                Error = true;
                if (we.Response == null)
                {
                    ErrorMessage = "Download Error: \r\n\r\n" + "Status: " + we.Status + "\r\n\r\n" + we.Message;
                }
                else
                {
                    response = we.Response as HttpWebResponse;
                    if (FileDownloaded == null)
                    {
                        ErrorMessage = "Error to Access: \r\n\r\n";
                        ErrorMessage += response.ResponseUri.AbsoluteUri;
                    }
                    else
                    {
                        ErrorMessage = "Error to download: \r\n\r\n";
                        ErrorMessage += FileDownloaded.URL;
                    }

                    ErrorMessage += "\r\n\r\n" + "Status Code: " + (int)response.StatusCode + " " + response.StatusDescription;
                }
            }

            return response;
        }

        protected override void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            if (FileDownloaded != null)
            {
                FileDownloaded.BytesReceived = e.BytesReceived;
                FileDownloaded.TotalBytesToReceive = e.TotalBytesToReceive;
                FileDownloaded.ProgressPercentage = e.ProgressPercentage;
            }

            base.OnDownloadProgressChanged(e);
        }

        protected override void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
        {
            if (Error)
            {
                File.Delete(FileDownloaded.Path);
                base.OnDownloadFileCompleted(e);
                return;
            }

            RevertTempFile(FileDownloaded.Path);

            if (IsBrotliContent)
            {
                var encfileData = File.ReadAllBytes(FileDownloaded.Path);
                var decfileData = Brotli.Decompress(encfileData);
                File.WriteAllBytes(FileDownloaded.Path, decfileData);
            }
            else if (IsGZipContent)
            {
                var fileToDecompress = new FileInfo(FileDownloaded.Path);

                string oldName = fileToDecompress.FullName;
                string gzName = oldName + GZipExtension;

                File.Delete(gzName);
                File.Move(oldName, gzName);
                fileToDecompress = new FileInfo(gzName);

                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        originalFileStream.CopyTo(ms);
                        originalFileStream.Position = 0;
                        GetGZipSize(ms.ToArray());
                    }

                    using (FileStream decompressedFileStream = File.Create(oldName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }
                }

                File.Delete(gzName);
            }

            base.OnDownloadFileCompleted(e);
            return;
        }

        private string SetTempFile(string fileName)
        {
            return fileName + ".tmp";
        }

        private void RevertTempFile(string fileName)
        {
            var newFile = fileName.Substring(0, fileName.Length - 4);
            FileDownloaded.Path = newFile;

            if (File.Exists(newFile))
            {
                File.Delete(newFile);
            }

            File.Move(fileName, newFile);
        }

        private void GetGZipSize(byte[] data)
        {
            if (IsGZipContent)
            {
                GZipSize = data.Length;

                byte[] last4 = new byte[4];
                last4[0] = data[data.Length - 4];
                last4[1] = data[data.Length - 3];
                last4[2] = data[data.Length - 2];
                last4[3] = data[data.Length - 1];
                GZipSizeUncompressed = BitConverter.ToUInt32(last4, 0);
            }
        }

        private byte[] DecodeGZip(byte[] gzBuffer)
        {
            var isGZip = gzBuffer.Length >= 2 && gzBuffer[0] == 31 && gzBuffer[1] == 139;

            if (isGZip == false)
            {
                return gzBuffer;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                var msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 0, gzBuffer.Length);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                int length;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    length = zip.Read(buffer, 0, buffer.Length);
                }

                byte[] data = new byte[length];
                Array.Copy(buffer, data, length);
                return data;
            }
        }
    }
}