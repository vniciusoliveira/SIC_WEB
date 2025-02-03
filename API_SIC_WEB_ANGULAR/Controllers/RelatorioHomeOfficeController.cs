using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioHomeOfficeController : ControllerBase
    {
        private readonly RelatorioHomeOfficeDAL _relatorioDAL;
        RelatorioHomeOfficeService _relatorioHomeService = new();
        public RelatorioHomeOfficeController()
        {
            _relatorioDAL = new RelatorioHomeOfficeDAL();
        }

        [HttpGet]
        public ActionResult<List<Vigencia>> Get([FromQuery] string dataIni, string dataFim)
        {
            List<RelatorioHomeOffice> relatorios;
            try
            {
                relatorios = _relatorioDAL.GetRelatorioHomeOffices(dataIni, dataFim);
                return Ok(relatorios);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet]
        [Route("BuscaRelatorioPremiacaoHome")]
        public  ActionResult<List<RelatorioHomeOfficeAvaliativo>> GetRelatorioPremiacao([FromQuery] string movCodigo = "", string nrreg = "")
        {

            try
            {
                List<RelatorioHomeOfficeAvaliativo> relatorios =   _relatorioHomeService.ReceberDados(movCodigo,nrreg);

                if (relatorios.Count() > 0) { 
                    return Ok(relatorios);
                }
                else
                {
                    return NotFound("Não foi possível achar um relatório");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Não foi possível achar um relatório: " + ex);
            }
        }

        [HttpGet]
        [Route("RetornaQuantidadeMovCodigo")]
        public ActionResult<List<Movimento>> GetMovCodigo()
        {

            try
            {
                List<Movimento> relatorios = _relatorioHomeService.RetornaMovCodigo();

                if (relatorios.Count() > 0)
                {
                    return Ok(relatorios);
                }
                else
                {
                    return NotFound("Não foi possível achar um relatório");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Não foi possível achar um relatório: " + ex);
            }
        }
    }
}
