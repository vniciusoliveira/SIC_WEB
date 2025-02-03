using Microsoft.AspNetCore.Mvc;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetInformacoesColaborador : ControllerBase
    {
        private readonly PermissaoDeAcessoEloginAntigoService _service;

        public GetInformacoesColaborador(PermissaoDeAcessoEloginAntigoService service)
        {
            _service = service;
        }

        [HttpGet("GetInfos")]

        public async Task<ColaboradorInfos> GetInfosColaborador(string matricula)
        {
            ColaboradorInfos colaboradores = await _service.EnviaMatriculaColaborador(matricula);
            return colaboradores;
        }

        [HttpPost("DesbloqueioOperador")]
        public async Task<IActionResult> DesbloquearOperador([FromBody] ColaboradorInfos desbloqueioOperador)
        {
            if (await _service.ConfirmaDesbloqueioOperador(desbloqueioOperador))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
