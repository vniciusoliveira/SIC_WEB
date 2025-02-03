using API_SIC_WEB_ANGULAR.Model;
using System.Drawing;
using System.Text.Json;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class ConsultaNotificacaoService
    {
        public async Task<List<ConsultaNotificacao>> ReceberDados(JsonElement data)
        {
            Dictionary<string, string> args = new()
            {
                { "NRREG", data.GetProperty("NRREG").GetString() ?? "" },
                { "DTINI",data.GetProperty("DTINI").GetString() ?? "" },
                { "DTEND", data.GetProperty("DTEND").GetString() ?? "" }

            };
            DAL.ConsultaNotificacaoDAO objDAO = new();
            return objDAO.ConsultarNotificacao(args);
        }

        public async Task<List<ChatRelatorio>> ReceberRelatorioChat(string matricula, string matriculaLog, string strConn, string? destinatario = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            if (matricula=="" || matriculaLog=="")
            {
                throw new Exception("Sem matricula de busca ou matricula de log.");
            }

            DAL.ConsultaNotificacaoDAO objDAO = new();
            List<ChatRelatorio> list = await objDAO.GetRelatorioChat(matricula,matriculaLog, strConn, destinatario, dataInicio, dataFim);
            return list;
        }
    }
}
