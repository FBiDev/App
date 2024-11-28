using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace App.Core
{
    internal static class ExceptionManagerAny
    {
        internal static ExceptionProcessed Process(Exception ex, string argumentString = null)
        {
            string customMessage = string.Empty;
            string errorLineBreak = "\r\n\r\n";

            Exception exceptionError = ex;
            var exceptionType = ex.GetType();

            OleDbError errorDb;
            bool isOleDbException = false;

            bool link = false;
            string linkStr = string.Empty;

            bool externalDLL = false;

            string innerMessage = string.Empty;

            if (ex.InnerException != null)
            {
                innerMessage = ex.InnerException.Message + errorLineBreak;
            }

            AddDataError(ex);

            if (exceptionType == typeof(Exception))
            {
                if (exceptionError.TargetSite.Module.Name == "ControleCasamentos.exe")
                {
                    // No Database File
                    if (exceptionError.TargetSite.Name == "CreateConnection")
                    {
                        // CustomMessage = "Arquivo de Banco de dados não encontrado";
                    }
                }

                customMessage = ex.Message + errorLineBreak + innerMessage + exceptionError.Data["Error"];
            }
            else if (exceptionType == typeof(TargetInvocationException))
            {
                customMessage = ex.Message + errorLineBreak + innerMessage + exceptionError.Data["Error"];
            }
            else if (exceptionType == typeof(InvalidOperationException))
            {
                exceptionError = (InvalidOperationException)ex;
                var message = ((InvalidOperationException)ex).Message;

                if (exceptionError.TargetSite.Module.Name == "System.Data.dll")
                {
                    switch (exceptionError.TargetSite.Name)
                    {
                        case "GetDataSource":
                            // Engine not Installed
                            link = true;
                            linkStr = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=54920";

                            customMessage = "Motor Excel não instalado, clique em OK para ir fazer o download";
                            break;
                        case "TryOpenConnection":
                            // Not Closed Connectiond
                            customMessage = "Falha na conexão com o Banco de Dados";
                            break;
                        case "Prepare":
                            customMessage = message + errorLineBreak + exceptionError.Data["Error"];
                            break;
                    }
                }
                else if (exceptionError.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    // Database is not Open
                    if (exceptionError.TargetSite.Name == "InitializeForReader")
                    {
                        customMessage = "Banco de dados não está aberto";
                    }
                }
            }
            else if (exceptionType == typeof(ArgumentException))
            {
                exceptionError = (ArgumentException)ex;

                if (exceptionError.TargetSite.Module.Name == "System.Data.dll")
                {
                    // Provider
                    if (exceptionError.TargetSite.Name == "ValidateProvider")
                    {
                        customMessage = "Falha ao validar Provider do arquivo";
                    }
                }
                else if (exceptionError.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    // Datasource
                    if (exceptionError.TargetSite.Name == "Open")
                    {
                        customMessage = "Falha na conexão com o Banco de Dados";
                    }
                }
            }
            else if (exceptionType == typeof(SqlException))
            {
                exceptionError = (SqlException)ex;
                var messages = ((SqlException)ex).Errors;

                if (exceptionError.TargetSite.Module.Name == "System.Data.dll")
                {
                    if (exceptionError.TargetSite.DeclaringType.FullName == "System.Data.SqlClient.SqlInternalConnectionTds")
                    {
                        // CustomMessage = "Falha na conexão com o Banco de Dados";
                        // CustomMessage += errorLineBreak + Messages[0];
                    }

                    customMessage = messages[0] + errorLineBreak + exceptionError.Data["Error"];
                }
            }
            else if (exceptionType == typeof(FormatException))
            {
                exceptionError = (FormatException)ex;

                if (exceptionError.TargetSite.Module.Name == "mscorlib.dll")
                {
                    switch (exceptionError.TargetSite.Name)
                    {
                        case "StringToNumber":
                            customMessage = "Falha ao converter Texto para Número:\r\n" + argumentString;
                            break;
                        case "ParseExactMultiple":
                            customMessage = "Falha ao converter Texto para Data:\r\n" + argumentString;
                            break;
                        case "ParseDouble":
                            customMessage = "Falha ao converter Texto para Double:\r\n" + argumentString;
                            break;
                        case "Parse":
                            customMessage = "Falha ao converter Texto para Boolean:\r\n" + argumentString;
                            break;
                    }
                }
            }
            else if (exceptionType == typeof(InvalidCastException))
            {
                exceptionError = (InvalidCastException)ex;

                if (exceptionError.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    // Conversion
                    if (exceptionError.TargetSite.Name == "BindParameter")
                    {
                        customMessage = "Falha ao converter Parâmetro:\r\n" + argumentString;
                    }
                }
            }
            else if (exceptionType == typeof(BadImageFormatException))
            {
                exceptionError = (BadImageFormatException)ex;

                if (exceptionError.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    // No ODBC Driver installed
                    if (exceptionError.TargetSite.Name == "sqlite3_config_none")
                    {
                        customMessage = "Falha na conexão com o Banco de Dados";
                    }
                }
            }
            else if (exceptionType == typeof(NotSupportedException))
            {
                exceptionError = (NotSupportedException)ex;

                if (exceptionError.TargetSite.Module.Name == "System.Data.SQLite.dll")
                {
                    // SQLite Version
                    if (exceptionError.TargetSite.Name == "Open")
                    {
                        customMessage = "Versão do SQLite incorreta";
                    }
                }
            }
            else if (exceptionType == typeof(FileNotFoundException))
            {
                // Error = ((FileNotFoundException)ex);
                customMessage = "Arquivo não encontrado";
            }
            else if (exceptionType == typeof(DllNotFoundException))
            {
                // Error = ((DllNotFoundException)ex);
                customMessage = "Arquivo DLL não encontrado\r\n" + ex.Message;
            }
            else if (exceptionType == typeof(OleDbException))
            {
                isOleDbException = true;
                errorDb = ((OleDbException)ex).Errors[0];

                switch (errorDb.NativeError)
                {
                    // ISAM Extended Properties
                    case -69141536: break;

                    // Opened File
                    case -67568648:
                        customMessage = "Arquivo já esta aberto em outro programa";
                        break;

                    // Excel Tab Wrong Name
                    case -537199594: break;
                }
            }
            else
            {
                string exceptionTypeString = "ExType  : " + exceptionType;
                string target = "\r\nTarget   : " + exceptionError.TargetSite.Module.Name;
                string method = "\r\nMethod : " + exceptionError.TargetSite.Name;

                customMessage = "Erro inesperado não tratado";
                customMessage += errorLineBreak + exceptionTypeString + target + method;
            }

            var processed = new ExceptionProcessed(link, linkStr, isOleDbException, externalDLL, customMessage);
            return processed;
        }

        private static void AddDataError(Exception ex)
        {
            var stackTrace = string.Empty;

            try
            {
                stackTrace += ObjectManager.GetStackTrace(null);
            }
            catch
            {
                stackTrace += string.Empty;
            }

            ex.Data.Remove("Error");
            ex.Data.Add("Error", stackTrace);
        }
    }
}