using System.Collections.Generic;
using System.Threading.Tasks;
using App.Cohab.DataAccess;

namespace App.Cohab
{
    public class UsuarioService
    {
        private readonly UsuarioDao dao;

        public UsuarioService()
        {
            dao = new UsuarioDao();
        }

        public async Task<List<UsuarioModel>> Listar()
        {
            return await dao.Listar();
        }

        public async Task<List<UsuarioModel>> Pesquisar(UsuarioModel obj)
        {
            return await dao.Pesquisar(obj);
        }

        public async Task<UsuarioModel> Buscar(UsuarioModel obj)
        {
            return await dao.Buscar(obj);
        }

        public async Task<bool> VerificarAcesso(string login, string sistema)
        {
            return await dao.VerificarAcesso(login, sistema);
        }

        public async Task<List<UsuarioModel>> ListarPorSetor(string setor = null, bool soAtivos = true)
        {
            return await dao.ListarPorSetor(setor, soAtivos);
        }

        public async Task<List<UsuarioModel>> ListarPorDepartamento(string depto, bool exclusivo = false, bool ativos = false)
        {
            return await dao.ListarPorDepartamento(depto, exclusivo, ativos);
        }

        public async Task<List<UsuarioModel>> ListarPorMatricula(string matricula, string login = null)
        {
            return await dao.ListarPorMatricula(matricula, login);
        }

        public async Task<bool> ClonarAcessos(string loginOrigem, string loginDestino)
        {
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");

            return await dao.ClonarAcessos(loginOrigem, loginDestino, senhaPadrao);
        }

        public async Task<bool> ResetarSenha(string login, string sistema)
        {
            var senhaPadrao = Funcoes.CriptografarSenha("senhasenha");
            var validade = (await dao.DataServidor()).AddDays(30).ToShortDateString();

            return await dao.ResetarSenha(login, sistema, senhaPadrao, validade);
        }
    }
}