using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class FormExtension
    {
        public static IEnumerable<T> GetControls<T>(this Control c)
        {
            var controls = c.Controls.OfType<T>().
                   Concat(c.Controls.OfType<Control>().SelectMany(x => x.GetControls<T>()));
            return controls;
        }

        public static Point ControlLocation(this Form f, Control c)
        {
            var locationOnForm = f.PointToClient(c.Parent.PointToScreen(c.Location));
            return locationOnForm;
        }

        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        public static Form GetOpenedForm(this Form source, string title)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == title)
                {
                    return f;
                }
            }

            return null;
        }
    }
}