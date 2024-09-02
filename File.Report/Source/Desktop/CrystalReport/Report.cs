using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Core.Desktop;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace App.File.Desktop.CrystalReport
{
    public static class Report
    {
        private static ReportForm frm;

        [Flags]
        public enum ViewerExportFormats
        {
            NoFormat,
            PdfFormat,
            ExcelFormat,
            WordFormat = 4,
            RtfFormat = 8,
            RptFormat = 16,
            ExcelRecordFormat = 32,
            EditableRtfFormat = 64,
            XmlFormat = 128,
            RptrFormat = 256,
            XLSXFormat = 512,
            CsvFormat = 1024,
            AllFormats = 268435455
        }

        public static async Task OpenForm()
        {
            frm = new ReportForm();
            frm.Show();
            await Task.Delay(50);

            var exportFormatFlags = (int)(ViewerExportFormats.PdfFormat
                                        | ViewerExportFormats.ExcelFormat
                                        | ViewerExportFormats.WordFormat);

            frm.crvRelatorio.AllowedExportFormats = exportFormatFlags;
        }

        public static void SetReport(ReportClass rpt, string title, Dictionary<IParameterField, object> parameters, int connection)
        {
            switch (connection)
            {
                case 1: SetReportConnection1(rpt);
                    break;
                case 2: SetReportConnection2(rpt);
                    break;
            }

            SetParameters(rpt, parameters);

            frm.crvRelatorio.ReportSource = rpt;
            SetViewerTabText(title);

            rpt.SummaryInfo.ReportTitle = title;
            frm.picLoader.Visible = false;
        }

        private static void SetReportConnection1(ReportDocument rpt)
        {
            // var serverAddress = (Session.Options.IsDatabaseDev) ? Session.Options.DatabaseDev : Session.Options.DatabaseProd;
            // rpt.DataSourceConnections[0].SetConnection(serverAddress, Session.Options.DatabaseName, Session.Options.DatabaseUsername, Session.Options.DatabasePassword);
            // rpt.DataSourceConnections[0].SetLogon(Session.Options.DatabaseUsername, Session.Options.DatabasePassword);
        }

        private static void SetReportConnection2(ReportDocument rpt)
        {
            rpt.DataSourceConnections[0].SetLogon("usr_db_controller", "usr_db_controller");
        }

        private static void SetParameters(ReportClass rpt, Dictionary<IParameterField, object> parameters)
        {
            foreach (var p in parameters)
            {
                rpt.SetParameterValue(p.Key.ParameterFieldName, p.Value);
            }
        }

        private static void SetViewerTabText(string value)
        {
            var oControl = frm.crvRelatorio.Controls[0];
            var oTabControl = (TabControl)oControl.Controls[0];

            oTabControl.TabPages[0].Text = value;
        }
    }
}