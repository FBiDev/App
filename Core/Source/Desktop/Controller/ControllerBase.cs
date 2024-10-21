using System.Windows.Forms;

namespace App.Core.Desktop
{
    public abstract class ControllerBase
    {
        #region Mensagens
        protected Label AlertLabel { get; set; }

        protected bool Alert(string msg)
        {
            if (AlertLabel == null)
            {
                AlertLabel = new Label();
            }

            AlertLabel.Text = msg;
            return !string.IsNullOrWhiteSpace(msg);
        }

        protected void ClearAlert()
        {
            FormManager.ResetControls(AlertLabel);
        }

        protected void SucessBox(ControllerResult res)
        {
            if (res.Sucess)
            {
                ShowBox.Info(res.SucessMessage, res.SucessTitle);
            }
        }

        protected void SucessFailBox(ControllerResult res)
        {
            if (res.Sucess)
            {
                ShowBox.Info(res.SucessMessage, res.SucessTitle);
            }
            else
            {
                Alert(res.ErrorMessage);
            }
        }
        #endregion
    }
}