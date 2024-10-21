namespace App.Cohab
{
    public static class Session
    {
        #region Fields
        private static Options _options = new Options();
        #endregion

        #region Properties
        public static Options Options
        {
            get { return _options; }
            private set { _options = value; }
        }
        #endregion
    }
}