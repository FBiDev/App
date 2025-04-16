using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class AppManager
    {
        public static string Name
        {
            get { return Assembly.GetEntryAssembly().GetName().Name; }
        }

        public static string Version
        {
            get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
        }

        public static string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string ExecutablePath
        {
            get { return Assembly.GetEntryAssembly().Location; }
        }

        public static FileInfo ExecutableInfo
        {
            get { return new FileInfo(Assembly.GetEntryAssembly().Location); }
        }

        public static bool Is64BitProcess
        {
            get { return Environment.Is64BitProcess; }
        }

        public static bool Is64BitOperatingSystem
        {
            get { return Environment.Is64BitOperatingSystem; }
        }

        public static string Processor_architecture
        {
            get { return Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"); }
        }

        public static ImageFileMachine TargetPlataform
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                PortableExecutableKinds p;
                ImageFileMachine machineInfo;
                assembly.ManifestModule.GetPEKind(out p, out machineInfo);

                return machineInfo;
            }
        }

        public static OSVersion GetOSVersion()
        {
            // var versionString = (string)Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion").GetValue("productName");
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;
            OSVersion operatingSystem = OSVersion.Unknown;

            if (os.Platform == PlatformID.Win32Windows)
            {
                switch (vs.Minor)
                {
                    case 0: operatingSystem = OSVersion.Windows_95;
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                        {
                            operatingSystem = OSVersion.Windows_98SE;
                        }
                        else
                        {
                            operatingSystem = OSVersion.Windows_98;
                        }

                        break;
                    case 90: operatingSystem = OSVersion.Windows_ME;
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                var osVersionInfo = NativeMethods.GetOSVersionInfo();

                int productType = osVersionInfo.wProductType;
                const int VER_NT_WORKSTATION = 1;
                const int VER_NT_SERVER = 3;

                ////int editionID = osVersionInfo.wSuiteMask;

                switch (productType)
                {
                    case VER_NT_WORKSTATION:
                        switch (vs.Major)
                        {
                            case 3: operatingSystem = OSVersion.Windows_NT3;
                                break;
                            case 4: operatingSystem = OSVersion.Windows_NT4;
                                break;
                            case 5:
                                if (vs.Minor == 0)
                                {
                                    operatingSystem = OSVersion.Windows_2000;
                                }
                                else if (vs.Minor == 1)
                                {
                                    operatingSystem = OSVersion.Windows_XP;
                                }
                                else
                                {
                                    operatingSystem = OSVersion.Windows_XP_x64;
                                }

                                break;
                            case 6:
                                switch (vs.Minor)
                                {
                                    case 0: operatingSystem = OSVersion.Windows_Vista;
                                        break;
                                    case 1: operatingSystem = OSVersion.Windows_7;
                                        break;
                                    case 2: operatingSystem = OSVersion.Windows_8;
                                        break;
                                    case 3: operatingSystem = OSVersion.Windows_8_1;
                                        break;
                                }

                                break;
                            case 10:
                                if (vs.Build < 22000)
                                {
                                    operatingSystem = OSVersion.Windows_10;
                                }
                                else
                                {
                                    operatingSystem = OSVersion.Windows_11;
                                }

                                break;
                        }

                        break;
                    case VER_NT_SERVER:
                        switch (vs.Major)
                        {
                            case 5:
                                if (vs.Minor == 2)
                                {
                                    operatingSystem = OSVersion.Windows_Server_2003;
                                }

                                break;
                            case 6:
                                switch (vs.Minor)
                                {
                                    case 0: operatingSystem = OSVersion.Windows_Server_2008;
                                        break;
                                    case 1: operatingSystem = OSVersion.Windows_Server_2008_R2;
                                        break;
                                    case 2: operatingSystem = OSVersion.Windows_Server_2012;
                                        break;
                                    case 3: operatingSystem = OSVersion.Windows_Server_2012_R2;
                                        break;
                                }

                                break;
                            case 10:
                                switch (vs.Minor)
                                {
                                    case 0: operatingSystem = OSVersion.Windows_Server_2016;
                                        break;
                                }

                                break;
                        }

                        break;
                }
            }

            return operatingSystem;
        }

        public static string GetCurrentWindowsUsername()
        {
            IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
            var windowsIdentity = new WindowsIdentity(accountToken);
            return windowsIdentity.Name.Split('\\').Last();
        }

        public static string GetMachineName()
        {
            return Environment.MachineName;
        }

        public static string GetGuid()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var id = attribute.Value;
            return id;
        }

        public static void Exit()
        {
            if (Application.MessageLoop)
            {
                // WinForms app
                Application.Exit();
            }
            else
            {
                // Console app
                Environment.Exit(0);
            }
        }

        #region Lock to 1 Execution only
        public static Mutex SingleProcess(bool locked, string systemName)
        {
            var mutex = new Mutex(false, "Global\\" + systemName + "-" + GetGuid());
            if (!mutex.WaitOne(0, true) && locked)
            {
                MessageBox.Show("O programa " + systemName + " já esta sendo executado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Exit();
            }

            return mutex;
        }
        #endregion

        #region Minimize RAM usage
        public static void CollectGarbage()
        {
            GC.Collect();
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }

        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(IntPtr hwProc);
        #endregion
    }
}