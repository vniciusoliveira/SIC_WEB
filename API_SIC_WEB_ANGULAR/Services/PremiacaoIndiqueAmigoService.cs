using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Model;
using System.Text;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class PremiacaoIndiqueAmigoService
    {
        private readonly DAL.PremiacaoIndiqueAmigoDAO _comissaoDAO;

        public PremiacaoIndiqueAmigoService(PremiacaoIndiqueAmigoDAO consultaComissaoDAO)
        {
            _comissaoDAO = consultaComissaoDAO;
        }


        public async Task<string> RetornaArquivoCSV( string connStr)
        {
            if (connStr == null)
            {
                return string.Empty;
            }
            List<Dictionary<string,object>> obj = await this._comissaoDAO.BuscaArquivoBanco(connStr);
            try
            {
                return await ConverteArquivoCSV(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> ConverteArquivoCSV(List<Dictionary<string,object>> data)
        {
            if (data == null || data.Count() == 0)
            {
                return string.Empty;
            }

            var csv = new StringBuilder();

            var headers = string.Join(";", data[0].Keys);
            csv.AppendLine(headers);

            foreach(var row in data)
            {
                var line = string.Join(";", row.Values.Select(v => v?.ToString() ?? string.Empty));
                csv.AppendLine(line);
            }

            return csv.ToString();
        }
    }
}
