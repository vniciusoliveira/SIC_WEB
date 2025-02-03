using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class CadastrarCIDService
    {
        public async Task<bool> ReceberDados(JsonElement data)
        {
            Dictionary<string, string> args = new()
            {
                { "CID", data.GetProperty("CID").GetString() ?? "" },
                { "DESC",data.GetProperty("DESC").GetString() ?? "" },
                { "MATRICULA", data.GetProperty("MATRICULA").GetString() ?? "" }

            };
            DAL.CadastrarCIDDAO objDAO = new();
            return objDAO.Cadastrar(args);
        }
    }
}
