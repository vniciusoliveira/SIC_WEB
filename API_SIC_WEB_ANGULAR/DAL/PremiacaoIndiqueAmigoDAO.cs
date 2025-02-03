using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.DTO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class PremiacaoIndiqueAmigoDAO
    {

        public async Task<List<Dictionary<string,object>>> BuscaArquivoBanco( string strConn)
        {
            try
            {
                var result = new List<Dictionary<string,object>>();

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "STP_ARQUIVO_CONSOLIDADO_INDIQUE_AMIGO";
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for(int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                result.Add(row);
                            }

                        }
                    }
                }
                return result;
            }
            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
