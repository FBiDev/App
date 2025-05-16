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
            ButtonMessage = "Copy";
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

        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }

        public string ButtonMessage { get; set; }

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

        public bool CustomDestination { get; set; }

        public bool IsBackupCopy { get; set; }

        public bool IsTimer { get; set; }

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
                SuccessMessage = _TimerIsRunning ? "Backup Started!" : "Backup Stopped!";
                ButtonMessage = _TimerIsRunning ? "Backup Stop" : "Backup Start";

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
            if (CustomDestination)
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
            var exist = File.Exists(DestinationPath);
            var canCopy = !exist || Overwrite;

            if (IsTimer)
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

            if (IsBackupCopy || canCopy)
            {
                return SecureCopy();
            }

            return false;
        }

        public void LoadTypes(FlatComboBox cbo)
        {
            var types = new ListBind<DropItem> 
            { 
                new DropItem(0, "Overwrite"),
                new DropItem(1, "Backup"),
                new DropItem(2, "Timer")
            };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = types;
            cbo.SelectedIndex = 0;

            cbo.SelectedIndexChanged += TypesComboBox_SelectedIndexChanged;
        }

        public void LoadTimer(FlatComboBox cbo)
        {
            var timerItems = new List<DropItem> 
            {
                new DropItem(0,    "None"),
                new DropItem(10,   "10 secs"),
                new DropItem(30,   "30 secs"),
                new DropItem(60,   "1 min"),
                new DropItem(300,  "5 mins"),
                new DropItem(600,  "10 mins"),
                new DropItem(900,  "15 mins"),
                new DropItem(1800, "30 mins"),
                new DropItem(3600, "1 hour"),
                new DropItem(7200, "2 hours"),
                new DropItem(10800, "3 hours")
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
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(OriginPath))
            {
                ErrorMessage = "Origin File is empty!";
            }
            else if (File.Exists(OriginPath) == false)
            {
                ErrorMessage = "Origin File not Found!";
            }
            else if (string.IsNullOrWhiteSpace(DestinationPath))
            {
                ErrorMessage = "Destination File is empty!";
            }
            else if (Archive.IsLocked(DestinationPath))
            {
                ErrorMessage = "Destination File is open in another program!";
            }
            else if (OriginPath == DestinationPath)
            {
                ErrorMessage = "Origin and Destination Files are the same!";
            }
            else if (IsTimer && TimerValue <= 0)
            {
                TimerIsRunning = false;
                ErrorMessage = "Timer must be greater than 0!";
            }

            if (ErrorMessage != string.Empty)
            {
                InvalidFile();
                return true;
            }

            return false;
        }

        private bool SecureCopy()
        {
            if (IsInvalidInputs())
            {
                return false;
            }

            if (File.Exists(OriginPath))
            {
                var attr = File.GetAttributes(OriginPath);
                attr = attr & ~FileAttributes.ReadOnly;
                File.SetAttributes(OriginPath, attr);
            }

            if (IsTimer || IsBackupCopy)
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

            SuccessMessage = "File Copied!";
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
            if (CustomDestination == false)
            {
                DestinationPath = Path.Combine(Path.GetDirectoryName(DestinationPath), OriginFile).NormalizePath();
            }
        }

        private void UpdateDestination()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath))
            {
                return;
            }

            DestinationDialogCustom.InitialDirectory = Path.GetDirectoryName(DestinationPath);
            DestinationDialogCustom.FileName = Path.GetFileName(DestinationPath);

            DestinationFolder = Path.GetDirectoryName(DestinationPath);
            DestinationFile = Path.GetFileName(DestinationPath);
        }

        private void TypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Overwrite = false;
            IsBackupCopy = false;
            IsTimer = false;
            ButtonMessage = "Copy";

            var cbo = sender as FlatComboBoxNew;

            switch (cbo.SelectedIndex)
            {
                case 0: Overwrite = true;
                    break;
                case 1: IsBackupCopy = true;
                    break;
                case 2:
                    IsTimer = true;
                    ButtonMessage = "Backup Start";
                    break;
            }
        }

        private void TimerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as FlatComboBoxNew;
            TimerValue = ((DropItem)cbo.SelectedItem).Value;
        }
    }
}