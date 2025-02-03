using API_SIC_WEB_ANGULAR.DTO;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class SenhaDAO
    {
        public async Task<dynamic?> setSenhaColaborador(TrocaSenhaDTO obj, string connStr)
        {
			try
			{


				using (SqlConnection conn = new SqlConnection(connStr))
				{
					await conn.OpenAsync();

					using (SqlCommand cmd = new SqlCommand("STP_GRAVA_SENHA_COM_CRIPTOGRAFIA_ALTERAR_SENHA_NOVO"))
					{
						cmd.Connection = conn;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@AGTCODIGO", obj.matricula);
						cmd.Parameters.AddWithValue("@SENHAATU", obj.senhaVelha);
						cmd.Parameters.AddWithValue("@SENHANOV", obj.senhaNova);
						cmd.Parameters.AddWithValue("@PES_CODIGO", 0);
						cmd.Parameters.AddWithValue("@HOSTNAME", "EXTERNO");
						cmd.Parameters.AddWithValue("@IPADDRESS", "EXTERNO");
						using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
						{
							var result = new ExpandoObject() as IDictionary<string, object>;
							while (await reader.ReadAsync())
							{
								result.Add("Status", reader["STATUS_MENSAGEM"]  == DBNull.Value ? "" : reader.GetString("STATUS_MENSAGEM"));
								result.Add("Mensagem", reader["MENSAGEM"]  == DBNull.Value ? "" : reader.GetString("MENSAGEM"));
							}
							return result;
						}
					}
				}
				return null;

            }
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
        }


        public async Task<List<dynamic>?> getPerguntasColaborador(string matricula, string connStr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("STP_CONSULTA_PERGUNTAS_FUNCIONARIO_ALTERAR_SENHA_NOVO"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AGTCODIGO", matricula);
    
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            List<dynamic> listResp = new List<dynamic>();
                            while (await reader.ReadAsync())
                            {
                                var result = new ExpandoObject() as IDictionary<string, object>;
                                result.Add("Codigo", reader["CODIGO"] == DBNull.Value ? "" :Convert.ToInt32(reader["CODIGO"].ToString()) );
                                result.Add("Descricao", reader["DESCRICAO"] == DBNull.Value ? "" : reader.GetString("DESCRICAO"));
                                result.Add("Observacao", reader["OBSERVACAO"] == DBNull.Value ? "" : reader.GetString("OBSERVACAO"));
                                result.Add("Resposta", reader["RESPOSTA"] == DBNull.Value ? "" : reader.GetString("RESPOSTA"));
                                listResp.Add(result);
                            }
                            return listResp;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<dynamic?> setSenhaColaboradorComPergunta(TrocaSenhaPerguntaDTO obj, string connStr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("STP_RECUPERAR_SENHA_COM_CRIPTOGRAFIA_ALTERAR_SENHA_NOVO"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AGTCODIGO", obj.matricula);
                        cmd.Parameters.AddWithValue("@SENHANOV", obj.senhaNova);
                        cmd.Parameters.AddWithValue("@PES_CODIGO", obj.pes_codigo);
                        cmd.Parameters.AddWithValue("@HOSTNAME", "EXTERNO");
                        cmd.Parameters.AddWithValue("@IPADDRESS", "EXTERNO");
                        cmd.Parameters.AddWithValue("@CPF", obj.cpf);
                        cmd.Parameters.AddWithValue("@RESPOSTA", obj.resposta?.ToUpper());
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            var result = new ExpandoObject() as IDictionary<string, object>;
                            while (await reader.ReadAsync())
                            {
                                result.Add("Status", reader["STATUS_MENSAGEM"] == DBNull.Value ? "" : reader.GetString("STATUS_MENSAGEM"));
                                result.Add("Mensagem", reader["MENSAGEM"] == DBNull.Value ? "" : reader.GetString("MENSAGEM"));
                            }
                            return result;
                        }
                    }
                }
                return null;

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
