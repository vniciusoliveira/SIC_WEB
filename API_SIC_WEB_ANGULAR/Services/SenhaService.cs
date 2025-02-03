using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DTO;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class SenhaService
    {

        private readonly SenhaDAO _senhaDAO;

        public SenhaService(SenhaDAO senhaDAO) 
        { 
            this._senhaDAO = senhaDAO;
        }

        public async Task<dynamic?> setNovaSenha(TrocaSenhaDTO obj, string connStr)
        {
            if (obj.senhaVelha != "")
            {
                return await this._senhaDAO.setSenhaColaborador(obj, connStr);
            }
            return null;
        }

        public async Task<List<dynamic>?> getPerguntas(string matricula, string connStr)
        {
            if (matricula != "")
            {
                return await this._senhaDAO.getPerguntasColaborador(matricula, connStr);
            }
            return null;
        }

        public async Task<dynamic?> setNovaSenhaPergunta(TrocaSenhaPerguntaDTO obj, string connStr)
        {
            if (obj.senhaNova != "")
            {
                return await this._senhaDAO.setSenhaColaboradorComPergunta(obj, connStr);
            }
            return null;
        }
    }
}
