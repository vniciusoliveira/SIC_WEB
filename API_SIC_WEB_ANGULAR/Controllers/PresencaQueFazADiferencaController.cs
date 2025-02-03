using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Interfaces;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaQueFazADiferencaController : ControllerBase
    {

        private readonly PresencaQueFazADiferencaService _service;
        private readonly IConfiguration _configuration;

        public PresencaQueFazADiferencaController(PresencaQueFazADiferencaService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet("VigenciaPresencaQueFazADiferenca")]

        public async Task<IActionResult> BuscarVigenciaComissao()
        {
            try
            {
                var vigenciaObj = await _service.BuscarVigenciaComissao();
                return Ok(vigenciaObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpGet("GetValoresCR")]

        public async Task<IActionResult> BuscarPontosCR(string nrreg)
        {
            try
            {
                DateTime dataDeHoje = DateTime.Today;

                ConsultaPontosCrsPresencaQueFazADiferenca crPontosObj = await _service.BuscarCRsPontosService(nrreg, dataDeHoje.ToString("yyyy/MM/dd"));
                return Ok(crPontosObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("GetComissao")]

        public async Task<IActionResult> BuscarValoresComissao(ConsultaPremiacaoConsolidadaPresencaQueFazADiferencaDTO objColaborador)
        {
            try
            {
                var detalheVendasColaborador = await _service.BuscaComissaoColaborador(objColaborador);
                return Ok(detalheVendasColaborador);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetArquivoCSV")]
        public async Task<IActionResult> BuscarArquivoCSV(string vpe_codigo)
        {
            try
            {
                var resp = await _service.RetornaArquivoCSV(vpe_codigo, _configuration.GetConnectionString("DataBase"));
                var byteArray = Encoding.UTF8.GetBytes(resp);
                var stream  = new MemoryStream(byteArray);
                if(resp == null)
                {
                    return File(stream, "text/csv", "export.csv");
                }
                return Ok(resp);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
