using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class DebugManager
    {
        private static readonly List<KeyValuePair<string, int>> Errors = new List<KeyValuePair<string, int>>();
        private static string messages = string.Empty;
        private static DebugForm output;

        private static ListBind<SqlLog> _logSQLBase = new ListBind<SqlLog>();
        private static ListBind<SqlLog> _logSQLSistema = new ListBind<SqlLog>();

        public static bool Enable { get; set; }

        public static ListBind<SqlLog> LogSQLBase
        {
            get { return _logSQLBase; }
            set { _logSQLBase = value; }
        }

        public static ListBind<SqlLog> LogSQLSistema
        {
            get { return _logSQLSistema; }
            set { _logSQLSistema = value; }
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

            if (Errors.Any(i => i.Key == error))
            {
                // Update Item
                var index = Errors.FindIndex(i => i.Key == error);
                item = new KeyValuePair<string, int>(error, Errors[index].Value + 1);
                Errors[index] = item;
            }
            else
            {
                // Add new Item
                Errors.Insert(0, item);
            }

            Create();

            output.UpdateErrors();
            output.TabSelectIndex(1);

            Open();
        }

        public static void AddMessage(string text)
        {
            messages = messages.Insert(0, text + Environment.NewLine);

            // if Form already open, update
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
            return messages;
        }

        private static void Create()
        {
            if (output.IsNull() || output.IsDisposed)
            {
                output = new DebugForm();

                LogSQLBase = LogSQLBase ?? new ListBind<SqlLog>();
                LogSQLSistema = LogSQLSistema ?? new ListBind<SqlLog>();
            }
        }
    }
}