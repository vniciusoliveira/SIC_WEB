using API_SIC_WEB_ANGULAR.Model;
using Microsoft.AspNetCore.Mvc;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class PermissaoDeAcessoEloginAntigoService
    {
        private readonly DAL.PermissaoDeAcessoEloginAntigo.GetInfoColaboradoresDAL _dao;

        public PermissaoDeAcessoEloginAntigoService(DAL.PermissaoDeAcessoEloginAntigo.GetInfoColaboradoresDAL dao)
        {
            _dao = dao;
        }

        public async Task<ColaboradorInfos> EnviaMatriculaColaborador(string matricula)
        {
            ColaboradorInfos colaboradores = await _dao.getInfoColaborador(matricula);
            return colaboradores;
        }

        public async Task<bool> ConfirmaDesbloqueioOperador(ColaboradorInfos objOperador)
        {
            if (await _dao.InsereDesbloqueioOperadorDAO(objOperador))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
