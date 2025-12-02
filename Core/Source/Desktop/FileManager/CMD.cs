using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1310:FieldNamesMustNotContainUnderscore",
        Justification = "Interop field requires underscore.")]
    public static class CMD
    {
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
            Native.Console.OpenConsole();

            // Position and Size
            IntPtr consoleHandle = Native.Console.GetConsoleWindow();
            Native.Window.MoveWindow(consoleHandle, 0, 0, 580, 480, true);

            if (topMost)
            {
                IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                Native.Window.SetWindowPos(hWnd, Native.Window.Flag.HWND_TOPMOST, 0, 0, 0, 0, Native.Window.Flag.SWP_NOMOVE | Native.Window.Flag.SWP_NOSIZE);
            }

            if (quickEdit == false)
            {
                IntPtr consoleEditHandle = Native.Console.GetStdHandle(Native.Console.Flag.STD_INPUT_HANDLE);
                Native.Console.SetConsoleMode(consoleEditHandle, Native.Console.Flag.ENABLE_EXTENDED_FLAGS);
            }
        }
    }
}