using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FilePick
    {
        private OpenFileDialog fileDialog;

        private Filters _filter;

        private Dictionary<Filters, string> filterMap = new Dictionary<Filters, string>
        {
            { Filters.AllFiles, "All Files (*.*)|*.*" },
            { Filters.PDF, "PDF (*.pdf)|*.pdf" },
            { Filters.Microsoft_Excel, "Microsoft Excel (*.xls, *.xlsx)|*.xls;*.xlsx" },
            { Filters.Microsoft_Word, "Microsoft Word (*.doc, *.docx)|*.doc;*.docx" },
            { Filters.Text, "Text (*.txt)|*.txt;" },
        };

        public FilePick()
        {
            fileDialog = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = string.Empty
            };

            fileDialog.Filter = GetFilter(Filters.AllFiles);
            fileDialog.FilterIndex = 2;
        }

        [Flags]
        public enum Filters
        {
            AllFiles = 0,
            PDF = 1,
            Microsoft_Excel = 2,
            Microsoft_Word = 4,
            Text = 8,
            Remessa = 16,
            Retorno = 32
        }

        public Filters Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                fileDialog.Filter = GetFilter(value);
                fileDialog.FilterIndex = 2;
            }
        }

        public string FileName
        {
            get { return fileDialog.SafeFileName; }
        }

        public string Path
        {
            get { return fileDialog.FileName; }
        }

        public bool Open()
        {
            return fileDialog.ShowDialog() == DialogResult.OK;
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
    }
}