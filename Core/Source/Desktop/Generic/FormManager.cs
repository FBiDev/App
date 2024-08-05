using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class FormManager
    {
        public static void ResetControls(params Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] is FlatLabel) (controls[i] as FlatLabel).Text = string.Empty;
                else if (controls[i] is FlatTextBox) (controls[i] as FlatTextBox).Text = string.Empty;
                else if (controls[i] is FlatMaskedTextBox)
                {
                    (controls[i] as FlatMaskedTextBox).Text = string.Empty;
                    (controls[i] as FlatMaskedTextBox).TextBox_LostFocus(null, null);
                }
                else if (controls[i] is FlatComboBox) (controls[i] as FlatComboBox).ResetIndex();
                else if (controls[i] is FlatDataGrid)
                {
                    (controls[i] as FlatDataGrid).Rows.Clear();
                    (controls[i] as FlatDataGrid).RefreshRows();
                }
                else if (controls[i] is FlatTableLayoutPanel) ResetFormTable(controls[i] as FlatTableLayoutPanel);
                else if (controls[i] is object) controls[i] = null;
            }
        }

        public static void EnableControls(bool enable, params Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] is FlatTextBox) (controls[i] as FlatTextBox).Enabled = enable;
                else if (controls[i] is FlatMaskedTextBox) (controls[i] as FlatMaskedTextBox).Enabled = enable;
                else if (controls[i] is FlatComboBox) (controls[i] as FlatComboBox).Enabled = enable;
                else if (controls[i] is FlatDataGrid) (controls[i] as FlatDataGrid).Enabled = enable;
                else if (controls[i] is FlatButton) (controls[i] as FlatButton).Enabled = enable;
            }
        }

        public static void ResetFormPanel(FlatPanel pnlForm)
        {
            var txtBoxes = pnlForm.GetControls<FlatTextBox>();
            var maskBoxes = pnlForm.GetControls<FlatMaskedTextBox>();

            foreach (var item in txtBoxes)
                ResetControls(item);

            foreach (var item in maskBoxes)
                ResetControls(item);
        }

        public static void ResetFormTable(FlatTableLayoutPanel table)
        {
            var txtBoxes = table.GetControls<FlatTextBox>();
            var maskBoxes = table.GetControls<FlatMaskedTextBox>();
            var comboBoxes = table.GetControls<FlatComboBox>();
            var dataGrids = table.GetControls<FlatDataGrid>();

            foreach (var item in txtBoxes) ResetControls(item);
            foreach (var item in maskBoxes) ResetControls(item);
            foreach (var item in comboBoxes) ResetControls(item);
            foreach (var item in dataGrids) ResetControls(item);
        }

        public static void EnableFormControls(bool enable, FlatTableLayoutPanel table)
        {
            var txtBoxes = table.GetControls<FlatTextBox>();
            var maskBoxes = table.GetControls<FlatMaskedTextBox>();
            var comboBoxes = table.GetControls<FlatComboBox>();
            var dataGrids = table.GetControls<FlatDataGrid>();
            var buttons = table.GetControls<FlatButton>();

            foreach (var item in txtBoxes) EnableControls(enable, item);
            foreach (var item in maskBoxes) EnableControls(enable, item);
            foreach (var item in comboBoxes) EnableControls(enable, item);
            foreach (var item in dataGrids) EnableControls(enable, item);
            foreach (var item in buttons) EnableControls(enable, item);
        }

        public static bool HasInvalidObject<T>(T obj) where T : class, new()
        {
            if (obj.IsNull())
                return true;
            if (IsList(obj) || IsBindList(obj))
                if (((IList)obj).Count == 0)
                    return true;
            if (obj.IsEqual(new T()))
                return true;

            return false;
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;

            var type = o.GetType();
            return o is IList && type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsBindList(object o)
        {
            if (o == null) return false;

            var type = o.GetType();
            return o is IList && type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(ListBind<>));
        }

        public static bool HasInvalidFields(params object[] fieldsValue)
        {
            foreach (var value in fieldsValue)
            {
                if (value is int)
                {
                    if ((int)value <= 0)
                        return true;
                    continue;
                }

                if (value is string)
                {
                    if (string.IsNullOrWhiteSpace(value as string))
                        return true;
                    continue;
                }

                if (value is DateTime)
                {
                    if (((DateTime)value).IsNull()
                        || ((DateTime)value) == DateTime.MinValue
                        || ((DateTime)value) < SqlDateTime.MinValue.Value)
                        return true;
                    continue;
                }

                if (value is FlatTextBox)
                {
                    if (string.IsNullOrWhiteSpace((value as FlatTextBox).Text))
                        return true;
                    continue;
                }

                if (value.IsNull())
                {
                    return true;
                }
            }
            return false;
        }
    }
}