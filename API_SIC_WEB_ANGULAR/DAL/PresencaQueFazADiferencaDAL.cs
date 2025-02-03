using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.DTO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class PresencaQueFazADiferencaDAL
    {
        private readonly DbUtility _dbUtility;

        public PresencaQueFazADiferencaDAL()
        {
            _dbUtility = new DbUtility();
        }

        public async Task<List<PresencaQueFazADiferencaDTO>> consultaComissaoBanco(ConsultaPremiacaoConsolidadaPresencaQueFazADiferencaDTO dtoComissao)
        {
             List<PresencaQueFazADiferencaDTO> listObj = new List<PresencaQueFazADiferencaDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand cmd = new SqlCommand("STP_CONSULTA_PREMIACAO_PRESENCA_QUE_FAZ_A_DIFERENCA", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NRREG", dtoComissao.nrreg);
                        cmd.Parameters.AddWithValue("@VPE_CODIGO", dtoComissao.vpe_codigo);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                PresencaQueFazADiferencaDTO dadosComissao = new PresencaQueFazADiferencaDTO
                                {
                                    nrreg = dtoComissao.nrreg,
                                    vpe_codigo = dtoComissao.vpe_codigo,
                                    data = reader["DATA_RSP"].ToString() ?? "",
                                    ponto = reader["PONTOS_RSP"].ToString() ?? "",
                                    valor = reader["VALOR_RSP"].ToString() ?? "",
                                    MSG_ELEGIVEL = reader["MSG_ELEGIVEL"].ToString() ?? "",
                                    elegivel = Convert.ToInt32(reader["FLG_ELEGIVEL"].ToString()) == 1
                                };
                                listObj.Add(dadosComissao);
                            }
                            listObj.RemoveAll(obj =>  obj.valor == "" && obj.elegivel == true);
                            return listObj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados da medida disciplinar: {dtoComissao.nrreg}", ex);
            }
        }

        public async Task<PresencaQueFazADiferencaDTO> consultaVigenciaBanco()
        {
            try
            {
                //List<PresencaQueFazADiferencaDTO> listObj = new List<PresencaQueFazADiferencaDTO>();

                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand cmd = new SqlCommand("STP_CONSULTA_VIGENCIA_PREMIACAO_PRESENCA_QUE_FAZ_A_DIFERENCA ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@DATAHORA", null);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                PresencaQueFazADiferencaDTO dadosVigencia = new PresencaQueFazADiferencaDTO
                                {
                                    vpe_codigo = reader["VPE_CODIGO"].ToString(),
                                    vpe_data_inicio = reader["VPE_DATA_INICIO"].ToString(),
                                    vpe_data_fim = reader["VPE_DATA_FIM"].ToString()                           
                                };
                                return dadosVigencia;
                                //listObj.Add(dadosVigencia);
                            }
                            //return listObj;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados da medida disciplinar:", ex);
            }
        }

        public async Task<ConsultaPontosCrsPresencaQueFazADiferenca> ConsultaPontosCRsDAL(string nrreg, string dtHora)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand cmd = new SqlCommand("STP_CONSULTA_VALOR_PREMIACAO_PRESENCA_QUE_FAZ_A_DIFERENCA ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NRREG", nrreg);
                        cmd.Parameters.AddWithValue("@DATAHORA", dtHora);


                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dadosCrOperador = new ConsultaPontosCrsPresencaQueFazADiferenca
                                {
                                    pep_ctrCodigo = reader["PEP_CTRCODIGO"].ToString() ?? "",
                                    pep_acocodigo = reader["PEP_ACOCODIGO"].ToString() ?? "",
                                    pep_flg_venda = reader["PEP_FLG_VENDA"].ToString() ?? "",
                                    pep_ponto_por_venda = reader["PEP_PONTO_POR_VENDA"].ToString() ?? "",
                                    pep_flg_tempo_disponivel = reader["PEP_FLG_TEMPO_DISPONIVEL"].ToString() ?? "",
                                    pep_ponto_tempo_disponivel_dia = reader["PEP_PONTO_TEMPO_DISPONIVEL_DIA"].ToString() ?? "",
                                };
                                return dadosCrOperador;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados da medida disciplinar:", ex);
            }
        }

        public async Task<List<Dictionary<string,object>>> BuscaArquivoBanco(string vpe_codigo, string strConn)
        {
            try
            {
                var result = new List<Dictionary<string,object>>();

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "STP_PREMIACAO_PRESENCA_QUE_FAZ_A_DIFERENCA_ARQUIVO";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VPE_CODIGO", vpe_codigo);

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
