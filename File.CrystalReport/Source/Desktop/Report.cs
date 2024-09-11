using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Core.Desktop;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace App.File.CrystalReport.Desktop
{
    public class Report<T> where T : ReportDocument, new()
    {
        private ReportForm form;
        private T document;
        private List<ReportField> formulaFields;

        public Report()
        {
        }

        public string Name { get; private set; }

        public Dictionary<object, object> Parameters { get; set; }

        public T Document
        {
            get
            {
                return document;
            }

            set
            {
                document = value;

                formulaFields = new List<ReportField>();

                foreach (FormulaFieldDefinition item in Document.DataDefinition.FormulaFields)
                {
                    formulaFields.Add(new ReportField(item));
                }
            }
        }

        public static async Task<Report<T>> Create(string name)
        {
            var rpt = new Report<T> { Name = name };

            var openedForm = rpt.form.GetOpenedForm(name);

            if (openedForm == null)
            {
                rpt.form = new ReportForm { Text = name };
            }
            else
            {
                rpt.form = (ReportForm)openedForm;
            }

            rpt.form.Show();

            await Task.Delay(50);

            rpt.Document = new T();
            rpt.Document.SummaryInfo.ReportTitle = name;

            rpt.Parameters = new Dictionary<object, object>();

            return rpt;
        }

        public void Open(Action<ReportField> fieldsAction, ReportConnection connection)
        {
            ProcessFields(fieldsAction);

            SetConnection(connection);

            SetParameters(Parameters);

            form.ReportViewer.AllowedExportFormats = (int)(ViewerExportFormats.PdfFormat | ViewerExportFormats.ExcelFormat | ViewerExportFormats.WordFormat);
            form.ReportViewer.ReportSource = Document;

            SetTabText(Name);

            form.LoaderPicture.Visible = false;
        }

        private void ProcessFields(Action<ReportField> fieldsAction)
        {
            foreach (ReportField formulaField in formulaFields)
            {
                fieldsAction(formulaField);
            }
        }

        private void SetConnection(ReportConnection connection)
        {
            switch (connection)
            {
                case ReportConnection.None:
                    // var serverAddress = (Session.Options.IsDatabaseDev) ? Session.Options.DatabaseDev : Session.Options.DatabaseProd;
                    // Document.DataSourceConnections[0].SetConnection(serverAddress, Session.Options.DatabaseName, Session.Options.DatabaseUsername, Session.Options.DatabasePassword);
                    // Document.DataSourceConnections[0].SetLogon(Session.Options.DatabaseUsername, Session.Options.DatabasePassword);
                    break;
                case ReportConnection.Controller:
                    Document.DataSourceConnections[0].SetLogon("usr_db_controller", "usr_db_controller");
                    break;
            }
        }

        private void SetParameters(Dictionary<object, object> parameters)
        {
            foreach (var p in parameters)
            {
                Document.SetParameterValue(((IParameterField)p.Key).ParameterFieldName, p.Value);
            }
        }

        private void SetTabText(string value)
        {
            var oControl = form.ReportViewer.Controls[0];
            var oTabControl = (TabControl)oControl.Controls[0];

            oTabControl.TabPages[0].Text = value;
        }
    }
}