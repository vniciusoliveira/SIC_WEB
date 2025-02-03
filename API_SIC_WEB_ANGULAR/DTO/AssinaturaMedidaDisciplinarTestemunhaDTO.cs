namespace API_SIC_WEB_ANGULAR.DTO
{
    public class AssinaturaMedidaDisciplinarTestemunhaDTO
    {
        public string? Mdd_Codigo { get; set; } = null;
        public string? MddTestemunhaNrreg { get; set; }
        public string? MddNomeTestemunha { get; set; }
        public string? MddTestemunhaCPF { get; set; }
        public DateTime? MddTestemunhaDtNasc { get; set; }
        public DateTime? MddTestemunhaAssinaturaDtHora { get; set; } = null;
        public string? MddTestemunhaSmdCodigo { get; set; }
        public int? MddMdrCodigo { get; set; }
        public string? MddTestemunhaHostNameAssinatura { get; set; }
        public string? MddTestemunhaIpAssinatura { get; set; }
        public string? MddTestemunhaGeoLocalizacaoAssinatura { get; set; }
        public bool? Assinado { get; set; }

    }
}
