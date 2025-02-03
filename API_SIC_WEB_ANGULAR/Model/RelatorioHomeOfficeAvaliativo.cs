namespace API_SIC_WEB_ANGULAR.Model
{
    public class RelatorioHomeOfficeAvaliativo
    {
        public string? Codigo { get; set; }
        public string? Operador { get; set; }
        public string? Sup_matricula { get; set; }
        public string? Supervisor { get; set; }
        public int? Dias_Escalados { get; set; }
        public int? Dias_Trabalhados { get; set; }
        public string? Cargo { get; set; }
        public string? Produto { get; set;}
        public TimeSpan? Media_Tempo_Disponivel { get; set; }
        public TimeSpan? Media_Tempo_Disponivel_Possivel { get; set; }
        public string? Apto_A_Premiacao { get; set; }
        public double? Premiacao { get; set; }
    }
}
