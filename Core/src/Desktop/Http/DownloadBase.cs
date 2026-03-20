using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Desktop
{
    public class DownloadBase
    {
        public DownloadBase()
        {
            Overwrite = true;
            Files = new List<DownloadFile>();
            FilesToDownload = new List<DownloadFile>();
        }

        public event Action ProgressChanged = delegate { };

        protected enum DownloadStatus
        {
            Connecting,
            ProgressChanged,
            FileDownloaded,
            NextFiles,
            Completed,
            Stopped,
        }

        public string FolderBase { get; set; }

        public List<DownloadFile> Files { get; set; }

        public List<DownloadFile> FilesToDownload { get; set; }

        public int FilesCompleted { get; set; }

        public bool Overwrite { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeCompleted { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public long BytesReceived { get; set; }

        public long TotalBytesToReceive { get; set; }

        public int Percentage { get; set; }

        public string Result { get; set; }

        public bool Error { get; set; }

        public string ErrorMessage { get; set; }

        protected DownloadStatus Status { get; set; }

        public void SetFile(DownloadFile file)
        {
            Files = new List<DownloadFile> { file };
        }

        public virtual async Task<bool> Start()
        {
            if (string.IsNullOrWhiteSpace(FolderBase))
            {
                FolderBase = @".\";
            }

            TimeStart = DateTime.Now;
            TimeCompleted = TimeStart;
            TimeElapsed = default(TimeSpan);

            // Remove Files with same URL
            Files = Files.Distinct().ToList();
            FilesCompleted = 0;

            BytesReceived = 0;
            TotalBytesToReceive = 0;
            Percentage = 0;

            var tasks = new List<Task>();
            string connecting = "Connecting..." + Environment.NewLine;
            Result = connecting;

            Status = DownloadStatus.Connecting;
            ProgressChanged();

            FilesToDownload.Clear();

            if (Overwrite == false)
            {
                var di = new DirectoryInfo(FolderBase);
                var physicalFiles = di.GetFiles("*.*", SearchOption.AllDirectories)
                                    .Where(fs => fs.Length > 0)
                                    .Select(f => Archive.RelativePath(f.FullName) + Path.GetFileName(f.Name));

                var fileNames = Files.Select(f => f.Path);

                var setOfFiles = new HashSet<string>(physicalFiles);
                var notPresent = (from name in fileNames
                                  where setOfFiles.Contains(name) == false
                                  select name).ToList();

                notPresent.Sort();
                Files = Files.OrderBy(f => f.Path).ToList();

                // Add files that not already downloaded
                foreach (DownloadFile file in Files)
                {
                    foreach (string nt in notPresent)
                    {
                        if (file.Path.Equals(nt))
                        {
                            FilesToDownload.Add(file);
                            break;
                        }
                    }
                }
            }
            else
            {
                FilesToDownload.AddRange(Files);
            }

            int filesTotal = FilesToDownload.Count;

            foreach (DownloadFile file in FilesToDownload)
            {
                if (file == null)
                {
                    continue;
                }

                using (var client = new WebClientExtend())
                {
                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        if (args.TotalBytesToReceive > 0 && args.TotalBytesToReceive > file.TotalBytesToReceive)
                        {
                            TotalBytesToReceive += args.TotalBytesToReceive - file.TotalBytesToReceive;
                            file.TotalBytesToReceive = args.TotalBytesToReceive;
                        }

                        if (args.BytesReceived > 0 && args.BytesReceived > file.BytesReceived)
                        {
                            BytesReceived += args.BytesReceived - file.BytesReceived;
                            file.BytesReceived = args.BytesReceived;
                        }

                        float ProgressPercentage = 0;
                        file.ProgressPercentage = args.ProgressPercentage;

                        foreach (DownloadFile f in FilesToDownload)
                        {
                            ProgressPercentage += f.ProgressPercentage / filesTotal;
                        }

                        Percentage = ProgressPercentage > 100 ? 100 : (int)Math.Ceiling(ProgressPercentage);

                        CalculateResult();

                        Status = DownloadStatus.ProgressChanged;
                        ProgressChanged();
                    };

                    client.DownloadFileCompleted += (sender, args) =>
                    {
                        FilesCompleted++;
                        Status = DownloadStatus.FileDownloaded;

                        Error = client.Error;
                        ErrorMessage = client.ErrorMessage;

                        // Downloaded All Files
                        if (Percentage == 100)
                        {
                            Status = DownloadStatus.Completed;
                        }
                        else
                        {
                            CalculateResult();
                        }
                    };

                    tasks.Add(client.DownloadFileTaskAsync(new Uri(file.URL), file.Path));

                    if (tasks.Count == Browser.MaxConnections)
                    {
                        Status = DownloadStatus.NextFiles;
                        ProgressChanged();

                        await Task.WhenAll(tasks);
                        tasks.Clear();
                    }
                }
            }

            try
            {
                await Task.WhenAll(tasks.Where(i => i != null));

                if (Percentage == 100)
                {
                    BytesReceived = 0;
                    TotalBytesToReceive = 0;
                    foreach (DownloadFile f in FilesToDownload)
                    {
                        BytesReceived += f.BytesReceived;
                        TotalBytesToReceive += f.TotalBytesToReceive;
                    }

                    FilesCompleted = filesTotal;

                    TimeElapsed = new TimeSpan(DateTime.Now.Ticks - TimeStart.Ticks);
                    TimeCompleted = DateTime.Now;

                    CalculateResult();
                }
                else
                {
                    Status = DownloadStatus.Stopped;
                    Result = Result == connecting ? "Files already exist" : Result;
                }

                ProgressChanged();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }

            return !Error;
        }

        private void CalculateResult()
        {
            int filesTotal = FilesToDownload.Count;
            string progressBytes = string.Empty;
            string progressFiles = string.Empty;

            progressBytes = "Downloaded " + DownloadedProgress(BytesReceived, TotalBytesToReceive);
            progressFiles = " (" + FilesCompleted + "/" + filesTotal + ")";
            Result = progressBytes + progressFiles;
        }

        private string DownloadedProgress(double bytesIn, double bytesTotal)
        {
            var bytesProgress = Archive.CalculateSize(bytesIn);
            double bytesCalc = bytesTotal <= 0 ? bytesIn : bytesTotal;
            bytesProgress += " of " + Archive.CalculateSize(bytesCalc);

            return bytesProgress;
        }
    }
}