namespace API_SIC_WEB_ANGULAR.Model
{
    public class ChatRelatorio
    {
        public string? matricula {  get; set; }
        public string? remetente { get; set; }
        public string? destinatario { get; set; }
        public string? mensagem { get; set; }
        public DateTime? dataEnvio { get; set; }
        public TimeSpan? horaEnvio { get; set; }
        public DateTime? dataLido { get; set; }
        public TimeSpan? horaLido { get; set; }
        public int remetenteFLG {  get; set; }
    }
}
