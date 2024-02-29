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
    public class Sistema
    {
        #region " _Propriedades "
        public string Sigla { get; set; }
        public string Nome { get; set; }
        #endregion

        #region " _Construtor "
        public Sistema()
        {
            Sigla = Nome = string.Empty;
        }
        #endregion

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

        #region " _Parameters "
        List<SqlParameter> GetFilters(Sistema obj)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@Nome", obj.Nome)
            };
        }
        #endregion

        #region " _Select "
        async Task<List<Sistema>> Search(Sistema obj)
        {
            var sql = Resources.SistemaListar;
            var parameters = GetFilters(obj);

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Sistema>> Pesquisar()
        {
            var obj = new Sistema { };
            return (await Search(obj));
        }

        public async Task<Sistema> Buscar(Sistema obj)
        {
            return (await Search(obj)).FirstOrNew();
        }

        public async Task<List<Sistema>> ListarCombo()
        {
            return (await Pesquisar()).Prepend(new Sistema()).ToList();
        }

        public async static Task<List<Sistema>> ListarProprio(bool? proprio = null)
        {
            string sql = Resources.SistemaListarProprio;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@proprio", proprio)
            };

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public static async Task<List<Sistema>> ListarPorUsuario(Usuario obj)
        {
            string sql = Resources.SistemaListarPorUsuario;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Login", obj.Login)
            };

            return Carregar<List<Sistema>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }
        #endregion
    }
}