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
                    Notes = row.Value<string>("Usuario_Notes")
                });
            }

            return list;
        }
        #endregion

        #region " _Select "
        public async Task<List<Usuario>> Pesquisar(Usuario obj)
        {
            var sql = Resources.sql_UsuarioListar;
            var parameters = GetFilters(obj);

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Usuario>> PesquisarAtivos()
        {
            var obj = new Usuario { Ativo = true };
            return await Pesquisar(obj);
        }

        public async Task<Usuario> Buscar(Usuario obj)
        {
            return (await Pesquisar(obj)).FirstOrNew();
        }

        public async Task<Usuario> BuscarLogin(string login)
        {
            var obj = new Usuario { Login = login.Trim() };
            return await Buscar(obj);
        }

        public async Task<List<Usuario>> ListarCombo()
        {
            return (await PesquisarAtivos()).Prepend(new Usuario()).ToList();
        }
        #endregion

        public async Task<bool> VerificarAcesso(string login, string sistema)
        {
            string sql = Resources.sql_UsuarioVerificarAcesso;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Login", login),
                new SqlParameter("@Sistema", sistema)
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters)).Count >= 1;
        }

        public async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino)
        {
            string sql = Resources.sql_UsuarioClonarAcessos;
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@loginDestino", loginDestino),
                new SqlParameter("@senhaPadrao", senhaPadrao),
                new SqlParameter("@loginOrigem", loginOrigem)
            };

            return (await BancoCOHAB.Executar(sql, DatabaseAction.Insert, parameters)).Success;
        }

        public async Task<bool> ResetarSenha(string login, string sistema)
        {
            string sql = Resources.sql_UsuarioTrocarSenha;
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");
            var validade = (await BancoCOHAB.DataServidor()).AddDays(30).ToShortDateString();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Login", login),
                new SqlParameter("@Sistema", sistema),
                new SqlParameter("@Senha", senhaPadrao),
                new SqlParameter("@Validade", validade)
            };

            return (await BancoCOHAB.Executar(sql, DatabaseAction.Update, parameters)).Success;
        }

        public async Task<List<Usuario>> ListarPorSetor(string setor = null, bool ativos = true)
        {
            string sql = Resources.sql_UsuarioListarPorSetor;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@setor", setor),
                new SqlParameter("@ativos", ativos)
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Usuario>> ListarPorDepartamento(string depto, bool exclusivo = false, bool ativos = false)
        {
            string sql = Resources.sql_UsuarioListarPorDepartamento;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@depto", depto),
                new SqlParameter("@exclusivo", exclusivo),
                new SqlParameter("@ativos", ativos)
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        public async Task<List<Usuario>> ListarPorMatricula(string matricula, string login = null)
        {
            string sql = Resources.sql_UsuarioListarPorMatricula;

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@matricula", matricula),
                new SqlParameter("@login", login)
            };

            return Carregar<List<Usuario>>(await BancoCOHAB.ExecutarSelect(sql, parameters));
        }

        #region " _Consistir "
        private string Consistir()
        {
            string erro = null;
            return erro;
        }
        #endregion

        #region " _Parameters "
        private List<SqlParameter> GetFilters(Usuario obj)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@Ativo", obj.Ativo),
                new SqlParameter("@Login",  obj.Login),
                new SqlParameter("@Nome", obj.Nome)
            };
        }
        #endregion
    }
}