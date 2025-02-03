namespace API_SIC_WEB_ANGULAR.Model
{
    public class ConsultaNotificacao
    {
        public string? Mop_nrreg {  get; set; }
        public string? Mop_link { get; set; }
        public string? Mop_mensagem { get; set; }
        public bool Mop_lida { get; set; }
        public DateTime Mop_datahora_lida { get; set; }
        public DateTime Mop_datahora_insert {  get; set; }
    }
}
