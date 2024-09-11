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
            if (ReportViewer.ReportSource != null)
            {
                ((ReportDocument)ReportViewer.ReportSource).Dispose();

                if (ReportViewer.Controls.Count > 0)
                {
                    ReportViewer.Controls[0].Controls[0].Dispose();
                }
            }

            ReportViewer.Dispose();
            Dispose();
        }
    }
}