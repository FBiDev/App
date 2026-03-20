using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using App.Cohab.Properties;
using App.Core;

namespace App.Cohab.DataAccess
{
    public class SistemaDao : DaoBase
    {
        #region " _Select "
        public async Task<List<SistemaModel>> Listar()
        {
            return await Select();
        }

        public async Task<List<SistemaModel>> Pesquisar(SistemaModel obj)
        {
            return await Select(obj);
        }

        public async Task<SistemaModel> Buscar(SistemaModel obj)
        {
            return (await Select(obj)).First();
        }
        #endregion

        #region " _Select_Custom "
        public async Task<List<SistemaModel>> ListarCombo()
        {
            return (await Select()).PrependNewItem();
        }

        public async Task<List<SistemaModel>> ListarProprio(bool? proprio = null)
        {
            var sql = new SqlQuery(
                Resources.sql_SistemaListarProprio,
                DatabaseAction.Select,
                P("@proprio", proprio));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<SistemaModel>> ListarPorUsuario(UsuarioModel obj)
        {
            obj = obj ?? new UsuarioModel();

            var sql = new SqlQuery(
                Resources.sql_SistemaListarPorUsuario,
                DatabaseAction.Select,
                P("@Login", obj.Login));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Load "
        private async Task<List<SistemaModel>> Select(SistemaModel obj = null)
        {
            obj = obj ?? new SistemaModel();

            var sql = new SqlQuery(
                Resources.sql_SistemaListar,
                DatabaseAction.Select,
                P("@Nome", obj.Nome));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        private List<SistemaModel> Load(DataTable table)
        {
            return table.ProcessRows<SistemaModel>((row, lst) =>
            {
                var model = new SistemaModel
                {
                    Sigla = row.Value<string>("Sistema_Sigla"),
                    Nome = row.Value<string>("Sistema_Nome")
                };

                lst.Add(model);
            });
        }
        #endregion
    }
}