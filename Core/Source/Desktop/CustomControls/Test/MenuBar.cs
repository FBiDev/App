using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class MenuBar : UserControl
    {
        MenuMode _Modo;
        public MenuMode Modo
        {
            get { return _Modo; }
            set
            {
                _Modo = value;
                lblModo.Text = _Modo.ToString();
            }
        }

        public MenuBar()
        {
            InitializeComponent();
            TabStop = false;
            Modo = MenuMode.Consulta;
        }

        void MenuBarOnSizeChanged(object sender, EventArgs e)
        {
            int ctrs = pnlMenuBar.Controls.Count;
            int each = pnlMenuBar.Width / ctrs;
            int cItem = 0;

            foreach (Control c in pnlMenuBar.Controls)
            {
                var newLocation = new Point((each / 2) - (c.Width / 2) + cItem * each, c.Location.Y);
                c.Location = newLocation;
                cItem++;
            }
        }

        void SearchOnClick(object sender, EventArgs e)
        {
        }

        void SaveOnClick(object sender, EventArgs e)
        {
            //lblModo.Text = Modo.ToString();
        }

        void NewOnClick(object sender, EventArgs e)
        {
            Modo = MenuMode.Inclusão;

            btnSave.Visible = true;
            btnDelete.Visible = true;
        }
    }
}