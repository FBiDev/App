using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class Forms
    {
        public static T Open<T>(Form parent = null, bool once = true) where T : new()
        {
            T gForm = default(T);
            var genericForm = (Form)(object)gForm;

            if (once && Application.OpenForms.OfType<T>().Count() == 0)
            {
                if (genericForm == null || genericForm.IsDisposed)
                {
                    // frm = new T();
                    // FormGeneric = ((Form)(object)frm);
                }

                gForm = new T();
                genericForm = (Form)(object)gForm;
            }
            else
            {
                genericForm = (Form)(object)Application.OpenForms.OfType<T>().First();
            }

            if (genericForm.WindowState == FormWindowState.Minimized)
            {
                genericForm.InvokeIfRequired(() =>
                {
                    genericForm.WindowState = FormWindowState.Normal;
                });
            }

            if (parent != null)
            {
                parent.InvokeIfRequired(() =>
                {
                    genericForm.MdiParent = parent;
                });
            }

            parent.InvokeIfRequired(() =>
            {
                genericForm.Show();
                genericForm.Focus();
            });

            return (T)(object)genericForm;
        }

        public static T Get<T>(bool createIfNull = false) where T : Form, new()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.GetType() == typeof(T))
                {
                    if (f.WindowState == FormWindowState.Minimized)
                    {
                        f.InvokeIfRequired(() =>
                        {
                            f.WindowState = FormWindowState.Normal;
                        });
                    }

                    return (T)f;

                    // return (T)(object)f;
                }
            }

            if (createIfNull)
            {
                return new T();
            }

            return default(T);
        }
    }
}