using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class MenuBar : UserControl
    {
        private MenuMode _modo;

        public MenuBar()
        {
            InitializeComponent();
            TabStop = false;
            Modo = MenuMode.Consulta;
        }

        public MenuMode Modo
        {
            get
            {
                return _modo;
            }

            set
            {
                _modo = value;
                lblModo.Text = _modo.ToString();
            }
        }

        private void MenuBarOnSizeChanged(object sender, EventArgs e)
        {
            int ctrs = pnlMenuBar.Controls.Count;
            int each = pnlMenuBar.Width / ctrs;
            int cItem = 0;

            foreach (Control c in pnlMenuBar.Controls)
            {
                var newLocation = new Point((each / 2) - (c.Width / 2) + (cItem * each), c.Location.Y);
                c.Location = newLocation;
                cItem++;
            }
        }

        private void SearchOnClick(object sender, EventArgs e)
        {
        }

        private void SaveOnClick(object sender, EventArgs e)
        {
            // lblModo.Text = Modo.ToString();
        }

        private void NewOnClick(object sender, EventArgs e)
        {
            Modo = MenuMode.Inclusão;

            btnSave.Visible = true;
            btnDelete.Visible = true;
        }
    }
}