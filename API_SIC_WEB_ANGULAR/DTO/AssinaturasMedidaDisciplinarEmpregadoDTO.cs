namespace API_SIC_WEB_ANGULAR.DTO
{
    public class AssinaturasMedidaDisciplinarEmpregadoDTO
    {
        public string? Mdd_Codigo { get; set; } = null;
        public string? MddEmpregadoNrreg { get; set; }
        public string? MddNomeEmpregado { get; set; }
        public string? MddEmpregadoCPF { get; set; }
        public DateTime? MddEmpregadoDtNasc { get; set; }
        public DateTime? MddEmpregadoAssinaturaDtHora { get; set; } = null;
        public string? MddEmpregadoSmdCodigo { get; set; }
        public int? MddMdrCodigo { get; set; }
        public bool? Assinado { get; set; }
        public string? MddEmpregadoHostNameAssinatura { get; set; }
        public string? MddEmpregadoIpAssinatura { get; set; }
        public string? MddEmpregadoGeoLocalizacaoAssinatura { get; set; }
    }
}
