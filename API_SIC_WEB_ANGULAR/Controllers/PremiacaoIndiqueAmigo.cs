using API_SIC_WEB_ANGULAR.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiacaoIndiqueAmigo : ControllerBase
    {
        private readonly PremiacaoIndiqueAmigoService _service;
        private readonly IConfiguration _configuration;

        public PremiacaoIndiqueAmigo(PremiacaoIndiqueAmigoService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarArquivoCSVIndiqueAmigo()
        {
            try
            {
                var resp = await _service.RetornaArquivoCSV(_configuration.GetConnectionString("DataBase"));
                var byteArray = Encoding.UTF8.GetBytes(resp);
                var stream = new MemoryStream(byteArray);
                if (resp != null)
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
