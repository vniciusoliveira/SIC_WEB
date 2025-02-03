using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Model;
using System.Text;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class PresencaQueFazADiferencaService
    {
        private readonly DAL.PresencaQueFazADiferencaDAL _comissaoDAL;

        public PresencaQueFazADiferencaService(PresencaQueFazADiferencaDAL consultaComissaoDAL)
        {
            _comissaoDAL = consultaComissaoDAL;
        }

        public async Task<PresencaQueFazADiferencaDTO> BuscarVigenciaComissao()
        {
            PresencaQueFazADiferencaDTO vigenciaComissao = await _comissaoDAL.consultaVigenciaBanco();
            return vigenciaComissao;
        }

        public async Task<ConsultaPontosCrsPresencaQueFazADiferenca> BuscarCRsPontosService(string nrreg, string dtHora)
        {
            ConsultaPontosCrsPresencaQueFazADiferenca vigenciaComissao = await _comissaoDAL.ConsultaPontosCRsDAL(nrreg, dtHora);
            return vigenciaComissao;
        }

        public async Task<List<PresencaQueFazADiferencaDTO>> BuscaComissaoColaborador(ConsultaPremiacaoConsolidadaPresencaQueFazADiferencaDTO objColaborador)
        {
            // Busca informações do colaborador
            List<PresencaQueFazADiferencaDTO> comissaoInfos = await _comissaoDAL.consultaComissaoBanco(objColaborador);
            return comissaoInfos;
        }

        public async Task<string> RetornaArquivoCSV(string vpe_codigo, string connStr)
        {
            if (vpe_codigo == null || connStr == null)
            {
                return string.Empty;
            }
            List<Dictionary<string,object>> obj = await this._comissaoDAL.BuscaArquivoBanco(vpe_codigo, connStr);
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
