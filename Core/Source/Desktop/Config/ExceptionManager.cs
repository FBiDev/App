using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    internal static class ExceptionManager
    {
        public static string Resolve(Exception ex, string customMessage = "")
        {
            var exProc = Core.ExceptionManagerAny.Process(ex);

            if (exProc.HasLink)
            {
                // + errorLineBreak + Error.Message
                if (MessageBox.Show(exProc.Message + customMessage, "Erro", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    var sInfo = new ProcessStartInfo(exProc.Link);
                    Process.Start(sInfo);
                }
            }
            else if (exProc.OleDb)
            {
                // + errorLineBreak + ErrorDb.Message
                MessageBox.Show(exProc.Message + customMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!exProc.ExternalDll)
            {
                MessageBox.Show(exProc.Message + customMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClipboardSafe.SetText(exProc.Message + customMessage);

                // MessageBox.Show(CustomMessage + errorLineBreak + Error.Message + errorLineBreak + Error.StackTrace, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // throw new Exception(CustomMessage + errorLineBreak + Error.Message);
            }

            return exProc.Message;
        }
    }
}