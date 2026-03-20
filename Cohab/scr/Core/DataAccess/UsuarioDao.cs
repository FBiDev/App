using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using App.Cohab.Properties;
using App.Core;

namespace App.Cohab.DataAccess
{
    public class UsuarioDao : DaoBase
    {
        #region " _Select "
        public async Task<List<UsuarioModel>> Listar()
        {
            return await Select();
        }

        public async Task<List<UsuarioModel>> Pesquisar(UsuarioModel obj)
        {
            return await Select(obj);
        }

        public async Task<UsuarioModel> Buscar(UsuarioModel obj)
        {
            return (await Select(obj)).First();
        }

        public async Task<DateTime> DataServidor()
        {
            return (await BancoCOHAB.DataServidor());
        }
        #endregion

        #region " _Select_Custom "
        public async Task<UsuarioModel> BuscarLogin(string login)
        {
            var obj = new UsuarioModel { Login = login.Trim() };
            return await Buscar(obj);
        }

        public async Task<List<UsuarioModel>> PesquisarAtivos()
        {
            var obj = new UsuarioModel { Ativo = true };
            return await Select(obj);
        }

        public async Task<List<UsuarioModel>> ListarCombo()
        {
            return (await PesquisarAtivos()).PrependNewItem();
        }

        public async Task<bool> VerificarAcesso(string login, string sistema)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioVerificarAcesso,
                DatabaseAction.Select,
                P("@Login", login),
                P("@Sistema", sistema));

            return Load(await BancoCOHAB.ExecutarSelect(sql)).Count >= 1;
        }

        public async Task<List<UsuarioModel>> ListarPorSetor(string setor = null, bool ativos = true)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioListarPorSetor,
                DatabaseAction.Select,
                P("@setor", setor),
                P("@ativos", ativos));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<UsuarioModel>> ListarPorDepartamento(string depto, bool exclusivo = false, bool ativos = false)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioListarPorDepartamento,
                DatabaseAction.Select,
                P("@depto", depto),
                P("@exclusivo", exclusivo),
                P("@ativos", ativos));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<UsuarioModel>> ListarPorMatricula(string matricula, string login = null)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioListarPorMatricula,
                DatabaseAction.Select,
                P("@matricula", matricula),
                P("@login", login));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Actions_Custom "
        public async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino, string senhaPadrao)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioClonarAcessos,
                DatabaseAction.Insert,
                P("@loginDestino", loginDestino),
                P("@senhaPadrao", senhaPadrao),
                P("@loginOrigem", loginOrigem));

            return (await BancoCOHAB.Executar(sql)).Success;
        }

        public async Task<bool> ResetarSenha(string login, string sistema, string senhaPadrao, string validade)
        {
            var sql = new SqlQuery(
                Resources.sql_UsuarioTrocarSenha,
                DatabaseAction.Update,
                P("@Login", login),
                P("@Sistema", sistema),
                P("@Senha", senhaPadrao),
                P("@Validade", validade));

            return (await BancoCOHAB.Executar(sql)).Success;
        }
        #endregion

        #region " _Load "
        private async Task<List<UsuarioModel>> Select(UsuarioModel obj = null)
        {
            obj = obj ?? new UsuarioModel();

            var sql = new SqlQuery(
                Resources.sql_UsuarioListar,
                DatabaseAction.Select,
                P("@Ativo", obj.Ativo),
                P("@Login", obj.Login),
                P("@Nome", obj.Nome));

            sql.SetOrderBy(new SqlOrder(SqlDirection.ASC, "Usuario_Nome"));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        private List<UsuarioModel> Load(DataTable table)
        {
            return table.ProcessRows<UsuarioModel>((row, lst) =>
            {
                var model = new UsuarioModel
                {
                    Login = row.Value<string>("Usuario_Login").Trim().ToUpper(),
                    Nome = row.Value<string>("Usuario_Nome").Trim(),
                    Matricula = row.Value<string>("Usuario_Matricula"),
                    Ramal = row.Value<string>("Usuario_Ramal"),
                    Email = row.Value<string>("Usuario_Email"),
                    Chefia = row.Value<bool>("Usuario_Chefia"),
                    Ativo = row.Value<bool>("Usuario_Ativo"),
                    Visivel = row.Value<bool>("Usuario_Visivel"),
                    Notes = row.Value<string>("Usuario_Notes")
                };

                lst.Add(model);
            });
        }
        #endregion
    }
}