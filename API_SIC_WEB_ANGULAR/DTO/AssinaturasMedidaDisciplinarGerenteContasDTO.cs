namespace API_SIC_WEB_ANGULAR.DTO
{
    public class AssinaturasMedidaDisciplinarGerenteContasDTO
    {
        public int Mdd_Codigo { get; set; }
        public string? MddGerenteContasNrreg { get; set; }
        public string? MddNomeGerenteContas { get; set; }
        public string? MddGerenteContasCpf { get; set; }
        public DateTime? MddGerenteContasDtNasc { get; set; }
        public DateTime? MddGerenteAssinaturaDtHora { get; set; }
        public string? MddSmdCodigoGerenteContas { get; set; }
        public int MddMdrCodigo { get; set; }
        public string? MddGerenteHostNameAssinatura { get; set; }
        public string? MddGerenteIpAssinatura { get; set; }
        public string? MddGerenteGeoLocalizacaoAssinatura { get; set; }
        public bool Assinado { get; set; }

    }
}
