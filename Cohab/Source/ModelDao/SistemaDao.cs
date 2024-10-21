using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using App.Cohab.Properties;
using App.Core;
using App.Core.Desktop;

namespace App.Cohab
{
    public class SistemaDao
    {
        #region " _Carregar "
        public static T Carregar<T>(DataTable table) where T : IList, new()
        {
            var list = new T();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new Sistema
                {
                    Sigla = row.Value<string>("Sistema_Sigla"),
                    Nome = row.Value<string>("Sistema_Nome")
                });
            }

            return list;
        }
        #endregion

        #region " _Select "
        public async Task<List<Sistema>> Pesquisar(Sistema obj)
        {
            var sql = Resources.sql_SistemaListar;
            var parameters = GetFilters(obj);

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<Sistema> Buscar(Sistema obj)
        {
            return (await Pesquisar(obj)).FirstOrNew();
        }

        public async Task<List<Sistema>> ListarCombo()
        {
            var obj = new Sistema { };
            return (await Pesquisar(obj)).Prepend(new Sistema()).ToList();
        }

        public async Task<List<Sistema>> ListarProprio(bool? proprio = null)
        {
            string sql = Resources.sql_SistemaListarProprio;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@proprio", proprio)
            };

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Sistema>> ListarPorUsuario(Usuario obj)
        {
            string sql = Resources.sql_SistemaListarPorUsuario;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Login", obj.Login)
            };

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }
        #endregion

        #region " _Parameters "
        private List<SqlParameter> GetFilters(Sistema obj)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@Nome", obj.Nome)
            };
        }
        #endregion
    }
}