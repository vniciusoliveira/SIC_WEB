using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class FolderManagerService
    {
        private const string DefaultRootPath = "C:\\projetos\\Simulacao_Chat";

        public IEnumerable<dynamic> GetFolders(string directoryPath)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"O diretório especificado não foi encontrado: {directoryPath}");

            return Directory.GetDirectories(directoryPath)
                .Select(folder => new
                {
                    Type = "Folder",
                    FullPath = Path.GetFullPath(folder),
                    Name = Path.GetFileName(folder),
                    CreationDate = Directory.GetCreationTime(folder) // Data de criação da pasta
                });
        }

        public string CreateFolder(string directoryPath, string folderName)
        {
            directoryPath = GetValidDirectoryPath(directoryPath);

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"O diretório especificado não foi encontrado: {directoryPath}");

            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("O nome da pasta é obrigatório.");

            if (folderName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"O nome da pasta contém caracteres inválidos: {folderName}");

            var folderPath = Path.Combine(directoryPath, folderName);

            if (Directory.Exists(folderPath))
                throw new IOException($"A pasta já existe: {folderPath}");

            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        public void RenameFolder(string currentFolderPath, string newFolderName)
        {
            if (string.IsNullOrWhiteSpace(currentFolderPath))
                currentFolderPath = DefaultRootPath;

            if (!Directory.Exists(currentFolderPath))
                throw new DirectoryNotFoundException($"A pasta especificada não foi encontrada: {currentFolderPath}");

            if (string.IsNullOrWhiteSpace(newFolderName))
                throw new ArgumentException("O novo nome da pasta é obrigatório.");

            if (newFolderName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"O novo nome da pasta contém caracteres inválidos: {newFolderName}");

            var parentDirectory = Path.GetDirectoryName(currentFolderPath);
            var newFolderPath = Path.Combine(parentDirectory, newFolderName);

            if (Directory.Exists(newFolderPath))
                throw new IOException($"Já existe uma pasta com o nome especificado: {newFolderPath}");

            Directory.Move(currentFolderPath, newFolderPath);
        }

        public void DeleteFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"A pasta especificada não foi encontrada: {folderPath}");

            if (Directory.EnumerateFileSystemEntries(folderPath).Any())
                throw new IOException($"A pasta não está vazia e não pode ser excluída: {folderPath}");

            Directory.Delete(folderPath);
        }

        private string GetValidDirectoryPath(string directoryPath)
        {
            return string.IsNullOrWhiteSpace(directoryPath) ? DefaultRootPath : directoryPath;
        }
    }
}