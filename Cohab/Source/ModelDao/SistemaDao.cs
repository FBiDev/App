using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using App.Cohab.Properties;
using App.Core;

namespace App.Cohab.Dao
{
    public class SistemaDao : DaoBase
    {
        #region " _Select "
        public async Task<List<Sistema>> Listar()
        {
            return await Select();
        }

        public async Task<List<Sistema>> Pesquisar(Sistema obj)
        {
            return await Select(obj);
        }

        public async Task<Sistema> Buscar(Sistema obj)
        {
            return (await Select(obj)).FirstOrNew();
        }
        #endregion

        #region " _Select_Custom "
        public async Task<List<Sistema>> ListarCombo()
        {
            var obj = new Sistema();
            return (await Select(obj)).PrependNew();
        }

        public async Task<List<Sistema>> ListarProprio(bool? proprio = null)
        {
            var sql = new SqlQuery(Resources.sql_SistemaListarProprio, DatabaseAction.Select,
                P("@proprio", proprio));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<Sistema>> ListarPorUsuario(Usuario obj)
        {
            obj = obj ?? new Usuario();

            var sql = new SqlQuery(Resources.sql_SistemaListarPorUsuario, DatabaseAction.Select,
                P("@Login", obj.Login));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Actions "
        private async Task<List<Sistema>> Select(Sistema obj = null)
        {
            obj = obj ?? new Sistema();

            var sql = new SqlQuery(Resources.sql_SistemaListar, DatabaseAction.Select,
                P("@Nome", obj.Nome));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Load "
        private List<Sistema> Load(DataTable table)
        {
            return table.ProcessRows<Sistema>((row, lst) =>
            {
                var entity = new Sistema
                {
                    Sigla = row.Value<string>("Sistema_Sigla"),
                    Nome = row.Value<string>("Sistema_Nome")
                };

                lst.Add(entity);
            });
        }
        #endregion
    }
}