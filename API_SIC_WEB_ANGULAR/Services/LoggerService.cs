public class LoggerService
{
    private readonly string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs.txt");

    // Método para registrar o log
    public void LogAction(string username, string action, string path)
    {
        string logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | User: {username} | Action: {action} | Path: {path}";

        // Escrever o log no arquivo
        File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
    }
}