using API_SIC_WEB_ANGULAR.Model;

public class LogService
{
    private readonly LogDal _logDal;

    // Construtor para injeção de dependência da LogDal
    public LogService()
    {
        _logDal = new LogDal(); // Instanciando a DAL
    }

    // Método para registrar o log no banco de dados
    public void RegistrarLog(Log log)
    {
        // Aqui você pode adicionar qualquer lógica adicional, se necessário
        // Por exemplo, verificar se o log tem valores válidos antes de salvar

        if (log == null)
        {
            throw new ArgumentNullException(nameof(log), "O log não pode ser nulo");
        }

        if (string.IsNullOrEmpty(log.IVA_NRREG_LOG))
        {
            throw new ArgumentException("O número de matrícula do usuário é obrigatório", nameof(log.IVA_NRREG_LOG));
        }

        if (log.IVA_DATAHORA_LOG == default)
        {
            throw new ArgumentException("A data e hora do log são obrigatórios", nameof(log.IVA_DATAHORA_LOG));
        }

        // Chama a DAL para registrar o log no banco
        _logDal.STP_LOG_ALTERACAO_ARQUIVOS_INOVAI(log);
    }
}
