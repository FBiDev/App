using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class CMD
    {
        private const int STD_INPUT_HANDLE = -10;
        private const int ENABLE_QUICK_EDIT_MODE = 0x0040;
        private const int ENABLE_EXTENDED_FLAGS = 0x0080;

        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        public static string Execute(string exeCmd)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),
                    StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = "cmd.exe",
                    Arguments = "/C " + exeCmd
                    ////WorkingDirectory = ""
                }
            };

            process.Start();

            string output = exeCmd + Environment.NewLine +
                process.StandardError.ReadToEnd() + Environment.NewLine + process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Dispose();
            return output;
        }

        public static void ExecuteProcess(string fileName, string folderPath, string folderBase = "")
        {
            var procArray = Process.GetProcessesByName(fileName.Replace(".exe", string.Empty));
            if (procArray.Length == 0)
            {
                var proc = new Process();

                proc.StartInfo.WorkingDirectory = Path.Combine(folderBase, folderPath);
                proc.StartInfo.FileName = Path.Combine(folderBase, folderPath, fileName);

                ////var folderExists = Directory.Exists(proc.StartInfo.WorkingDirectory);
                ////var fileExists = File.Exists(proc.StartInfo.FileName);

                proc.StartInfo.Arguments = string.Empty;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.Start();
            }
            else
            {
                MessageBox.Show("Esse programa já esta sendo executado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void AttachWindow(bool quickEdit = true, bool topMost = false)
        {
            AllocConsole();

            // Position and Size
            IntPtr consoleHandle = GetConsoleWindow();
            MoveWindow(consoleHandle, 0, 0, 580, 480, true);

            if (topMost)
            {
                IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                SetWindowPos(hWnd, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }

            if (quickEdit == false)
            {
                IntPtr consoleEditHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(consoleEditHandle, ENABLE_EXTENDED_FLAGS);
            }
        }

        public static void CloseWindow()
        {
            FreeConsole();
        }

        public static bool IsOpen()
        {
            IntPtr consoleHandle = GetConsoleWindow();
            return consoleHandle != IntPtr.Zero;
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);
    }
}