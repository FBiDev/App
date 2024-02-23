using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace App.Core
{
    static class ExceptionManager
    {
        static void AddDataError(Exception ex)
        {
            var stackTrace = string.Empty;

            try
            {
                stackTrace += ObjectManager.GetStackTrace(null);
            }
            catch
            {
                stackTrace += "";
            }

            ex.Data.Remove("Error");
            ex.Data.Add("Error", stackTrace);
        }

        internal static ExceptionProcessed Process(Exception ex, string ArgumentString = null)
        {
            string CustomMessage = "";
            string errorLineBreak = "\r\n\r\n";

            Exception Error = ex;
            var ExType = ex.GetType();

            OleDbError ErrorDb;
            bool OleDb = false;

            bool link = false;
            string linkStr = string.Empty;

            bool externalDLL = false;

            string innerMessage = string.Empty;

            if (ex.InnerException != null)
                innerMessage = ex.InnerException.Message + errorLineBreak;

            AddDataError(ex);

            if (ExType == typeof(Exception))
            {
                if (Error.TargetSite.Module.Name == "ControleCasamentos.exe")
                {
                    //No Database File
                    if (Error.TargetSite.Name == "CreateConnection")
                    {
                        //CustomMessage = "Arquivo de Banco de dados não encontrado";
                    }
                }
                CustomMessage = ex.Message + errorLineBreak + innerMessage + Error.Data["Error"];
            }
            else if (ExType == typeof(TargetInvocationException))
            {
                CustomMessage = ex.Message + errorLineBreak + innerMessage + Error.Data["Error"];
            }
            else if (ExType == typeof(InvalidOperationException))
            {
                Error = ((InvalidOperationException)ex);
                var Message = ((InvalidOperationException)ex).Message;

                if (Error.TargetSite.Module.Name == "System.Data.dll")
                {
                    switch (Error.TargetSite.Name)
                    {
                        case "GetDataSource":
                            //Engine not Installed
                            link = true;
                            linkStr = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=54920";

                            CustomMessage = "Motor Excel não instalado, clique em OK para ir fazer o download";
                            break;
                        case "TryOpenConnection":
                            //Not Closed Connectiond
                            CustomMessage = "Falha na conexão com o Banco de Dados";
                            break;
                        case "Prepare":
                            CustomMessage = Message + errorLineBreak + Error.Data["Error"];
                            break;
                    }
                }
                else if (Error.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    //Database is not Open
                    if (Error.TargetSite.Name == "InitializeForReader")
                    {
                        CustomMessage = "Banco de dados não está aberto";
                    }
                }
            }
            else if (ExType == typeof(ArgumentException))
            {
                Error = ((ArgumentException)ex);

                if (Error.TargetSite.Module.Name == "System.Data.dll")
                {
                    //Provider
                    if (Error.TargetSite.Name == "ValidateProvider")
                    {
                        CustomMessage = "Falha ao validar Provider do arquivo";
                    }
                }
                else if (Error.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    //Datasource
                    if (Error.TargetSite.Name == "Open")
                    {
                        CustomMessage = "Falha na conexão com o Banco de Dados";
                    }
                }
            }
            else if (ExType == typeof(SqlException))
            {
                Error = ((SqlException)ex);
                var Messages = ((SqlException)ex).Errors;

                if (Error.TargetSite.Module.Name == "System.Data.dll")
                {
                    if (Error.TargetSite.DeclaringType.FullName == "System.Data.SqlClient.SqlInternalConnectionTds")
                    {
                        //CustomMessage = "Falha na conexão com o Banco de Dados";
                        //CustomMessage += errorLineBreak + Messages[0];
                    }

                    CustomMessage = Messages[0] + errorLineBreak + Error.Data["Error"];
                }
            }
            else if (ExType == typeof(FormatException))
            {
                Error = ((FormatException)ex);

                if (Error.TargetSite.Module.Name == "mscorlib.dll")
                {
                    switch (Error.TargetSite.Name)
                    {
                        case "StringToNumber": CustomMessage = "Falha ao converter Texto para Número:\r\n" + ArgumentString; break;
                        case "ParseExactMultiple": CustomMessage = "Falha ao converter Texto para Data:\r\n" + ArgumentString; break;
                        case "ParseDouble": CustomMessage = "Falha ao converter Texto para Double:\r\n" + ArgumentString; break;
                        case "Parse": CustomMessage = "Falha ao converter Texto para Boolean:\r\n" + ArgumentString; break;
                    }
                }
            }
            else if (ExType == typeof(InvalidCastException))
            {
                Error = ((InvalidCastException)ex);

                if (Error.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    //Conversion
                    if (Error.TargetSite.Name == "BindParameter")
                    {
                        CustomMessage = "Falha ao converter Parâmetro:\r\n" + ArgumentString;
                    }
                }
            }
            else if (ExType == typeof(BadImageFormatException))
            {
                Error = ((BadImageFormatException)ex);

                if (Error.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    //No ODBC Driver installed
                    if (Error.TargetSite.Name == "sqlite3_config_none")
                    {
                        CustomMessage = "Falha na conexão com o Banco de Dados";
                    }
                }
            }
            else if (ExType == typeof(NotSupportedException))
            {
                Error = ((NotSupportedException)ex);

                if (Error.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    //SQLite Version
                    if (Error.TargetSite.Name == "Open")
                    {
                        CustomMessage = "Versão do SQLite incorreta";
                    }
                }
            }
            else if (ExType == typeof(FileNotFoundException))
            {
                //Error = ((FileNotFoundException)ex);

                CustomMessage = "Arquivo não encontrado";
            }
            else if (ExType == typeof(DllNotFoundException))
            {
                //Error = ((DllNotFoundException)ex);

                CustomMessage = "Arquivo DLL não encontrado\r\n" + ex.Message;
            }
            else if (ExType == typeof(OleDbException))
            {
                OleDb = true;
                ErrorDb = ((OleDbException)ex).Errors[0];

                switch (ErrorDb.NativeError)
                {
                    //ISAM Extended Properties
                    case -69141536: break;
                    //Opened File
                    case -67568648: CustomMessage = "Arquivo já esta aberto em outro programa"; break;
                    //Excel Tab Wrong Name
                    case -537199594: break;
                }
            }
            else
            {
                string ExTypeStr = "ExType  : " + ExType;
                string Target = "\r\nTarget   : " + Error.TargetSite.Module.Name;
                string Method = "\r\nMethod : " + Error.TargetSite.Name;

                CustomMessage = "Erro inesperado não tratado";
                CustomMessage += errorLineBreak + ExTypeStr + Target + Method;
            }

            var Processed = new ExceptionProcessed(link, linkStr, OleDb, externalDLL, CustomMessage);
            return Processed;
        }
    }

    class ExceptionProcessed
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