using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class Forms
    {
        public static T Open<T>(Form parent = null, bool once = true) where T : new()
        {
            T frmG = default(T);
            var FormGeneric = ((Form)(object)frmG);

            if (once && Application.OpenForms.OfType<T>().Count() == 0)
            {
                if (FormGeneric == null || FormGeneric.IsDisposed)
                {
                    //frm = new T();
                    //FormGeneric = ((Form)(object)frm);
                }

                frmG = new T();
                FormGeneric = ((Form)(object)frmG);
            }
            else
            {
                FormGeneric = ((Form)(object)(Application.OpenForms.OfType<T>().First()));
            }

            if (FormGeneric.WindowState == FormWindowState.Minimized)
            {
                FormGeneric.WindowState = FormWindowState.Normal;
            }

            if (parent != null)
            {
                FormGeneric.MdiParent = parent;
            }

            FormGeneric.Show();
            FormGeneric.Focus();
            return (T)(object)FormGeneric;
        }

        public static T Get<T>() where T : Form
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.GetType() == typeof(T))
                {
                    return (T)f;
                    //return (T)(object)f;
                }
            }

            return default(T);
        }
    }
}