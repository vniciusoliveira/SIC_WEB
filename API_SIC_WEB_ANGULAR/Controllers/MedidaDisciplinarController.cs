using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_SIC_WEB_ANGULAR.Services;
using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Interfaces;
using System.Net.NetworkInformation;
using System.Net;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedidaDisciplinarController : ControllerBase
    {
        private readonly GetInfoMaquinaColaborador _maquina;
        private readonly MedidaDisciplinarService _service;
        private readonly IpInterface _ipInterface;

        public MedidaDisciplinarController(GetInfoMaquinaColaborador maquina, MedidaDisciplinarService service, IpInterface ipInterface)
        {
            _maquina = maquina;
            _service = service;
            _ipInterface = ipInterface;
        }

        [HttpGet("GetMedidaDisciplinares")]
        /*Essa função é responsável por buscar qual é o tipo de acesso que o colaborador tem de visualização de medidas disciplinares.*/
        public async Task<IActionResult> BuscarTipoAprovacaoMedidaDisciplinar(string matricula)
        {
            try
            {
                // Buscamos a informação do colaborador.
                var colaborador = await _service.BuscaDadosColaboradorMedidaDisciplinar(matricula);
                // Pegamos o MtaCodigo que é responsável por elaborar a lógica de quem tem direito a ver as medidas disciplinares. 
                var mtaCodigo = await _service.CalcularMtaCodigo(colaborador);

                var resultado = new
                {
                    MtaCodigo = mtaCodigo,
                    colaborador.Matricula,
                    colaborador.Nome,
                    colaborador.CgoDescricao,
                    colaborador.CgoCodigo,
                };

                //Buscamos as medidas disciplinares pendentes com base na matrícula e MtaCodigo.
                var medidasDisciplinares = await _service.BuscarMedidasDisciplinaresPendentes(resultado.MtaCodigo, resultado.Matricula);

                return Ok(medidasDisciplinares);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("GetInformationsEmployee")]

        public async Task<IActionResult> GetInformacaoMedidaDisciplinarFuncionario([FromBody] MedidaDisciplinarDTO medidaDTO)
        {
            try
            {
               var detalheMedidaDisciplinar = await _service.GetMedidaDisciplinarDetalhes(medidaDTO);
               return Ok(detalheMedidaDisciplinar);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("ValidarGerente")]

        public async Task<IActionResult> ValidarGerente([FromBody] ColaboradorInfosDTO objColaborador)
        {
            try
            {
                var infoGerente = await _service.GetDadosGerente(objColaborador);
                return Ok(infoGerente);
            }
            catch(Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("ValidarEmpregado")]

        public async Task<IActionResult> ValidarEmpregado([FromBody] ColaboradorInfosDTO objColaboradorEmpregado)
        {
            try
            {
                var infoOperador = await _service.GetDadosOperador(objColaboradorEmpregado);
                return Ok(infoOperador);
            }
            catch(Exception ex)                                 
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("ValidarTestemunha")]

        public async Task<IActionResult> ValidarTestemunha([FromBody] ColaboradorInfosDTO objColaboradorTestemunha)
        {
            try
            {
                var infoOperador = await _service.GetDadosTestemunha(objColaboradorTestemunha);
                return Ok(infoOperador);
            }
            catch(Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores {ex.Message}");
            }
        }

        [HttpPost("ConfirmaAssinaturaEmpregado")]

        public async Task<IActionResult> ConfirmaAssinaturaEmpregado([FromBody] AssinaturasMedidaDisciplinarEmpregadoDTO objAssinaturaEmpregado)
        {
            try
            {
                _maquina.ip = _ipInterface.GetIp(HttpContext);
                _maquina.hostname = GetClientHostname(_maquina.ip);
                _maquina.geoLocalizacao = objAssinaturaEmpregado.MddEmpregadoGeoLocalizacaoAssinatura;

                var getAssinaturaEmpregado = await _service.ConfirmaAssinaturaOperadorService(objAssinaturaEmpregado, _maquina);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores. {ex.Message}");
            }
        }

        [HttpPost("ConfirmaAssinaturaGerenteContas")]
        public async Task<IActionResult> ConfirmaAssinaturaGerenteContas([FromBody] AssinaturasMedidaDisciplinarGerenteContasDTO objAssinatura)
        {
            try
            {
                
                _maquina.ip = _ipInterface.GetIp(HttpContext);
                _maquina.hostname = GetClientHostname(_maquina.ip);
                _maquina.geoLocalizacao = objAssinatura.MddGerenteGeoLocalizacaoAssinatura;       

                var getAssinaturasGerente = await _service.ConfirmaAssinaturaGerenteContasService(objAssinatura, _maquina);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores. {ex.Message}");
            } 
        }

        [HttpPost("ConfirmaAssinaturaTestemunha")]
        public async Task<IActionResult> ConfirmaAssinaturaTestemunha([FromBody] AssinaturaMedidaDisciplinarTestemunhaDTO objAssinaturaTestemunha)
        {
            try
            {
                _maquina.ip = _ipInterface.GetIp(HttpContext);
                _maquina.hostname = GetClientHostname(_maquina.ip);
                _maquina.geoLocalizacao = objAssinaturaTestemunha.MddTestemunhaGeoLocalizacaoAssinatura;

                var getAssinaturasGerente = await _service.ConfirmaAssinaturaTestemunhaService(objAssinaturaTestemunha, _maquina);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar os dados da API. Consulte os administradores. {ex.Message}");
            }
        }


        [HttpGet("BuscaAssinaturaConfirmadaGerente")]
        public async Task<IActionResult> BuscaAssinaturaConfirmada(int mdrCodigo)
        {
            try
            {
                var getAssinaturasConfirmadas = await _service.BuscaAssinaturaAssinadaMedidaDisciplinarService(mdrCodigo);
                return Ok(getAssinaturasConfirmadas);
            }
            catch(Exception ex)
            {
               return BadRequest($"Erro na busca por Assinaturas Confirmadas: {ex.Message}");
            }
        }

        [HttpGet("BuscaAssinaturaConfirmadaEmpregado")]
        public async Task<IActionResult> BuscaAssinaturaConfirmadaEmpregado(int mdrCodigo)
        {
            try
            {
                var getAssinaturasConfirmadas = await _service.BuscaAssinaturaAssinadaMedidaDisciplinarEmpregadoService(mdrCodigo);
                return Ok(getAssinaturasConfirmadas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na busca por Assinaturas Confirmadas: {ex.Message}");
            }
        }

        [HttpGet("BuscaAssinaturaConfirmadaTestemunha")]
        public async Task<IActionResult> BuscaAssinaturaConfirmadaTestemunha(int mdrCodigo)
        {
            try
            {
                var getAssinaturasConfirmadas = await _service.BuscaAssinaturaAssinadaMedidaDisciplinarTestemunhaService(mdrCodigo);
                return Ok(getAssinaturasConfirmadas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na busca por Assinaturas Confirmadas: {ex.Message}");
            }
        }

        private static string GetClientHostname(string ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry?.HostName ?? "Nome de máquina não disponível";
            }
            catch (Exception)
            {
                return "Nome de máquina não disponível";
            }
        }   
    }
}
