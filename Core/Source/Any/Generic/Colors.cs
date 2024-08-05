using System.Drawing;

namespace App.Core
{
    public static class Colors
    {
        public static Color HTML(string htmlCode)
        {
            return ColorTranslator.FromHtml("#" + htmlCode);
        }

        public static Color RGB(int red, int green, int blue)
        {
            return Color.FromArgb(red, green, blue);
        }
    }
}