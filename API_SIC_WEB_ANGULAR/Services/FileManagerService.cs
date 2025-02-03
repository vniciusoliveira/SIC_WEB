using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class FileManagerService
    {
        private const string DefaultRootPath = "C:\\projetos\\Simulacao_Chat";
        private readonly List<string> _ignoredFiles;

        public FileManagerService()
        {
            _ignoredFiles = new List<string> {"*.exe"}; // Arquivos padrão ignorados
        }

        public bool IsIgnored(string fileName) => _ignoredFiles.Contains(fileName);

        public IEnumerable<dynamic> GetFiles(string directoryPath)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Diretório não encontrado: {directoryPath}");

            return Directory.GetFiles(directoryPath)
                .Where(file => !IsIgnored(Path.GetFileName(file)))
                .Select(file => new
                {
                    Type = "File",
                    FullPath = Path.GetFullPath(file),
                    Name = Path.GetFileName(file),
                    Size = new FileInfo(file).Length // Tamanho do arquivo em bytes
                });
        }

        public string CreateFile(string directoryPath, string fileName, string content)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Diretório não encontrado: {directoryPath}");

            ValidateFileName(fileName);

            if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                fileName += ".txt";

            var filePath = Path.Combine(directoryPath, fileName);

            if (System.IO.File.Exists(filePath))
                throw new IOException($"O arquivo já existe: {filePath}");

            System.IO.File.WriteAllText(filePath, content ?? string.Empty);

            return filePath;
        }

        public string ReadFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Leitura não permitida para este arquivo: {filePath}");

            return System.IO.File.ReadAllText(filePath);
        }

        public string ReadAndEditFile(string filePath, string newContent = null)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Operação não permitida para este arquivo: {filePath}");

            if (newContent == null)
            {
                // Apenas leitura
                return System.IO.File.ReadAllText(filePath);
            }

            // Edição do conteúdo
            System.IO.File.WriteAllText(filePath, newContent);
            return newContent;
        }

        public void DeleteFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            if (IsIgnored(Path.GetFileName(filePath)))
                throw new UnauthorizedAccessException($"Exclusão não permitida para este arquivo: {filePath}");

            System.IO.File.Delete(filePath);
        }

        private string GetValidDirectoryPath(string directoryPath)
        {
            return string.IsNullOrWhiteSpace(directoryPath) ? DefaultRootPath : directoryPath;
        }

        private void ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("O nome do arquivo é obrigatório.");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"O nome do arquivo contém caracteres inválidos: {fileName}");
        }
    }
}