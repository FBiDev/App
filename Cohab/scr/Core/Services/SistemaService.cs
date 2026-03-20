using System.Collections.Generic;
using System.Threading.Tasks;
using App.Cohab.DataAccess;

namespace App.Cohab
{
    public class SistemaService
    {
        private readonly SistemaDao dao;

        public SistemaService()
        {
            dao = new SistemaDao();
        }

        public async Task<List<SistemaModel>> Listar()
        {
            return await dao.Listar();
        }

        public async Task<List<SistemaModel>> Pesquisar(SistemaModel obj)
        {
            return await dao.Pesquisar(obj);
        }

        public async Task<SistemaModel> Buscar(SistemaModel obj)
        {
            return await dao.Buscar(obj);
        }

        public async Task<List<SistemaModel>> ListarProprio(bool? proprio = null)
        {
            return await dao.ListarProprio(proprio);
        }

        public async Task<List<SistemaModel>> ListarPorUsuario(UsuarioModel obj)
        {
            return await dao.ListarPorUsuario(obj);
        }
    }
}