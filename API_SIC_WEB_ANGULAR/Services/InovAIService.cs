using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using API_SIC_WEB_ANGULAR.Model;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class InovAIService
    {
        private const string DefaultRootPath = "C:\\projetos\\Simulacao_Chat";
        private readonly List<string> _ignoredFiles;
        private readonly LogService _logService; // Injeção de dependência

        public InovAIService(LogService logService)
        {
            _ignoredFiles = new List<string> { "*.exe" }; // Arquivos padrão ignorados
            _logService = logService;
        }

        // ================== MANIPULAÇÃO DE ARQUIVOS ==================

        public bool IsIgnored(string fileName) => _ignoredFiles.Contains(fileName);

        public IEnumerable<InovAI> ListFilesAndFolders(string directoryPath)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            // Impedir que o usuário navegue para além da pasta inicial
            string baseDirectory = DefaultRootPath; // Diretório raiz permitido
            string fullPath = Path.GetFullPath(directoryPath);
            string basePath = Path.GetFullPath(baseDirectory);

            // Verifica se o caminho solicitado está dentro do diretório permitido
            if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Acesso negado. Você não tem permissão para acessar esse diretório.");
            }

            // Validar se o diretório existe
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"O diretório não existe: {directoryPath}");
            }

            // Obter arquivos .txt e diretórios
            string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt", SearchOption.TopDirectoryOnly);
            string[] folders = Directory.EnumerateFileSystemEntries(directoryPath, "*", SearchOption.TopDirectoryOnly)
                .Where(path => Directory.Exists(path)) // Filtra apenas diretórios
                .ToArray();

            // Preparar os resultados usando a classe InovAI
            List<InovAI> folderItems = folders
                .Select(folder => new InovAI
                {
                    Name = Path.GetFileName(folder),
                    Path = folder,
                    Type = "Folder",
                    CreationTime = Directory.GetCreationTime(folder),
                    LastWriteTime = Directory.GetLastWriteTime(folder),
                    Content = null // Pastas não têm conteúdo
                })
                .ToList();

            List<InovAI> fileItems = txtFiles
                .Select(file => new InovAI
                {
                    Name = Path.GetFileName(file),
                    Path = file,
                    Type = "File",
                    CreationTime = System.IO.File.GetCreationTime(file),
                    LastWriteTime = System.IO.File.GetLastWriteTime(file),
                    Content = System.IO.File.ReadAllText(file) // Ler conteúdo do arquivo
                })
                .ToList();

            // Combinar os resultados de pastas e arquivos
            return folderItems.Concat(fileItems).ToList();
        }

        public string CreateFile(string directoryPath, string fileName, string content)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("O conteúdo do arquivo não pode ser vazio.");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("O nome do arquivo é obrigatório.");
            }

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Diretório não encontrado: {directoryPath}");
            }

            ValidateFileName(fileName);

            if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                fileName += ".txt";

            var filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
                throw new IOException($"O arquivo já existe: {filePath}");

            File.WriteAllText(filePath, content ?? string.Empty);
            return filePath;
        }

        public string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Leitura não permitida para este arquivo: {filePath}");

            return File.ReadAllText(filePath);
        }

        public string ReadAndEditFile(string filePath, string newContent = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Operação não permitida para este arquivo: {filePath}");

            string currentContent = null;

            if (newContent != null)
            {
                currentContent = File.ReadAllText(filePath); // Ler o conteúdo atual antes de editar
                File.WriteAllText(filePath, newContent);
            }
            else
            {
                currentContent = File.ReadAllText(filePath);
            }

            // Registrar o log APÓS a edição bem-sucedida
            if (newContent != null)
            {
                try
                {
                    Log log = new Log
                    {
                        IVA_NRREG_LOG = "893526", // Substitua pela matrícula do usuário logado
                        IVA_DATAHORA_LOG = DateTime.Now,
                        IVA_CONTEUDO_ANTERIOR = currentContent,
                        IVA_NOMEARQUIVO = Path.GetFileName(filePath),
                        IVA_CAMINHO = filePath
                    };
                    _logService.RegistrarLog(log);
                }
                catch (Exception ex)
                {
                    // Trate a exceção de acordo com as necessidades da sua aplicação.
                    // Você pode registrar o erro em um log de aplicação, por exemplo.
                    Console.WriteLine($"Erro ao registrar log: {ex.Message}");
                }
            }

            return newContent ?? currentContent;
        }

        public void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Exclusão não permitida para este arquivo: {filePath}");

            File.Delete(filePath);
        }

        // ================== MANIPULAÇÃO DE PASTAS ==================

        public string CreateFolder(string directoryPath, string folderName)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"O diretório especificado não foi encontrado: {directoryPath}");

            ValidateFileName(folderName);

            var folderPath = Path.Combine(directoryPath, folderName);
            if (Directory.Exists(folderPath))
                throw new IOException($"A pasta já existe: {folderPath}");

            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        public void RenameFolder(string currentFolderPath, string newFolderName)
        {
            if (!Directory.Exists(currentFolderPath))
                throw new DirectoryNotFoundException($"A pasta especificada não foi encontrada: {currentFolderPath}");

            ValidateFileName(newFolderName);

            var parentDirectory = Path.GetDirectoryName(currentFolderPath);
            var newFolderPath = Path.Combine(parentDirectory, newFolderName);

            if (Directory.Exists(newFolderPath))
                throw new IOException($"Já existe uma pasta com o nome especificado: {newFolderPath}");

            Directory.Move(currentFolderPath, newFolderPath);
        }

        public void DeleteItem(string path)
        {
            // Verifica se o caminho fornecido é um arquivo
            if (System.IO.File.Exists(path))
            {
                // Exclui o arquivo
                System.IO.File.Delete(path);
            }
            // Verifica se o caminho fornecido é uma pasta
            else if (Directory.Exists(path))
            {
                // Exclui a pasta e todo o seu conteúdo
                Directory.Delete(path, recursive: true);
            }
            else
            {
                // Caso o caminho não seja nem arquivo nem pasta
                throw new FileNotFoundException("O caminho especificado não corresponde a um arquivo ou pasta.");
            }
        }

        // ================== MÉTODOS AUXILIARES ==================

        private string GetValidDirectoryPath(string directoryPath)
        {
            return string.IsNullOrWhiteSpace(directoryPath) ? DefaultRootPath : directoryPath;
        }

        private void ValidateFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome é obrigatório.");

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"O nome contém caracteres inválidos: {name}");
        }
    }
}