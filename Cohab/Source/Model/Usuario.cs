using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using App.Cohab.Dao;

namespace App.Cohab
{
    public class Usuario
    {
        #region " _Fields "
        private static readonly UsuarioDao DAO = new UsuarioDao();
        #endregion

        #region " _Constructor "
        public Usuario()
        {
            Login = Nome = Apelido = Ramal = Matricula = string.Empty;
        }
        #endregion

        #region " _Properties "
        public string Login { get; set; }

        [Style(Width = 270)]
        public string Nome { get; set; }

        [Display(AutoGenerateField = false)]
        public string Apelido { get; set; }

        [Style(Align = ColumnAlign.Center)]
        public string Matricula { get; set; }

        [Style(Align = ColumnAlign.Center)]
        public string Ramal { get; set; }

        [Display(AutoGenerateField = false)]
        public string Email { get; set; }

        public bool Ativo { get; set; }

        [Display(AutoGenerateField = false)]
        public int TipoColaborador { get; set; }

        public bool Chefia { get; set; }

        public bool Visivel { get; set; }

        [Display(AutoGenerateField = false)]
        public string Notes { get; set; }
        #endregion

        #region " _Static_Methods "
        public static async Task<List<Usuario>> Listar()
        {
            return await DAO.Listar();
        }

        public static async Task<List<Usuario>> Pesquisar(Usuario obj)
        {
            return await DAO.Pesquisar(obj);
        }

        public static async Task<Usuario> Buscar(Usuario obj)
        {
            return await DAO.Buscar(obj);
        }

        public static async Task<bool> VerificarAcesso(string login, string sistema)
        {
            return await DAO.VerificarAcesso(login, sistema);
        }

        public static async Task<List<Usuario>> ListarPorSetor(string setor = null, bool soAtivos = true)
        {
            return await DAO.ListarPorSetor(setor, soAtivos);
        }

        public static async Task<List<Usuario>> ListarPorDepartamento(string depto, bool exclusivo = false, bool ativos = false)
        {
            return await DAO.ListarPorDepartamento(depto, exclusivo, ativos);
        }

        public static async Task<List<Usuario>> ListarPorMatricula(string matricula, string login = null)
        {
            return await DAO.ListarPorMatricula(matricula, login);
        }

        public static async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino)
        {
            return await DAO.ClonarAcessos(loginOrigem, loginDestino);
        }

        public static async Task<bool> ResetarSenha(string login, string sistema)
        {
            return await DAO.ResetarSenha(login, sistema);
        }
        #endregion

        #region " _Override_Methods "
        public override string ToString()
        {
            return Nome + " - " + Login;
        }
        #endregion
    }
}