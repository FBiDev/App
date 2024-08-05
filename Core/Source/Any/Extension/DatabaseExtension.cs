using System.Data;

namespace App.Core
{
    public static class DatabaseExtension
    {
        public static string GetState(this IDbCommand cmd)
        {
            return cmd.Connection.State + ", " + cmd.Connection.Database + ", Params[" + cmd.Parameters.Count + "]";
        }
    }
}