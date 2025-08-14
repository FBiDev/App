namespace App.Core.Desktop
{
    public static class NativeMethods
    {
        #region Tests
        // [DllImport("user32.dll")]
        // private static extern int SystemParametersInfo(int uAction, int uParam, int lpvParam, int fuWinIni);

        // int SPI_SETDRAGFULLWINDOWS = 0x0025;
        // SystemParametersInfo(SPI_SETDRAGFULLWINDOWS, 0, 0, 2);
        // SystemParametersInfo(SPI_SETDRAGFULLWINDOWS, 1, 0, 2);

        // [Flags()]
        // public enum RedrawWindowFlags : uint
        // {
        //    Invalidate = 0X1,
        //    InternalPaint = 0X2,
        //    Erase = 0X4,
        //    Validate = 0X8,
        //    NoInternalPaint = 0X10,
        //    NoErase = 0X20,
        //    NoChildren = 0X40,
        //    AllChildren = 0X80,
        //    UpdateNow = 0X100,
        //    EraseNow = 0X200,
        //    Frame = 0X400,
        //    NoFrame = 0X800
        // }

        // public static int WM_NCPAINT = 0x85;

        // [DllImport("user32.dll")]
        // public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);
        #endregion
    }
}