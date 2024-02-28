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
    public class UsuarioDao : DatabaseDao
    {
        #region " _Carregar "
        public static T Carregar<T>(DataTable table) where T : IList, new()
        {
            var list = new T();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new Usuario
                {
                    Login = row.Value<string>("Usuario_Login").Trim().ToUpper(),
                    Nome = row.Value<string>("Usuario_Nome").Trim(),
                    Matricula = row.Value<string>("Usuario_Matricula"),
                    Ramal = row.Value<string>("Usuario_Ramal"),
                    Email = row.Value<string>("Usuario_Email"),
                    Chefia = row.Value<bool>("Usuario_Chefia"),
                    Ativo = row.Value<bool>("Usuario_Ativo"),
                    Visivel = row.Value<bool>("Usuario_Visivel"),
                    Notes = row.Value<string>("Usuario_Notes"),
                });
            }
            return list;
        }
        #endregion

        #region " _Consistir "
        private string Consistir()
        {
            string erro = null;
            return erro;
        }
        #endregion

        #region " _Parameters "
        List<SqlParameter> GetFilters(Usuario obj)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@Ativo", obj.Ativo),
                new SqlParameter("@Login",  obj.Login),
                new SqlParameter("@Nome", obj.Nome)
            };
        }
        #endregion

        #region " _Select "
        public async Task<List<Usuario>> Pesquisar(Usuario obj)
        {
            var sql = Resources.UsuarioListar;
            var parameters = GetFilters(obj);

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Usuario>> PesquisarAtivos()
        {
            var obj = new Usuario { Ativo = true };
            return (await Pesquisar(obj));
        }

        public async Task<Usuario> Buscar(Usuario obj)
        {
            return (await Pesquisar(obj)).FirstOrNew();
        }

        public async Task<Usuario> BuscarLogin(string login)
        {
            var obj = new Usuario { Login = login.Trim() };
            return (await Buscar(obj));
        }

        public async Task<List<Usuario>> ListarCombo()
        {
            return (await PesquisarAtivos()).Prepend(new Usuario()).ToList();
        }
        #endregion

        public async Task<bool> VerificarAcesso(string login, string sistema)
        {
            string sql = Resources.UsuarioVerificarAcesso;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Login", login),
                new SqlParameter("@Sistema", sistema),
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters)).Count >= 1;
        }

        public static async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino, string usuarioLogado)
        {
            string lSQLRemove = " DELETE FROM [DB_COHAB].[dbo].[UsuarioSistema] " +
                                " WHERE [UsuarioSistema_UsuarioLogin] = '" + loginDestino + "' ";

            var lParams = new List<SqlParameter> { };
            await BancoCOHAB.Executar(lSQLRemove, DatabaseAction.Delete, lParams);

            string sql = " INSERT INTO [DB_COHAB].[dbo].[UsuarioSistema] " +
             "  ([UsuarioSistema_Senha] " +
               " ,[UsuarioSistema_Ativo] " +
               " ,[UsuarioSistema_Validade] " +
               " ,[UsuarioSistema_Free] " +
               " ,[UsuarioSistema_UsuarioLogin] " +
               " ,[UsuarioSistema_GrupoId] " +
               " ,[UsuarioSistema_SistemaSigla] " +
               " ,[UsuarioSistema_Micro] " +
               " ,[UsuarioSistema_UsuarioRede] " +
               " ,[UsuarioSistema_Data] " +
               " ,[UsuarioSistema_Hora]) " +
               " Select " +
               "         '³Ã9zi³Ã9zi' " +
               "      ,1 " +
               " ,DATEADD(MONTH, 3, GETDATE()) " +
               " ,5 " +
               " ,'" + loginDestino + "' " +
               " ,[UsuarioSistema_GrupoId] " +
               " ,[UsuarioSistema_SistemaSigla] " +
               " ,[UsuarioSistema_Micro] " +
               " ,[UsuarioSistema_UsuarioRede] " +
               " ,[UsuarioSistema_Data] " +
               " ,[UsuarioSistema_Hora] " +
           " FROM [DB_COHAB].[dbo].[UsuarioSistema] " +
           " where [UsuarioSistema_UsuarioLogin] = '" + loginOrigem + "' ";

            return (await BancoCOHAB.Executar(sql, DatabaseAction.Insert, lParams)).Success;
        }

        public async Task<bool> ResetarSenha(string login, string sistema)
        {
            string sql = Resources.UsuarioTrocarSenha;
            string senhaPadrao = Funcoes.CriptografarSenha("senhasenha");
            string validade = (await BancoCOHAB.DataServidor()).AddDays(30).ToShortDateString();

            var lParams = new List<SqlParameter>
            {
                new SqlParameter("@Login", login),
                new SqlParameter("@Sistema", sistema),
                new SqlParameter("@Senha", senhaPadrao),
                new SqlParameter("@Validade", validade),
            };

            return (await BancoCOHAB.Executar(sql, DatabaseAction.Update, lParams)).Success;
        }

        public async Task<List<Usuario>> ListarPorSetor(string setor = null, bool ativos = true)
        {
            string sql = Resources.UsuarioListarPorSetor;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@setor", setor),
                new SqlParameter("@ativos", ativos)
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public static async Task<List<Usuario>> ListarPorDepartamento(string depto, bool exclusivo = false, bool soAtivos = false)
        {
            string sql = "SELECT U.Usuario_Matricula, U.Usuario_Nome, U.Usuario_Login, U.Usuario_Email, U.Usuario_Notes  " +
                   "FROM [vw_lotacaodp] L WITH (nolock) " +
                   " INNER JOIN Usuario U WITH (nolock)  ON U.Usuario_Login = L.Usuario_Login  WHERE 1 = 1  ";

            if (depto.HasValue())
            {
                sql += " AND L.[departamento_sigla] = '" + depto.Trim() + "'";
                if (exclusivo) { sql += " AND L.[setor_sigla] is null "; }
            }

            if (soAtivos) { sql += " AND U.usuario_ativo = 1 "; }
            sql += " ORDER BY U.[Usuario_Nome] ASC ";

            var lParams = new List<SqlParameter> { };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, lParams));
        }

        public static async Task<List<Usuario>> ListarPorMatricula(string matricula, string login = null)
        {
            string sql = "SELECT * " +
                   " FROM [vw_lotacaodp] L WITH (nolock) " +
                   " INNER JOIN Usuario U WITH (nolock)  ON U.Usuario_Login = L.Usuario_Login  WHERE 1 = 1  ";

            if (matricula != null) { sql += " AND L.[matricula] = '" + matricula.Trim() + "'"; }

            if (login != null) { sql += " AND U.[usuario_login] = '" + login.Trim() + "'"; }

            sql += " ORDER BY U.[Usuario_Nome] ASC ";

            var lParams = new List<SqlParameter> { };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, lParams));
        }
    }
}