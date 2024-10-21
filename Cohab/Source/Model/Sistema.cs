using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Cohab
{
    public class Sistema
    {
        private static readonly SistemaDao DAO = new SistemaDao();

        public Sistema()
        {
            Sigla = Nome = string.Empty;
        }

        #region " _Propriedades "
        public string Sigla { get; set; }

        public string Nome { get; set; }
        #endregion

        public static async Task<List<Sistema>> Pesquisar(Sistema obj)
        {
            return await DAO.Pesquisar(obj);
        }

        public static async Task<List<Sistema>> ListarProprio(bool? proprio = null)
        {
            return await DAO.ListarProprio(proprio);
        }

        public static async Task<List<Sistema>> ListarPorUsuario(Usuario obj)
        {
            return await DAO.ListarPorUsuario(obj);
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}