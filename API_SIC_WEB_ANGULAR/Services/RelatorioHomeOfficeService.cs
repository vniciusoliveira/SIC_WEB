using API_SIC_WEB_ANGULAR.Model;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class RelatorioHomeOfficeService
    {
        public List<RelatorioHomeOfficeAvaliativo> ReceberDados(string movCodigo, string nrreg)
        {
            Dictionary<string, string> args = new()
            {
                { "MOVIMENTO", movCodigo ?? "" },
                { "NRREG_SEARCH", nrreg ?? ""},
            };
            DAL.RelatorioHomeOfficeDAL objDAO = new();
            return  objDAO.GetRelatorioHomeOfficeOpAvaliativo(args);
        }

        public List<Movimento> RetornaMovCodigo()
        {
            DAL.RelatorioHomeOfficeDAL objDAO = new();
            return objDAO.GetMovCodigoAtivo();
        }
    }
}
