using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace App.Data.Report
{
    public partial class ReportViewerForm : Form
    {
        public ReportViewerForm()
        {
            InitializeComponent();
            //Load += (s, e) => ReportViewerForm_Load(s, e).TryAwait();
            FormClosing += ReportViewerForm_FormClosing;

            HandleCreated += (sender, e) =>
            {
                if (DesignMode) return;
            };
        }

        void ReportViewerForm_Load(object sender, EventArgs e)
        {
        }

        void ReportViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (crvRelatorio.ReportSource != null)
            {
                ((ReportClass)crvRelatorio.ReportSource).Dispose();

                if (crvRelatorio.Controls.Count > 0)
                    crvRelatorio.Controls[0].Controls[0].Dispose();
            }

            crvRelatorio.Dispose();
            Dispose();
        }
    }
}