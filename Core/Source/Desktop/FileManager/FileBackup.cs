using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FileBackup
    {
        private Dictionary<Filters, string> filterMap = new Dictionary<Filters, string>
        {
            { Filters.AllFiles, "All Files (*.*)|*.*" },
            { Filters.PDF, "PDF (*.pdf)|*.pdf" },
            { Filters.Microsoft_Excel, "Microsoft Excel (*.xls, *.xlsx)|*.xls;*.xlsx" },
            { Filters.Microsoft_Word, "Microsoft Word (*.doc, *.docx)|*.doc;*.docx" }
        };

        private OpenFileDialog originDialog;

        private string _originPath;
        private string _destinationPath;
        private TaskController timerTask = new TaskController();

        public FileBackup()
        {
            originDialog = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = string.Empty
            };

            DestinationDialog = new FolderPicker
            {
            };

            DestinationDialogCustom = new SaveFileDialog
            {
            };

            Filter = Filters.AllFiles;

            Overwrite = true;
            OriginFile = string.Empty;
        }

        public event Action Copied = delegate { };

        public event Action InvalidFile = delegate { };

        public event Action TimerRunningChanged = delegate { };

        [Flags]
        public enum Filters
        {
            AllFiles = 0,
            PDF = 1,
            Microsoft_Excel = 2,
            Microsoft_Word = 4
        }

        public Filters Filter
        {
            get
            {
                return _Filter;
            }

            set
            {
                _Filter = value;
                DestinationDialogCustom.Filter = GetFilter(value);
                DestinationDialogCustom.FilterIndex = 2;
            }
        }

        public string OriginPath
        {
            get
            {
                return _originPath;
            }

            set
            {
                _originPath = value;
                UpdateOrigin();
            }
        }

        public string OriginFolder { get; set; }

        public string OriginFile { get; set; }

        public string DestinationPath
        {
            get
            {
                return _destinationPath;
            }

            set
            {
                _destinationPath = value;
                UpdateDestination();
            }
        }

        public string DestinationFolder { get; set; }

        public string DestinationFile { get; set; }

        public bool Overwrite { get; set; }

        public bool CustomName { get; set; }

        public bool MakeBackup { get; set; }

        public bool Timer { get; set; }

        public int TimerValue { get; set; }

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

        private Filters _Filter { get; set; }

        private FolderPicker DestinationDialog { get; set; }

        private SaveFileDialog DestinationDialogCustom { get; set; }

        private bool _TimerIsRunning { get; set; }

        public bool PickOrigin()
        {
            bool result;

            if (result = originDialog.ShowDialog() == DialogResult.OK)
            {
                OriginPath = originDialog.FileName.NormalizePath();
                UpdateDestinationFile();
            }

            return result;
        }

        public bool PickDestination()
        {
            if (CustomName)
            {
                DestinationDialogCustom.InitialDirectory = DestinationFolder;

                if (DestinationDialogCustom.InitialDirectory == null)
                {
                    DestinationDialogCustom.InitialDirectory = originDialog.InitialDirectory;
                }

                if (DestinationDialogCustom.ShowDialog() == DialogResult.OK)
                {
                    DestinationPath = DestinationDialogCustom.FileName.NormalizePath();
                    return true;
                }

                return false;
            }

            DestinationDialog.InputPath = DestinationFolder;

            if (DestinationDialog.InputPath == null)
            {
                DestinationDialog.InputPath = originDialog.InitialDirectory;
            }

            if (DestinationDialog.ShowDialog() == true)
            {
                DestinationPath = Path.Combine(DestinationDialog.ResultPath, OriginFile).NormalizePath();
                return true;
            }

            return false;
        }

        public async Task StartBackupTimer()
        {
            do
            {
                await timerTask.DelayStart(TimerValue);
                if (timerTask.IsCanceled)
                {
                    TimerIsRunning = false;
                    return;
                }

                SecureCopy();
            }
            while (TimerIsRunning);
        }

        public bool Copy()
        {
            if (IsInvalidInputs())
            {
                return false;
            }

            var exist = File.Exists(DestinationPath);
            var canCopy = !exist || Overwrite;

            if (Timer)
            {
                TimerIsRunning = !TimerIsRunning;

                if (TimerIsRunning)
                {
                    timerTask.Reset();
                }
                else
                {
                    timerTask.Cancel();
                }

                return true;
            }

            if (MakeBackup || canCopy)
            {
                return SecureCopy();
            }

            return false;
        }

        public void LoadTypes(FlatComboBox cbo)
        {
            var types = new ListBind<ListItem>
            {
                new ListItem { Text = "Overwrite", Value = 0 },
                new ListItem { Text = "Backup", Value = 1 },
                new ListItem { Text = "Timer", Value = 2 }
            };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = types;
            cbo.SelectedIndex = 0;

            cbo.SelectedIndexChanged += TypesComboBox_SelectedIndexChanged;
        }

        public void LoadTimer(FlatComboBox cbo)
        {
            var timerItems = new List<ListItem> 
            {
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
                new ListItem(10800, "3 hours")
            };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = timerItems;
            cbo.SelectedIndex = 0;

            cbo.SelectedIndexChanged += TimerComboBox_SelectedIndexChanged;
        }

        private string GetFilter(Filters value)
        {
            string filter = string.Empty;

            foreach (var kvp in filterMap)
            {
                if ((value & kvp.Key) == kvp.Key)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += "|";
                    }

                    filter += kvp.Value;
                }
            }

            return filter;
        }

        private bool IsInvalidInputs()
        {
            if (string.IsNullOrWhiteSpace(OriginPath) || string.IsNullOrWhiteSpace(DestinationPath) ||
                OriginPath == DestinationPath || (Timer && TimerValue <= 0))
            {
                InvalidFile();
                return true;
            }

            return false;
        }

        private bool SecureCopy()
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
                    var backupString = "-back-" + backupNumber.ToString().PadLeft(0, '0');
                    var fullName = name + backupString + ext;
                    var fullPath = Path.Combine(folder, fullName).NormalizePath();

                    exist = File.Exists(fullPath);
                    if (exist)
                    {
                        backupNumber++;
                    }
                    else
                    {
                        File.Move(DestinationPath, fullPath);
                    }
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

        private void UpdateOrigin()
        {
            if (string.IsNullOrWhiteSpace(OriginPath))
            {
                return;
            }

            originDialog.InitialDirectory = Path.GetDirectoryName(OriginPath);
            originDialog.FileName = Path.GetFileName(OriginPath);

            OriginFolder = originDialog.InitialDirectory;
            OriginFile = originDialog.FileName;
        }

        private void UpdateDestinationFile()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath) == false)
            {
                DestinationPath = Path.Combine(Path.GetDirectoryName(DestinationPath), OriginFile).NormalizePath();
            }
            else if (CustomName)
            {
                DestinationDialogCustom.FileName = originDialog.FileName;
            }
        }

        private void UpdateDestination()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath))
            {
                return;
            }

            if (CustomName)
            {
                DestinationDialogCustom.InitialDirectory = Path.GetDirectoryName(DestinationPath);
                DestinationDialogCustom.FileName = Path.GetFileName(DestinationPath);

                DestinationFolder = Path.GetDirectoryName(DestinationPath);
                DestinationFile = DestinationDialogCustom.FileName;
                return;
            }

            DestinationFolder = Path.GetDirectoryName(DestinationPath);
            DestinationFile = Path.GetFileName(DestinationPath);
        }

        private void TypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Overwrite = false;
            MakeBackup = false;
            Timer = false;

            var cbo = sender as FlatComboBoxNew;

            switch (cbo.SelectedIndex)
            {
                case 0: Overwrite = true;
                    break;
                case 1: MakeBackup = true;
                    break;
                case 2: Timer = true;
                    break;
            }
        }

        private void TimerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as FlatComboBoxNew;
            TimerValue = ((ListItem)cbo.SelectedItem).Value;
        }
    }
}