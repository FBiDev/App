using System;
using App.Core;
using App.File;

namespace App.Cohab
{
    public class Options
    {
        #region Fields
        [JsonIgnore]
        public readonly string ProductionDatabase = "COHAB-BD.COHABCT.COM.BR";
        [JsonIgnore]
        public readonly string DeveloperDatabase = @"COHAB-DSV.COHABCT.COM.BR\SQLENTERPRISE";
        [JsonIgnore]
        public readonly string DatabaseUsername = "mherman";
        [JsonIgnore]
        public readonly string DatabasePassword = "mherman";
        [JsonIgnore]
        public readonly string DatabaseCOHAB = "DB_COHAB";
        [JsonIgnore]
        private DatabaseMode databaseMode = DatabaseMode.Producao;
        #endregion

        #region Properties
        [JsonIgnore]
        public static bool IsLoaded { get; private set; }

        [JsonConverter(JsonType.Boolean)]
        public bool IsDeveloperDatabase
        {
            get { return Convert.ToBoolean(databaseMode); }
            set { databaseMode = (DatabaseMode)value.ToInt(); }
        }
        #endregion

        public bool ToggleDeveloperDatabaseMode()
        {
            IsDeveloperDatabase = !IsDeveloperDatabase;

            BancoCOHAB.Reload();

            return BancoCOHAB.Loaded;
        }
    }
}