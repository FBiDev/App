using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EnvDTE;

namespace App.Core.Desktop
{
    public static class DebugLog
    {
        public enum Window
        {
            Output,
            Console
        }

        public static Window WindowType { get; set; }

        public static string Buffer { get; private set; }

        public static void OpenConsole(bool quickEdit = true, bool topMost = false)
        {
            CMD.AttachWindow(quickEdit, topMost);
        }

        public static void WriteLine(string message)
        {
            if (WindowType == Window.Output)
            {
                Debug.WriteLine(message);
            }
            else
            {
                if (Native.Console.IsOpen() == false)
                {
                    OpenConsole(false, false);
                }

                Native.Console.WriteLine(message);
            }

            Buffer += message + Environment.NewLine;
        }

        public static void OutputToImmediate(bool value = true)
        {
            DTE dte = GetVisualStudioOptions();

            try
            {
                dte.Properties["Debugging", "General"].Item("RedirectOutputToImmediate").Value = value;
            }
            catch (Exception)
            {
            }
        }

        private static DTE GetVisualStudioOptions()
        {
            DTE dte = null;

            try
            {
                dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.14.0");
            }
            catch (Exception)
            {
            }

            try
            {
                dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.11.0");
            }
            catch (Exception)
            {
            }

            if (dte != null)
            {
                WriteLine("Visual Studio Version: " + dte.Version);
            }

            return dte;
        }
    }
}