using System.Threading.Tasks;

namespace App.Core.Desktop
{
    public static class LabelExtension
    {
        public static async Task ClearTextAfterDelay(this FlatLabel label, double seconds)
        {
            if (label.Text == string.Empty)
            {
                return;
            }

            label.ClearTask.Cancel();
            label.ClearTask = new TaskController();

            await label.ClearTask.DelayStart(seconds);

            if (label.ClearTask.IsCanceled == false)
            {
                label.Text = string.Empty;
            }
        }
    }
}