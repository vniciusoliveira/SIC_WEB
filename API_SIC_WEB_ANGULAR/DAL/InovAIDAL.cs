using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using API_SIC_WEB_ANGULAR.Model;

public class LogDal
{
    public readonly string _connectionString;

    public LogDal()
    {
        // Carrega as configurações do appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)  // Define o caminho base para buscar o appsettings
            .AddJsonFile("appsettings.example.json")  // Carrega o arquivo de configuração
            .Build();

        // Lê a connection string configurada
        _connectionString = configuration.GetConnectionString("SicWeb");  // Aqui, 'DataBase' é o nome da connection string
    }

    public void STP_LOG_ALTERACAO_ARQUIVOS_INOVAI(Log log)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))  // Usando a connection string configurada
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("STP_LOG_ALTERACAO_ARQUIVOS_INOVAI", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@IVA_NRREG_LOG", SqlDbType.VarChar, 10).Value = log.IVA_NRREG_LOG;
                    //command.Parameters.Add("@IVA_DATAHORA_LOG", SqlDbType.DateTime).Value = log.IVA_DATAHORA_LOG;
                    command.Parameters.Add("@IVA_CONTEUDO_ANTERIOR", SqlDbType.VarChar, -1).Value = log.IVA_CONTEUDO_ANTERIOR;
                    command.Parameters.Add("@IVA_NOMEARQUIVO", SqlDbType.VarChar, 100).Value = log.IVA_NOMEARQUIVO;
                    command.Parameters.Add("@IVA_CAMINHO", SqlDbType.VarChar, 500).Value = log.IVA_CAMINHO;

                    // Executa a stored procedure e verifica o retorno
                    var result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Log inserido com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro foi inserido.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log mais detalhado do erro
                Console.WriteLine("Erro ao inserir log no banco: " + ex.Message);
                throw;  // Propaga o erro com a stack trace
            }
        }
    }
}
