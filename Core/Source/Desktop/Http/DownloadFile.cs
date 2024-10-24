﻿using System;
using System.Linq;

namespace App.Core.Desktop
{
    public class DownloadFile : IEquatable<DownloadFile>
    {
        public DownloadFile(string url, string path)
        {
            URL = url;
            Path = path;
            var pathSplit = Path.Split(new string[] { @"\" }, StringSplitOptions.None);

            Name = pathSplit.Last();
            Extension = Name.Split(new string[] { @"." }, StringSplitOptions.None).Last().ToLower();
        }

        public string URL { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long BytesReceived { get; set; }

        public long TotalBytesToReceive { get; set; }

        public float ProgressPercentage { get; set; }

        public bool Equals(DownloadFile other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return URL.Equals(other.URL) && Path.Equals(other.Path);
        }

        public override int GetHashCode()
        {
            int hashProductURL = URL == null ? 0 : URL.GetHashCode();
            int hashProductPath = Path == null ? 0 : Path.GetHashCode();

            return hashProductPath ^ hashProductURL;
        }
    }
}