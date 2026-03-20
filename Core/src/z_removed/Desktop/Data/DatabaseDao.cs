using System.Collections;
using System.Data;

namespace App.Core.Desktop
{
#pragma warning disable
    internal class DatabaseDao
    {
#pragma warning restore
        #region " _Load "
        public static T Load<T>(DataTable table) where T : IList, new()
        {
            return Core.DatabaseDao.Load<T>(table, typeof(DataRowCastExtension));
        }
        #endregion
    }
}