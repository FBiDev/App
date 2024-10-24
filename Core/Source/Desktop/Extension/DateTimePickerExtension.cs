using System;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class DateTimePickerExtension
    {
        public static void Open(this DateTimePicker obj)
        {
            obj.Show();
            obj.Select();
            SendKeys.Send("%{DOWN}");
        }

        public static bool ValidDateTime(this DateTimePicker obj, DateTime? date)
        {
            return date is DateTime
                        && date.Value > obj.MinDate
                        && date.Value < obj.MaxDate;
        }

        public static bool TryParse(this DateTimePicker obj, DateTime? date)
        {
            if (ValidDateTime(obj, date))
            {
                obj.Value = date.Value;
                return true;
            }

            MessageBox.Show("Insira uma data válida, entre 1753 e 9998", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }
}