using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

// using System.Windows;
// using System.Windows.Interop;
/* refs:
 * PresentationCore
 * PresentationFramework
 * System.Xaml
 * WindowsBase
 */

namespace App.Core.Desktop
{
    public class FolderPicker
    {
        private readonly List<string> _resultPaths = new List<string>();

        private readonly List<string> _resultNames = new List<string>();

        public IReadOnlyList<string> ResultPaths
        {
            get { return _resultPaths; }
        }

        public IReadOnlyList<string> ResultNames
        {
            get { return _resultNames; }
        }

        public string ResultPath
        {
            get { return ResultPaths.LastOrDefault(); }
        }

        public string ResultName
        {
            get { return ResultNames.LastOrDefault(); }
        }

        public virtual string InputPath { get; set; }

        public virtual bool ForceFileSystem { get; set; }

        public virtual bool Multiselect { get; set; }

        public virtual string Title { get; set; }

        public virtual string OkButtonLabel { get; set; }

        public virtual string FileNameLabel { get; set; }

        // for WPF support
        // public bool? ShowDialog(Window owner = null, bool throwOnError = false)
        // {
        //    if (Application.Current != null)
        //        owner = owner ?? Application.Current.MainWindow;
        //    return ShowDialog(owner != null ? new WindowInteropHelper(owner).Handle : IntPtr.Zero, throwOnError);
        // }

        // for all .NET
        public virtual bool? ShowDialog(IntPtr owner = default(IntPtr), bool throwOnError = false)
        {
            var dialog = (Native.FileDialog.IFileOpenDialog)new FileOpenDialog();

            if (!string.IsNullOrEmpty(InputPath))
            {
                KeyValuePair<int, Native.FileDialog.IShellItem> item = Native.FileDialog.CreateShellItem(InputPath);
                if (CheckHr(item.Key, throwOnError) != 0)
                {
                    return null;
                }

                dialog.SetFolder(item.Value);
            }

            var options = Native.FileDialog.FOS.FOS_PICKFOLDERS;
            options = (Native.FileDialog.FOS)SetOptions((int)options);
            dialog.SetOptions(options);

            if (Title != null)
            {
                dialog.SetTitle(Title);
            }

            if (OkButtonLabel != null)
            {
                dialog.SetOkButtonLabel(OkButtonLabel);
            }

            if (FileNameLabel != null)
            {
                dialog.SetFileName(FileNameLabel);
            }

            if (owner == IntPtr.Zero)
            {
                owner = Process.GetCurrentProcess().MainWindowHandle;
                if (owner == IntPtr.Zero)
                {
                    owner = Native.Window.GetDesktopWindow();
                }
            }

            var hr = dialog.Show(owner);

            if (hr == Native.FileDialog.CANCELED)
            {
                return null;
            }

            if (CheckHr(hr, throwOnError) != 0)
            {
                return null;
            }

            Native.FileDialog.IShellItemArray items;
            if (CheckHr(dialog.GetResults(out items), throwOnError) != 0)
            {
                return null;
            }

            int count;
            items.GetCount(out count);
            for (var i = 0; i < count; i++)
            {
                Native.FileDialog.IShellItem item;
                items.GetItemAt(i, out item);
                string path;
                CheckHr(item.GetDisplayName(Native.FileDialog.SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, out path), throwOnError);
                string name;
                CheckHr(item.GetDisplayName(Native.FileDialog.SIGDN.SIGDN_DESKTOPABSOLUTEEDITING, out name), throwOnError);
                if (path != null || name != null)
                {
                    _resultPaths.Add(path);
                    _resultNames.Add(name);
                }
            }

            return true;
        }

        protected virtual int SetOptions(int options)
        {
            if (ForceFileSystem)
            {
                options |= (int)Native.FileDialog.FOS.FOS_FORCEFILESYSTEM;
            }

            if (Multiselect)
            {
                options |= (int)Native.FileDialog.FOS.FOS_ALLOWMULTISELECT;
            }

            return options;
        }

        private static int CheckHr(int hr, bool throwOnError)
        {
            if (hr != 0 && throwOnError)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return hr;
        }

        [ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")] // CLSID_FileOpenDialog
        private class FileOpenDialog
        {
        }
    }
}