using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastrarCIDController : ControllerBase
    {
        private readonly CadastrarCIDService _service;
        public CadastrarCIDController()
        {
            _service = new Services.CadastrarCIDService();
        }


        [HttpPost]
        public async Task<IActionResult> ReceberDados([FromBody] JsonElement data)
        {
            try
            {
                if (await _service.ReceberDados(data))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
