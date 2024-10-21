namespace App.Core
{
    internal class ExceptionProcessed
    {
        public readonly bool HasLink;
        public readonly string Link;
        public readonly bool OleDb;
        public readonly bool ExternalDll;
        public readonly string Message;

        public ExceptionProcessed(bool haslink, string link, bool oledb, bool externalDll, string msg)
        {
            HasLink = haslink;
            Link = link;
            OleDb = oledb;
            ExternalDll = externalDll;
            Message = msg;
        }
    }
}