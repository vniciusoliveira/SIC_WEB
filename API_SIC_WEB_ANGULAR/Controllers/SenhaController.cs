using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;

using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Services;
using System.Runtime.CompilerServices;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    public class SenhaController : ControllerBase
    {

        private readonly SenhaService _senhaService;
        private readonly IConfiguration _configuration;

        public SenhaController(SenhaService senhaService, IConfiguration configuration)
        {
            _senhaService = senhaService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("ColaboradorSenha")]
        public async Task<ActionResult<dynamic?>> getDadosColaborador([FromBody] TrocaSenhaDTO obj)
        {
            try
            {
                dynamic resp = await _senhaService.setNovaSenha(obj, _configuration.GetConnectionString("DataBase"));
                if(resp == null)
                {
                    return NotFound(resp);
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ColaboradorPerguntas")]
        public async Task<ActionResult<List<dynamic>>> getDadosColaborador([FromBody] Dictionary<string, object> ope)
        {
            try
            {
                dynamic resp = await _senhaService.getPerguntas(ope["matricula"].ToString(), _configuration.GetConnectionString("DataBase"));
                if (resp == null)
                {
                    return NotFound(resp);
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ColaboradorSenhaPergunta")]
        public async Task<ActionResult<dynamic?>> setSenhaPergunta([FromBody] TrocaSenhaPerguntaDTO obj)
        {
            try
            {
                dynamic resp = await _senhaService.setNovaSenhaPergunta(obj, _configuration.GetConnectionString("DataBase")) ;
                if (resp == null)
                {
                    return NotFound(resp);
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
