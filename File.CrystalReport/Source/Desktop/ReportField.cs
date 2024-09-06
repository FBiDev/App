using System;
using CrystalDecisions.CrystalReports.Engine;

namespace App.File.CrystalReport.Desktop
{
    public class ReportField
    {
        public ReportField(FormulaFieldDefinition formulaField)
        {
            FormulaField = formulaField;
        }

        public string Name
        {
            get { return FormulaField.Name; }
        }

        public string Text
        {
            get
            {
                return FormulaField.Text;
            }

            set
            {
                value = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim('\'').Trim();
                FormulaField.Text = "'" + value + "'";
            }
        }

        private FormulaFieldDefinition FormulaField { get; set; }
    }
}