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

        protected void SuccessBox(ControllerResult res)
        {
            if (res.Success)
            {
                ShowBox.Info(res.SuccessMessage, res.SuccessTitle);
            }
        }

        protected void SuccessFailBox(ControllerResult res)
        {
            if (res.Success)
            {
                ShowBox.Info(res.SuccessMessage, res.SuccessTitle);
            }
            else
            {
                Alert(res.ErrorMessage);
            }
        }
        #endregion
    }
}