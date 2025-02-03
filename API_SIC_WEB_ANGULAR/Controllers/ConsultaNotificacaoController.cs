using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using API_SIC_WEB_ANGULAR.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;


namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaNotificacaoController : ControllerBase
    {
        private readonly ConsultaNotificacaoService _service;
        private readonly ExellUtility _excellUtility;
        public ConsultaNotificacaoController()
        {
            _service = new();
            _excellUtility = new();
        }



        [HttpPost]
        public async Task<ActionResult> ReceberDados([FromBody] JsonElement data)
        {
            try
            {
                List<ConsultaNotificacao> lstNotif = await _service.ReceberDados(data);
                if(lstNotif.Count > 0)
                {
                    List<object> listaObjetos = lstNotif.Cast<object>().ToList();
                    var excelBytes = await _excellUtility.CriarExcelAsync(listaObjetos);
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dados.xlsx");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
