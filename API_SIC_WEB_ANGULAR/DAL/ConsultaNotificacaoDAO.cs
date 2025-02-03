using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class ConsultaNotificacaoDAO
    {
       private readonly DbUtility _dbUtility;
        public ConsultaNotificacaoDAO()
        {
            _dbUtility = new DbUtility();
        }

        public List<ConsultaNotificacao> ConsultarNotificacao(Dictionary<string, string> request)
        {
            List<ConsultaNotificacao> returnList = new();
            string[] select =
            {
                "MOP_NRREG",
                "MOP_LINK",
                "MOP_MENSAGEM",
                "MOP_LIDA",
                "MOP_DATAHORA_LIDA",
                "MOP_DATAHORA_INSERT"
            };          
            string[] from = { "TB_MENSAGEM_OPERADORES" };          
            string[] where =
            {
                $"MOP_NRREG='{ request["NRREG"] }'",
                $"MOP_DATAHORA_INSERT BETWEEN '{request["DTINI"]}' AND '{request["DTEND"]}'"
            };
            string query = _dbUtility.Select(select, from, where);
            try
            {
                using SqlConnection conn = new(_dbUtility.StrConnEPONTO());
                conn.Open();
                using SqlCommand cmd = new(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ConsultaNotificacao obj = new()
                    {
                        Mop_nrreg = !reader["mop_nrreg"].Equals(DBNull.Value) ? reader["MOP_NRREG"].ToString() : "N/T",
                        Mop_link = !reader["mop_link"].Equals(DBNull.Value) ? reader.GetString(1) : "N/T",
                        Mop_mensagem = !reader["mop_mensagem"].Equals(DBNull.Value) ? reader.GetString(2) : "N/T",
                        Mop_lida = (!reader["mop_mensagem"].Equals(DBNull.Value)) && (Convert.ToInt32(reader["MOP_LIDA"]) == 1),
                        Mop_datahora_insert = !reader["mop_datahora_insert"].Equals(DBNull.Value) ? reader.GetDateTime(4) : DateTime.MinValue,
                        Mop_datahora_lida = !reader["mop_datahora_lida"].Equals(DBNull.Value) ? reader.GetDateTime(5) : DateTime.MinValue
                    };

                    returnList.Add(obj);
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnList;
        }


        public async Task<List<ChatRelatorio>> GetRelatorioChat(string matricula, string matriculaLog, string strConn, string? destinatario = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                List<ChatRelatorio> listResponse = new List<ChatRelatorio>();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.CommandText = "STP_GET_RELATORIO_MENSAGENS_CHAT_TMKT";
                        cmd.Parameters.AddWithValue("@MATRICULA_REMETENTE", matricula);
                        cmd.Parameters.AddWithValue("@MATRICULA_DESTINATARIO", destinatario);
                        cmd.Parameters.AddWithValue("@DATA_INI", dataInicio);
                        cmd.Parameters.AddWithValue("@DATA_FIM", dataFim);
                        cmd.Parameters.AddWithValue("@MATRICULA_LOG", matriculaLog);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ChatRelatorio obj = new ChatRelatorio();
                                obj.matricula = reader["MATRICULA"] == DBNull.Value ? "" : reader.GetString("MATRICULA");
                                obj.remetente = reader["REMETENTE"] == DBNull.Value ? "" : reader.GetString("REMETENTE");
                                obj.destinatario = reader["DESTINATARIO"] == DBNull.Value ? "" : reader.GetString("DESTINATARIO");
                                obj.mensagem = reader["MENSAGEM"] == DBNull.Value ? "" : reader.GetString("MENSAGEM");
                                obj.dataEnvio = reader["DATA ENVIO"] == DBNull.Value ? null : Convert.ToDateTime(reader["DATA ENVIO"]);
                                obj.horaEnvio = reader["HORA ENVIO"] == DBNull.Value ? null : Convert.ToDateTime(reader["HORA ENVIO"]).TimeOfDay;
                                obj.dataLido = reader["DATA LIDO"] == DBNull.Value ? null : Convert.ToDateTime(reader["DATA LIDO"]);
                                obj.horaLido = reader["HORA LIDO"] == DBNull.Value ? null : Convert.ToDateTime(reader["HORA LIDO"]).TimeOfDay;
                                obj.remetenteFLG = reader["REMETENTEFLG"] == DBNull.Value ? 0 : Convert.ToInt32(reader["REMETENTEFLG"].ToString());
                                listResponse.Add(obj);
                            }
                            return listResponse;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
