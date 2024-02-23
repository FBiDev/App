using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FileBackup
    {
        [Flags]
        public enum Filters
        {
            AllFiles = 0,
            PDF = 1,
            Microsoft_Excel = 2,
            Microsoft_Word = 4
        }

        Dictionary<Filters, string> filterMap = new Dictionary<Filters, string>
        {
            { Filters.AllFiles, "All Files (*.*)|*.*" },
            { Filters.PDF, "PDF (*.pdf)|*.pdf" },
            { Filters.Microsoft_Excel, "Microsoft Excel (*.xls, *.xlsx)|*.xls;*.xlsx" },
            { Filters.Microsoft_Word, "Microsoft Word (*.doc, *.docx)|*.doc;*.docx" }
        };

        Filters _Filter { get; set; }
        public Filters Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                _Filter = value;
                dlgDestinationCustom.Filter = GetFilter(value);
                dlgDestinationCustom.FilterIndex = 2;
            }
        }

        OpenFileDialog dlgOrigin;

        string _OriginPath;
        public string OriginPath
        {
            get
            {
                return _OriginPath;
            }
            set
            {
                _OriginPath = value;
                UpdateOrigin();
            }
        }
        public string OriginFolder { get; set; }
        public string OriginFile { get; set; }

        FolderPicker dlgDestination { get; set; }
        SaveFileDialog dlgDestinationCustom { get; set; }

        string _DestinationPath;
        public string DestinationPath
        {
            get
            {
                return _DestinationPath;
            }
            set
            {
                _DestinationPath = value;
                UpdateDestination();
            }
        }
        public string DestinationFolder { get; set; }
        public string DestinationFile { get; set; }

        public bool Overwrite { get; set; }
        public bool CustomName { get; set; }
        public bool MakeBackup { get; set; }
        public bool Timer { get; set; }

        bool _TimerIsRunning { get; set; }
        public bool TimerIsRunning
        {
            get
            {
                return _TimerIsRunning;
            }
            set
            {
                _TimerIsRunning = value;
                TimerRunningChanged();
            }
        }
        public int TimerValue { get; set; }
        TaskController TimerTask = new TaskController();

        public event Action Copied = delegate { };
        public event Action InvalidFile = delegate { };
        public event Action TimerRunningChanged = delegate { };

        public FileBackup()
        {
            dlgOrigin = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = ""
            };

            dlgDestination = new FolderPicker
            {
            };

            dlgDestinationCustom = new SaveFileDialog
            {
            };

            Filter = Filters.AllFiles;

            Overwrite = true;
            OriginFile = string.Empty;
        }

        string GetFilter(Filters value)
        {
            string filter = string.Empty;

            foreach (var kvp in filterMap)
            {
                if ((value & kvp.Key) == kvp.Key)
                {
                    if (!string.IsNullOrEmpty(filter))
                        filter += "|";

                    filter += kvp.Value;
                }
            }

            return filter;
        }

        public bool PickOrigin()
        {
            bool result;

            if (result = dlgOrigin.ShowDialog() == DialogResult.OK)
            {
                OriginPath = dlgOrigin.FileName.NormalizePath();
                UpdateDestinationFile();
            }

            return result;
        }

        public bool PickDestination()
        {
            if (CustomName)
            {
                dlgDestinationCustom.InitialDirectory = DestinationFolder;
                if (dlgDestinationCustom.InitialDirectory == null)
                    dlgDestinationCustom.InitialDirectory = dlgOrigin.InitialDirectory;

                if (dlgDestinationCustom.ShowDialog() == DialogResult.OK)
                {
                    DestinationPath = dlgDestinationCustom.FileName.NormalizePath();
                    return true;
                }
                return false;
            }

            dlgDestination.InputPath = DestinationFolder;
            if (dlgDestination.InputPath == null)
                dlgDestination.InputPath = dlgOrigin.InitialDirectory;

            if (dlgDestination.ShowDialog() == true)
            {
                DestinationPath = (Path.Combine(dlgDestination.ResultPath, OriginFile)).NormalizePath();
                return true;
            }
            return false;
        }

        bool IsInvalidInputs()
        {
            if (string.IsNullOrWhiteSpace(OriginPath) || string.IsNullOrWhiteSpace(DestinationPath)
            || OriginPath == DestinationPath
            || Timer && TimerValue <= 0)
            {
                InvalidFile();
                return true;
            }
            return false;
        }

        bool SecureCopy()
        {
            if (File.Exists(OriginPath))
            {
                var attr = File.GetAttributes(OriginPath);
                attr = attr & ~FileAttributes.ReadOnly;
                File.SetAttributes(OriginPath, attr);
            }

            if (Timer || MakeBackup)
            {
                var backupNumber = 1;
                var fileToSave = DestinationPath;

                var folder = Path.GetDirectoryName(fileToSave);
                var name = Path.GetFileNameWithoutExtension(fileToSave);
                var ext = Path.GetExtension(fileToSave);

                var exist = File.Exists(fileToSave);

                while (exist)
                {
                    var backupString = "-back-" + backupNumber.ToString().PadLeft(0, '0') + "";
                    var fullName = name + backupString + ext;
                    var fullPath = Path.Combine(folder, fullName).NormalizePath();

                    exist = File.Exists(fullPath);
                    if (exist)
                        backupNumber++;
                    else
                        File.Move(DestinationPath, fullPath);
                }

                File.Copy(OriginPath, DestinationPath, Overwrite);
            }
            else
            {
                File.Copy(OriginPath, DestinationPath, Overwrite);
            }

            Copied();
            return true;
        }

        public async Task StartBackupTimer()
        {
            do
            {
                await TimerTask.DelayStart(TimerValue);
                if (TimerTask.IsCanceled)
                {
                    TimerIsRunning = false;
                    return;
                }

                SecureCopy();

            } while (TimerIsRunning);
        }

        public bool Copy()
        {
            if (IsInvalidInputs())
                return false;

            var exist = File.Exists(DestinationPath);
            var canCopy = !exist || Overwrite;

            if (Timer)
            {
                TimerIsRunning = !TimerIsRunning;

                if (TimerIsRunning)
                    TimerTask.Reset();
                else
                    TimerTask.Cancel();
                return true;
            }

            if (MakeBackup || canCopy)
            {
                return SecureCopy();
            }

            return false;
        }

        void UpdateOrigin()
        {
            if (string.IsNullOrWhiteSpace(OriginPath)) return;

            dlgOrigin.InitialDirectory = Path.GetDirectoryName(OriginPath);
            dlgOrigin.FileName = Path.GetFileName(OriginPath);

            OriginFolder = dlgOrigin.InitialDirectory;
            OriginFile = dlgOrigin.FileName;
        }

        void UpdateDestinationFile()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath) == false)
                DestinationPath = (Path.Combine(Path.GetDirectoryName(DestinationPath), OriginFile)).NormalizePath();
            else if (CustomName)
                dlgDestinationCustom.FileName = dlgOrigin.FileName;
        }

        void UpdateDestination()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath)) return;

            if (CustomName)
            {
                dlgDestinationCustom.InitialDirectory = Path.GetDirectoryName(DestinationPath);
                dlgDestinationCustom.FileName = Path.GetFileName(DestinationPath);

                DestinationFolder = Path.GetDirectoryName(DestinationPath);
                DestinationFile = dlgDestinationCustom.FileName;
                return;
            }

            DestinationFolder = Path.GetDirectoryName(DestinationPath);
            DestinationFile = Path.GetFileName(DestinationPath);
        }

        public void LoadComboTypes(FlatComboBox cbo)
        {
            var types = new ListBind<ListItem>
            {
                new ListItem{ Text="Overwrite", Value=0},
                new ListItem{ Text="Backup", Value=1},
                new ListItem{ Text="Timer", Value=2}
            };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = types;
            cbo.SelectedIndex = 0;

            cbo.SelectedIndexChanged += cboTypes_SelectedIndexChanged;
        }

        void cboTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as FlatComboBoxNew;
            Overwrite = false;
            MakeBackup = false;
            Timer = false;

            switch (cbo.SelectedIndex)
            {
                case 0: Overwrite = true; break;
                case 1: MakeBackup = true; break;
                case 2: Timer = true; break;
            }
        }

        public void LoadComboTimer(FlatComboBox cbo)
        {
            var timerItems = new List<ListItem> {
                new ListItem(0,    "None"),
                new ListItem(10,   "10 secs"),
                new ListItem(30,   "30 secs"),
                new ListItem(60,   "1 min"),
                new ListItem(300,  "5 mins"),
                new ListItem(600,  "10 mins"),
                new ListItem(900,  "15 mins"),
                new ListItem(1800, "30 mins"),
                new ListItem(3600, "1 hour"),
                new ListItem(7200, "2 hours"),
                new ListItem(10800,"3 hours")
            };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = timerItems;
            cbo.SelectedIndex = 0;

            cbo.SelectedIndexChanged += cboTimer_SelectedIndexChanged;
        }

        void cboTimer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as FlatComboBoxNew;
            TimerValue = ((ListItem)cbo.SelectedItem).Value;
        }
    }
}