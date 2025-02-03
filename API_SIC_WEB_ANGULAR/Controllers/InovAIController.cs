using API_SIC_WEB_ANGULAR.Model;
using API_SIC_WEB_ANGULAR.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection.Metadata;

namespace API_SIC_WEB_ANGULAR.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas a arquivos e pastas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InovAIController : ControllerBase
    {
        private readonly InovAIService _inovAIService;
        private readonly LogService _logService;

        /// <summary>
        /// Construtor da controller InovAIController.
        /// </summary>
        /// <param name="inovAIService">Serviço para gerenciamento de arquivos e pastas.</param>
        /// <param name="logService">Serviço para gerenciamento de arquivos e pastas.</param>
        public InovAIController(InovAIService inovAIService, LogService logService)
        {
            _inovAIService = inovAIService;
            _logService = logService;
        }

        /// <summary>
        /// Lista arquivos e pastas em um diretório especificado.
        /// </summary>
        /// <param name="directoryPath">O caminho do diretório a ser listado. Se nulo, o diretório padrão "C:\projetos\Simulacao_Chat" será usado.</param>
        /// <returns>Uma lista de arquivos e pastas no diretório especificado.</returns>
        /// <response code="200">Retorna a lista de arquivos e pastas.</response>
        /// <response code="401">Retorna se o usuário não tem permissão para acessar o diretório.</response>
        /// <response code="404">Retorna se o diretório não for encontrado.</response>
        /// <response code="500">Retorna se ocorrer um erro ao listar os itens.</response>
        [HttpGet("list")]
        public ActionResult ListFilesAndFolders(string? directoryPath = null)
        {
            try
            {
                var items = _inovAIService.ListFilesAndFolders(directoryPath);
                return Ok(items);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar itens: {ex.Message}");
            }
        }


        /// <summary>
        /// Cria um novo arquivo.
        /// </summary>
        /// <param name="fileName">O nome do arquivo a ser criado.</param>
        /// <param name="directoryPath">O caminho do diretório onde o arquivo será criado. Se o caminho passado for "/" será usado o caminho default.</param>
        /// <param name="content">O conteúdo do arquivo.</param>
        /// <returns>Uma mensagem indicando que o arquivo foi criado com sucesso.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso e o caminho do arquivo criado.</response>
        /// <response code="400">Retorna se o nome do arquivo ou o conteúdo estiverem vazios, ou se houver um erro de argumento.</response>
        /// <response code="409">Retorna se já existir um arquivo com o mesmo nome no diretório especificado.</response>
        /// <response code="500">Retorna se ocorrer um erro ao criar o arquivo.</response>
        [HttpPost("create-file")]
        public IActionResult CreateFile([FromQuery] string fileName, [FromQuery] string directoryPath, [FromBody] string content)
        {
            try
            {
                directoryPath = directoryPath == "/" ? "C:\\projetos\\Simulacao_Chat" : directoryPath;

                var filePath = _inovAIService.CreateFile(directoryPath, fileName, content);
                return Ok(new { Message = "Arquivo criado com sucesso.", FilePath = filePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (IOException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Erro ao criar arquivo: {ex.Message}" });
            }
        }

        /// <summary>
        /// Cria uma nova pasta.
        /// </summary>
        /// <param name="folderName">O nome da pasta a ser criada.</param>
        /// <param name="directoryPath">O caminho do diretório onde a pasta será criada. Se o caminho passado for "/" será usado o caminho default.</param>
        /// <returns>Uma mensagem indicando que a pasta foi criada com sucesso.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso e o caminho da pasta criada.</response>
        /// <response code="400">Retorna se o nome da pasta for inválido ou se houver um erro de argumento.</response>
        /// <response code="404">Retorna se o diretório pai não for encontrado.</response>
        /// <response code="409">Retorna se já existir uma pasta com o mesmo nome no diretório especificado.</response>
        /// <response code="500">Retorna se ocorrer um erro ao criar a pasta.</response>
        [HttpGet("create-folder")]
        public IActionResult CreateFolder([FromQuery] string folderName, [FromQuery] string directoryPath)
        {
            try
            {
                directoryPath = directoryPath == "/" ? "C:\\projetos\\Simulacao_Chat" : directoryPath;

                var folderPath = _inovAIService.CreateFolder(directoryPath, folderName);
                return Ok(new { Message = "Pasta criada com sucesso.", FolderPath = folderPath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (IOException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Erro ao criar pasta: {ex.Message}" });
            }
        }

        /// <summary>
        /// Edita o conteúdo de um arquivo existente ou, se newContent for nulo, retorna o conteúdo atual do arquivo.
        /// </summary>
        /// <param name="filePath">O caminho do arquivo a ser editado.</param>
        /// <param name="newContent">O novo conteúdo do arquivo. Se nulo, o conteúdo atual será retornado.</param>
        /// <returns>Uma mensagem indicando que o arquivo foi editado com sucesso ou que o conteúdo atual foi retornado.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso, o caminho do arquivo e o conteúdo atual (se newContent for nulo).</response>
        /// <response code="403">Retorna se o acesso ao arquivo for proibido.</response>
        /// <response code="404">Retorna se o arquivo não for encontrado.</response>
        /// <response code="500">Retorna se ocorrer um erro ao processar o arquivo.</response>
        [HttpPost("edit-file")]
        public IActionResult EditFile(string filePath, [FromBody] string newContent = null)
        {
            try
            {
                var currentContent = _inovAIService.ReadAndEditFile(filePath, newContent);

                return Ok(new
                {
                    Message = newContent == null
                        ? "Conteúdo do arquivo retornado com sucesso."
                        : "Arquivo editado com sucesso.",
                    FilePath = filePath,
                    CurrentContent = currentContent
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar o arquivo: {ex.Message}");
            }
        }

        /// <summary>
        /// Renomeia uma pasta existente.
        /// </summary>
        /// <param name="currentFolderPath">O caminho atual da pasta.</param>
        /// <param name="newFolderName">O novo nome da pasta.</param>
        /// <returns>Uma mensagem indicando que a pasta foi renomeada com sucesso.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso.</response>
        /// <response code="400">Retorna se o novo nome da pasta for inválido ou se houver um erro de argumento.</response>
        /// <response code="409">Retorna se já existir uma pasta com o novo nome no diretório pai.</response>
        /// <response code="500">Retorna se ocorrer um erro ao renomear a pasta.</response>
        [HttpPost("rename-folder")]
        public IActionResult RenameFolder(string currentFolderPath, string newFolderName)
        {
            try
            {
                _inovAIService.RenameFolder(currentFolderPath, newFolderName);
                return Ok("Pasta renomeada com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao renomear pasta: {ex.Message}");
            }
        }

        /// <summary>
        /// Exclui um arquivo ou pasta.
        /// </summary>
        /// <param name="path">O caminho do arquivo ou pasta a ser excluído.</param>
        /// <returns>Uma mensagem indicando que o item foi excluído com sucesso.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso e o caminho do item excluído.</response>
        /// <response code="403">Retorna se a permissão para excluir o item for negada.</response>
        /// <response code="404">Retorna se o arquivo ou pasta não for encontrado.</response>
        /// <response code="409">Retorna se ocorrer um erro ao excluir o item.</response>
        /// <response code="500">Retorna se ocorrer um erro ao excluir o item.</response>
        [HttpDelete("delete")]
        public IActionResult DeleteItem(string path)
        {
            try
            {
                _inovAIService.DeleteItem(path);
                return Ok(new { Message = "Item excluído com sucesso.", Path = path });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid($"Permissão negada para excluir o item: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (IOException ex)
            {
                return Conflict($"Erro ao excluir o item: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir o item: {ex.Message}");
            }
        }

        /// <summary>
        /// Lê o conteúdo de um arquivo.
        /// </summary>
        /// <param name="filePath">O caminho do arquivo a ser lido.</param>
        /// <returns>O conteúdo do arquivo.</returns>
        /// <response code="200">Retorna uma mensagem de sucesso e o conteúdo do arquivo.</response>
        /// <response code="403">Retorna se o acesso ao arquivo for proibido.</response>
        /// <response code="404">Retorna se o arquivo não for encontrado.</response>
        /// <response code="500">Retorna se ocorrer um erro ao ler o arquivo.</response>
        [HttpGet("read-file")]
        public IActionResult ReadFile(string filePath)
        {
            try
            {
                var content = _inovAIService.ReadFile(filePath);
                return Ok(new { Message = "Conteúdo do arquivo retornado com sucesso.", Content = content });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao ler arquivo: {ex.Message}");
            }
        }
    }
}