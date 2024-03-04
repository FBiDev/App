using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace App.Cohab
{
    public class Usuario
    {
        #region " _Propriedades "
        public string Login { get; set; }

        [Style(Width = 270)]
        public string Nome { get; set; }

        [Display(AutoGenerateField = false)]
        public string Apelido { get; set; }

        public string Matricula { get; set; }
        public string Ramal { get; set; }

        [Display(AutoGenerateField = false)]
        public string Email { get; set; }

        [Display(isBool = IsBool.Yes)]
        public bool Ativo { get; set; }

        [Display(AutoGenerateField = false)]
        public int TipoColaborador { get; set; }

        [Display(isBool = IsBool.Yes)]
        public bool Chefia { get; set; }

        [Display(isBool = IsBool.Yes)]
        public bool Visivel { get; set; }

        [Display(AutoGenerateField = false)]
        public string Notes { get; set; }

        static readonly UsuarioDao DAO = new UsuarioDao();
        #endregion

        #region " _Construtor "
        public Usuario()
        {
            Login = Nome = Apelido = Ramal = Matricula = string.Empty;
        }

        public override string ToString()
        {
            return Nome;
        }
        #endregion

        public static async Task<List<Usuario>> Pesquisar(Usuario obj)
        {
            if (obj == null) obj = new Usuario { };
            return await DAO.Pesquisar(obj);
        }

        public static async Task<bool> VerificarAcesso(string login, string sistema)
        {
            return await DAO.VerificarAcesso(login, sistema);
        }

        public static async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino)
        {
            return await DAO.ClonarAcessos(loginOrigem, loginDestino);
        }

        public static async Task<bool> ResetarSenha(string login, string sistema)
        {
            return await DAO.ResetarSenha(login, sistema);
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
    }
}