using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.Model;
using Microsoft.AspNetCore.Mvc;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VigenciaController : ControllerBase
    {
        private readonly VigenciaDAO _vigenciaDAL;
        private readonly DbUtility _dbUtility;
        public VigenciaController()
        {
            _dbUtility = new DbUtility();
            _vigenciaDAL = new VigenciaDAO(_dbUtility.StrConnSICWEB());
        }

        [HttpGet]
        public ActionResult<List<Vigencia>> BuscarVigencias()

        {
            List<Vigencia> vigencias;
            try
            {
                vigencias = _vigenciaDAL.getVigencia();
                return Ok(vigencias);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
