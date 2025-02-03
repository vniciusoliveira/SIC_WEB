using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using API_SIC_WEB_ANGULAR.Utility;
using Microsoft.AspNetCore.Mvc;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuridicoController : ControllerBase
    {
        private readonly ConsultaNotificacaoService _service = new();
        private readonly ExellUtility _excellUtility = new();
        private readonly IConfiguration _configuration;

        public JuridicoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetRelatorioChat")]
        public async Task<ActionResult> GetRelatorioChat(string matricula, string matriculaLog, string? destinatario = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                List<ChatRelatorio> resp = await _service.ReceberRelatorioChat(matricula, matriculaLog, this._configuration.GetConnectionString("EPONTO"), destinatario,  dataInicio, dataFim);
                if (resp.Count > 0)
                {
                    return Ok(resp);
                }
                else
                {
                    return NoContent();
                }
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRelatorioChatArquivo")]
        public async Task<ActionResult> GetRelatorioChatArquivo(string matricula, string matriculaLog, string? destinatario = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            { 
                List<ChatRelatorio> resp = await _service.ReceberRelatorioChat(matricula, matriculaLog, this._configuration.GetConnectionString("EPONTO"), destinatario, dataInicio, dataFim);
                if (resp.Count > 0)
                {
                    List<object> listaObjetos = resp.Cast<object>().ToList();
                    var excelBytes = await _excellUtility.CriarExcelAsync(listaObjetos);
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorioChat_"+DateTime.Now.ToString("yyyy=MM-dd-HH-mm")+".xlsx");

                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
