
namespace System.Windows.Forms
{
    public static class ShowBox
    {
        public static void Info(string message, string title = null)
        {
            if (string.IsNullOrWhiteSpace(title)) title = "Info";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}