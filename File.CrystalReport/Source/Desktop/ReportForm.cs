using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace App.File.CrystalReport.Desktop
{
    internal partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            
            // Load += (s, e) => ReportViewerForm_Load(s, e).TryAwait();
            FormClosing += ReportViewerForm_FormClosing;

            HandleCreated += (sender, e) =>
            {
                if (DesignMode)
                {
                    return;
                }
            };
        }

        private void ReportViewerForm_Load(object sender, EventArgs e)
        {
        }

        private void ReportViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (crvRelatorio.ReportSource != null)
            {
                ((ReportDocument)crvRelatorio.ReportSource).Dispose();

                if (crvRelatorio.Controls.Count > 0)
                {
                    crvRelatorio.Controls[0].Controls[0].Dispose();
                }
            }

            crvRelatorio.Dispose();
            Dispose();
        }
    }
}