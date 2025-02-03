using System.Data.SqlClient;

namespace API_SIC_WEB_ANGULAR.Utils // Defina o namespace CORRETO aqui!
{
    public class ConnectionFactory
    {
        public static SqlConnection GetConnection()
        {
            // *** SUBSTITUA PELA SUA STRING DE CONEXÃO ***
            string connectionString = "Data Source=seu_servidor;Initial Catalog=seu_banco;User ID=seu_usuario;Password=sua_senha;";

            // ** MELHOR PRÁTICA: Armazenar a string de conexão em um arquivo de configuração (appsettings.json) ou variável de ambiente **

            return new SqlConnection(connectionString);
        }
    }
}