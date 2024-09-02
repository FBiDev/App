using System;
using App.Core;
using App.File;

namespace App.Cohab
{
    public class Options
    {
        #region Options
        [JsonIgnore]
        public static bool Loaded;
        #endregion

        #region System
        [JsonIgnore]
        public string DatabaseProd = "COHAB-BD.COHABCT.COM.BR";
        [JsonIgnore]
        public string DatabaseDev = @"COHAB-DSV.COHABCT.COM.BR\SQLENTERPRISE";
        [JsonIgnore]
        public string DatabaseUsername = "mherman";
        [JsonIgnore]
        public string DatabasePassword = "mherman";
        [JsonIgnore]
        public DatabaseMode DatabaseMode = DatabaseMode.Producao;

        [JsonIgnore]
        public string Database_COHAB = "DB_COHAB";

        [JsonConverter(JsonType.Boolean)]
        public bool DatabaseDevMode
        {
            get { return Convert.ToBoolean(DatabaseMode); }
            set { DatabaseMode = (DatabaseMode)value.ToInt(); }
        }

        public bool ToggleDatabaseDevMode()
        {
            DatabaseDevMode = !DatabaseDevMode;

            BancoCOHAB.Reload();

            return BancoCOHAB.Loaded;
        }
        #endregion
    }
}