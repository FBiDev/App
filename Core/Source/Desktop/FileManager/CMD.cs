using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace App.Core.Desktop
{
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
                    //WorkingDirectory = ""
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
            var procArray = Process.GetProcessesByName(fileName.Replace(".exe", ""));
            if (procArray.Length == 0)
            {
                var proc = new Process();

                proc.StartInfo.WorkingDirectory = Path.Combine(folderBase, folderPath);
                proc.StartInfo.FileName = Path.Combine(folderBase, folderPath, fileName);

                //var folderExists = Directory.Exists(proc.StartInfo.WorkingDirectory);
                //var fileExists = File.Exists(proc.StartInfo.FileName);

                proc.StartInfo.Arguments = "";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.Start();
            }
            else
            {
                MessageBox.Show("Esse programa já esta sendo executado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}