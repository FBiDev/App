using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using App.Cohab.Properties;
using App.Core;

namespace App.Cohab.Dao
{
    public class UsuarioDao : DaoBase
    {
        #region " _Select "
        public async Task<List<Usuario>> Listar()
        {
            return await Select();
        }

        public async Task<List<Usuario>> Pesquisar(Usuario obj)
        {
            return await Select(obj);
        }

        public async Task<Usuario> Buscar(Usuario obj)
        {
            return (await Select(obj)).FirstOrNew();
        }
        #endregion

        #region " _Select_Custom "
        public async Task<Usuario> BuscarLogin(string login)
        {
            var obj = new Usuario { Login = login.Trim() };
            return await Buscar(obj);
        }

        public async Task<List<Usuario>> PesquisarAtivos()
        {
            var obj = new Usuario { Ativo = true };
            return await Select(obj);
        }

        public async Task<List<Usuario>> ListarCombo()
        {
            return (await PesquisarAtivos()).PrependNew();
        }

        public async Task<bool> VerificarAcesso(string login, string sistema)
        {
            var sql = new SqlQuery(Resources.sql_UsuarioVerificarAcesso, DatabaseAction.Select,
                P("@Login", login),
                P("@Sistema", sistema));

            return Load(await BancoCOHAB.ExecutarSelect(sql)).Count >= 1;
        }

        public async Task<List<Usuario>> ListarPorSetor(string setor = null, bool ativos = true)
        {
            var sql = new SqlQuery(Resources.sql_UsuarioListarPorSetor, DatabaseAction.Select,
                P("@setor", setor),
                P("@ativos", ativos));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<Usuario>> ListarPorDepartamento(string depto, bool exclusivo = false, bool ativos = false)
        {
            var sql = new SqlQuery(Resources.sql_UsuarioListarPorDepartamento, DatabaseAction.Select,
                P("@depto", depto),
                P("@exclusivo", exclusivo),
                P("@ativos", ativos));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }

        public async Task<List<Usuario>> ListarPorMatricula(string matricula, string login = null)
        {
            var sql = new SqlQuery(Resources.sql_UsuarioListarPorMatricula, DatabaseAction.Select,
                P("@matricula", matricula),
                P("@login", login));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Actions_Custom "
        public async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino)
        {
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");

            var sql = new SqlQuery(Resources.sql_UsuarioClonarAcessos, DatabaseAction.Insert,
                P("@loginDestino", loginDestino),
                P("@senhaPadrao", senhaPadrao),
                P("@loginOrigem", loginOrigem));

            return (await BancoCOHAB.Executar(sql)).Success;
        }

        public async Task<bool> ResetarSenha(string login, string sistema)
        {
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");
            var validade = (await BancoCOHAB.DataServidor()).AddDays(30).ToShortDateString();

            var sql = new SqlQuery(Resources.sql_UsuarioTrocarSenha, DatabaseAction.Update,
                P("@Login", login),
                P("@Sistema", sistema),
                P("@Senha", senhaPadrao),
                P("@Validade", validade));

            return (await BancoCOHAB.Executar(sql)).Success;
        }
        #endregion

        #region " _Actions "
        private async Task<List<Usuario>> Select(Usuario obj = null)
        {
            obj = obj ?? new Usuario();

            var sql = new SqlQuery(Resources.sql_UsuarioListar, DatabaseAction.Select,
                P("@Ativo", obj.Ativo),
                P("@Login", obj.Login),
                P("@Nome", obj.Nome));

            sql.SetOrderBy(new SqlOrder(SqlDirection.ASC, "Usuario_Nome"));

            return Load(await BancoCOHAB.ExecutarSelect(sql));
        }
        #endregion

        #region " _Load "
        private List<Usuario> Load(DataTable table)
        {
            return table.ProcessRows<Usuario>((row, lst) =>
            {
                var entity = new Usuario
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

                lst.Add(entity);
            });
        }
        #endregion
    }
}