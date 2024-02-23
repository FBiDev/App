using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatTableLayoutPanel : TableLayoutPanel
    {
        #region Defaults
        [DefaultValue(true)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [DefaultValue(typeof(DockStyle), "Top")]
        public new DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [DefaultValue(typeof(Size), "64, 34")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [DefaultValue(false)]
        public bool FillOnFormResize { get; set; }
        #endregion

        [Browsable(false)]
        public Size SizeOriginal { get; set; }

        public FlatTableLayoutPanel()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Dock = DockStyle.Top;
            Margin = new Padding(0);
            MinimumSize = new Size(64, 34);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            SizeOriginal = Size;

            int tabIndex = 0;
            foreach (Control control in Controls)
            {
                control.Enter += Control_Enter;
                control.TabIndex = tabIndex;
                tabIndex++;
            }
        }

        void Control_Enter(object sender, EventArgs e)
        {
            var pnt = Parent;

            while (pnt != null)
            {
                if (pnt is Form)
                {
                    (pnt as Form).ScrollControlIntoView(sender as Control);
                    break;
                }

                pnt = pnt.Parent;
            }
        }
    }
}
