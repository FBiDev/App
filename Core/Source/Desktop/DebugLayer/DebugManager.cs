using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class DebugManager
    {
        static readonly List<KeyValuePair<string, int>> Errors = new List<KeyValuePair<string, int>>();
        static string Messages = string.Empty;

        public static ListBind<SqlLog> LogSQLSistema = new ListBind<SqlLog>();
        public static ListBind<SqlLog> LogSQLBase = new ListBind<SqlLog>();

        public static bool Enable;
        static DebugForm output;

        static void Create()
        {
            if (output.IsNull() || output.IsDisposed)
                output = new DebugForm();
        }

        public static void Open()
        {
            Create();

            if (Enable)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f.GetType() == typeof(DebugForm))
                    {
                        ((DebugForm)f).WindowState = FormWindowState.Normal;
                        return;
                    }
                }

                output.Show();
                output.Focus();
            }
        }

        public static List<KeyValuePair<string, int>> GetErrors()
        {
            return Errors;
        }

        public static void AddError(string error)
        {
            error += Environment.NewLine;
            var item = new KeyValuePair<string, int>(error, 1);

            //Update Item
            if (Errors.Any(i => i.Key == error))
            {
                var index = Errors.FindIndex(i => i.Key == error);
                item = new KeyValuePair<string, int>(error, Errors[index].Value + 1);
                Errors[index] = item;
            }
            //Add new Item
            else
            {
                Errors.Insert(0, item);
            }

            Create();

            output.UpdateErrors();
            output.TabSelectIndex(1);

            Open();
        }

        public static void AddMessage(string text)
        {
            Messages = Messages.Insert(0, text + Environment.NewLine);

            //if Form already open, update
            foreach (Form f in Application.OpenForms)
            {
                if (f.GetType() == typeof(DebugForm))
                {
                    ((DebugForm)f).UpdateMessages();
                }
            }
        }

        public static string GetMessages()
        {
            return Messages;
        }
    }
}