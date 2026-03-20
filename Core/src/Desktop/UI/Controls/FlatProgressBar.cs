using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatProgressBar : ProgressBar
    {
        public FlatProgressBar()
        {
            InitializeComponent();

            // this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rec = new Rectangle(0, 0, Width, Height);

            if (ProgressBarRenderer.IsSupported)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
            }

            double scaleFactor = (Value - (double)Minimum) / (Maximum - (double)Minimum);
            rec.Width = (int)((rec.Width * scaleFactor) - 1);
            rec.Height -= -1;

            var brush = new LinearGradientBrush(rec, ForeColor, BackColor, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, 0, 0, rec.Width, rec.Height);
            e.Graphics.DrawRoundBorder(this, ColorTranslator.FromHtml("#D5DFE5"), 1, true);
        }
    }
}