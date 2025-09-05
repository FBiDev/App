using System.Collections.Generic;
using System.Threading.Tasks;
using App.Cohab.Dao;

namespace App.Cohab
{
    public class Sistema
    {
        #region " _Fields "
        private static readonly SistemaDao DAO = new SistemaDao();
        #endregion

        #region " _Constructor "
        public Sistema()
        {
            Sigla = Nome = string.Empty;
        }
        #endregion

        #region " _Properties "
        public string Sigla { get; set; }

        public string Nome { get; set; }
        #endregion

        #region " _Static_Methods "
        public static async Task<List<Sistema>> Listar()
        {
            return await DAO.Listar();
        }

        public static async Task<List<Sistema>> Pesquisar(Sistema obj)
        {
            return await DAO.Pesquisar(obj);
        }

        public static async Task<Sistema> Buscar(Sistema obj)
        {
            return await DAO.Buscar(obj);
        }

        public static async Task<List<Sistema>> ListarProprio(bool? proprio = null)
        {
            return await DAO.ListarProprio(proprio);
        }

        public static async Task<List<Sistema>> ListarPorUsuario(Usuario obj)
        {
            return await DAO.ListarPorUsuario(obj);
        }
        #endregion

        #region " _Override_Methods "
        public override string ToString()
        {
            return Nome + " - " + Sigla;
        }
        #endregion
    }
}