using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class MedidaDisciplinarDAL
    {
        private readonly DbUtility _dbUtility;

        public MedidaDisciplinarDAL()
        {
            _dbUtility = new DbUtility();
        }

        public async Task<List<MedidaDisciplinarModel>> GetMedidaDisciplinarDAL(int mtaCodigo, string matricula)
        {
            try
            {
                var medidasDisciplinares = new List<MedidaDisciplinarModel>();

                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand cmd = new SqlCommand("STP_SELECIONA_MEDIDA_DISCIPLINAR_APLICACAO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MTA_CODIGO", mtaCodigo);
                        cmd.Parameters.AddWithValue("@AGT_CODIGO", matricula);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                               var medidasDisciplinar = new MedidaDisciplinarModel
                                {
                                    MDR_Codigo = Convert.ToInt32(reader["MDR_CODIGO"]),
                                    OPE_NRREG = reader["OPE_NRREG"].ToString() ?? "",
                                    OPE_Nome = reader["OPE_NOME"].ToString() ?? "",
                                    CGO_Descricao = reader["CGO_DESCRICAO"].ToString() ?? "",
                                    SUP_NRREG = reader["SUP_NRREG"].ToString() ?? "",
                                    SUP_Nome = reader["SUP_NOME"].ToString() ?? "",
                                    MDR_Data = reader["MDR_DATA"].ToString() ?? "",
                                    CMD_Descricao = reader["CMD_DESCRICAO"].ToString() ?? "",
                                    MDR_DescricaoMedida = reader["MDR_DESCRICAO_MEDIDA"].ToString() ?? "",
                                    TSD_Descricao = reader["TSD_DESCRICAO"].ToString() ?? "",
                                    CTR_Descricao = reader["CTR_DESCRICAO"].ToString() ?? ""
                                };
                                medidasDisciplinares.Add(medidasDisciplinar);
                            }
                        }
                    }
                    return medidasDisciplinares;
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao buscar medidas disciplinares para a matrícula {matricula}", ex);
            }
        }

        public async Task<MedidaDisciplinarModel?> ListaDeparaMedidaDisciplinar(MedidaDisciplinarDTO medidaDTO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand cmd = new SqlCommand("STP_LISTA_DEPARA_MEDIDA_DISCIPLINAR_V3", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MDRCODIGO", medidaDTO.MDR_Codigo);

                        await conn.OpenAsync();

                        using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                    var dadosMedidaDisciplinar = new MedidaDisciplinarModel
                                    {
                                        MDR_Codigo = Convert.ToInt32(reader["MDR_CODIGO"]),
                                        OPE_NRREG = reader["MDR_NRREG"].ToString() ?? "",
                                        OPE_Nome = reader["NOME"].ToString() ?? "",
                                        CTR_Descricao = reader["CTR_DESCRICAO"].ToString() ?? "",
                                        MDR_Data = Convert.ToDateTime(reader["DATAHORA"]).ToString() ?? DateTime.MinValue.ToString(),
                                        ADVERTENCIA = reader["ADVERTENCIA"].ToString() ?? "",
                                        SUSPENSAO = reader["SUSPENSAO"].ToString() ?? "",
                                        DATARETORNO = reader["DATARETORNO"].ToString() ?? "",
                                        NOME_GESTOR = reader["NOME_GESTOR"].ToString() ?? "",
                                        DATAEMISSAO = reader["DATAEMISSAO"].ToString() ?? "",
                                        MOTIVO_MEDIDA = reader["MOTIVO_MEDIDA"].ToString() ?? "",
                                        MDR_DescricaoMedida = reader["DESCRICAO_MEDIDA"].ToString() ?? "",
                                        MDR_DataHora = reader["DATA_OCORRENCIA"].ToString() ?? "",
                                        TESTEMUNHA_1 = reader["TESTEMUNHA_1"].ToString() ?? "",
                                        TESTEMUNHA_2 = reader["TESTEMUNHA_2"].ToString() ?? ""
                                    };
                                    return dadosMedidaDisciplinar;                             
                            }
                        }
                        return null;
                    }
                }
            }
            catch(SqlException ex)
            {
                 throw new Exception($"Erro ao buscar os dados da medida disciplinar: {medidaDTO.MDR_Codigo}", ex);
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosGerente(ColaboradorInfosDTO colaboradorInfos)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT DTNASC,NUMCPF,NRREG FROM TMKT_ORI.DBO.OPERADORES WHERE NUMCPF = @CPF AND FUNCAO IN ('G','C') and STATINDI = 'A'";

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPF", colaboradorInfos.NumCpf);
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dadosGerente = new ColaboradorInfosDTO
                                {
                                    DtNasc = reader["DTNASC"].ToString() ?? "",
                                    NumCpf = reader["NUMCPF"].ToString() ?? "",
                                    Nrreg = reader["NRREG"].ToString() ?? "",
                                    valido = true                                   
                                };
                                return dadosGerente;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados do Gerente: {ex}");
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosOperadorDAL(ColaboradorInfosDTO colaboradorInfos)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT NOME,DTNASC,NUMCPF,NRREG FROM TMKT_ORI.DBO.OPERADORES WHERE NUMCPF = @CPF AND STATINDI = 'A'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPF", colaboradorInfos.NumCpf);
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dadosOperador = new ColaboradorInfosDTO
                                {
                                    Nome = reader["NOME"].ToString() ?? "",
                                    DtNasc = reader["DTNASC"].ToString() ?? "",
                                    NumCpf = reader["NUMCPF"].ToString() ?? "",
                                    Nrreg = reader["NRREG"].ToString() ?? "",
                                    valido = true                                  
                                };
                                return dadosOperador;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados do Gerente: {ex}");
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosTestemunhaDAL(ColaboradorInfosDTO colaboradorInfos)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT NOME,DTNASC,NUMCPF,NRREG FROM TMKT_ORI.DBO.OPERADORES WHERE NUMCPF = @CPF AND STATINDI = 'A'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPF", colaboradorInfos.NumCpf);
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dadosOperador = new ColaboradorInfosDTO
                                {
                                    Nome = reader["NOME"].ToString() ?? "",
                                    DtNasc = reader["DTNASC"].ToString() ?? "",
                                    NumCpf = reader["NUMCPF"].ToString() ?? "",
                                    Nrreg = reader["NRREG"].ToString() ?? "",
                                    valido = true                                   
                                };
                                return dadosOperador;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados do Gerente: {ex}");
            }
        }

        public async Task<bool> ConfirmaAssinaturaGerenteContasDAL(AssinaturasMedidaDisciplinarGerenteContasDTO objAssinaturaMedida, GetInfoMaquinaColaborador maquinaInfo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "INSERT INTO TB_MEDIDA_DISCIPLINAR_DIGITAL (MDD_GERENTE_CONTAS_NRREG, MDD_NOME_GERENTE_CONTAS, MDD_GERENTE_CONTAS_CPF, MDD_GERENTE_CONTAS_DTNASC, MDD_MDRCODIGO,MDD_DATAHORA,MDD_GERENTE_HOSTNAME_ASSINATURA, MDD_GERENTE_IP_ASSINATURA, MDD_GERENTE_GEOLOCALIZACAO_ASSINATURA) VALUES (@MatriculaGerenteContas, @NomeGerenteContas, @CpfGerenteContas, @DataNascimentoGerenteContas, @MddMdrCodigo, @MddDataHora, @MddHostnameGerente, @MddIpGerente, @MddGeolocalizacaoGerente)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MatriculaGerenteContas", objAssinaturaMedida.MddGerenteContasNrreg);
                        cmd.Parameters.AddWithValue("@NomeGerenteContas", objAssinaturaMedida.MddNomeGerenteContas);
                        cmd.Parameters.AddWithValue("@CpfGerenteContas", objAssinaturaMedida.MddGerenteContasCpf);
                        cmd.Parameters.AddWithValue("@DataNascimentoGerenteContas", objAssinaturaMedida.MddGerenteContasDtNasc);
                        cmd.Parameters.AddWithValue("@MddMdrCodigo", objAssinaturaMedida.MddMdrCodigo);
                        cmd.Parameters.AddWithValue("@MddDataHora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@MddHostnameGerente", maquinaInfo.hostname);
                        cmd.Parameters.AddWithValue("@MddIpGerente", maquinaInfo.ip);
                        cmd.Parameters.AddWithValue("@MddGeolocalizacaoGerente", maquinaInfo.geoLocalizacao);

                        await conn.OpenAsync();

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        return linhasAfetadas > 1;
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao confirmar assinatura do gerente de contas: {ex.Message}");
            }
        }

        public async Task<bool> ConfirmaAssinaturaOperadorDAL(AssinaturasMedidaDisciplinarEmpregadoDTO objMedidaAssinaturaEmpregado, GetInfoMaquinaColaborador maquinaInfo) 
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "UPDATE TB_MEDIDA_DISCIPLINAR_DIGITAL SET MDD_EMPREGADO_NRREG = @Mdd_empregado_nrreg, " +
                                   "MDD_NOME_EMPREGADO = @Mdd_nome_empregado, MDD_EMPREGADO_CPF = @Mdd_empregado_cpf, " +
                                   "MDD_EMPREGADO_DTNASC = @Mdd_empregado_dtnasc, MDD_DATAHORA_ASSINATURA_EMPREGADO = " +
                                   "@Mdd_datahora_empregado_assinatura, MDD_EMPREGADO_HOSTNAME_ASSINATURA = @Mdd_hostname_empregado, " +
                                   "MDD_EMPREGADO_IP_ASSINATURA = @Mdd_ip_empregado, MDD_EMPREGADO_GEOLOCALIZACAO_ASSINATURA = @Mdd_geolocalizacao_empregado WHERE MDD_MDRCODIGO = @MdrCodigo";

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Mdd_empregado_nrreg", objMedidaAssinaturaEmpregado.MddEmpregadoNrreg);
                        cmd.Parameters.AddWithValue("@Mdd_nome_empregado", objMedidaAssinaturaEmpregado.MddNomeEmpregado);
                        cmd.Parameters.AddWithValue("@Mdd_empregado_cpf", objMedidaAssinaturaEmpregado.MddEmpregadoCPF);
                        cmd.Parameters.AddWithValue("@Mdd_empregado_dtnasc", objMedidaAssinaturaEmpregado.MddEmpregadoDtNasc);
                        cmd.Parameters.AddWithValue("@Mdd_datahora_empregado_assinatura", DateTime.Now);
                        cmd.Parameters.AddWithValue("@MdrCodigo", objMedidaAssinaturaEmpregado.MddMdrCodigo);
                        cmd.Parameters.AddWithValue("@Mdd_hostname_empregado", maquinaInfo.hostname);
                        cmd.Parameters.AddWithValue("@Mdd_ip_empregado", maquinaInfo.ip);
                        cmd.Parameters.AddWithValue("@Mdd_geolocalizacao_empregado", maquinaInfo.geoLocalizacao);

                        await conn.OpenAsync();

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        return linhasAfetadas > 1;

                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao confirmar assinatura do empregado: {ex.Message}");
            }
        }


        public async Task<bool> ConfirmaAssinaturaTestemunhaDAL(AssinaturaMedidaDisciplinarTestemunhaDTO objMedidaAssinaturaTestemunha, GetInfoMaquinaColaborador maquinaInfo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "UPDATE TB_MEDIDA_DISCIPLINAR_DIGITAL SET MDD_TESTEMUNHA_NRREG = @Mdd_testemunha_nrreg, " +
                                   "MDD_NOME_TESTEMUNHA = @Mdd_nome_testemunha, MDD_TESTEMUNHA_CPF = @Mdd_testemunha_cpf, " +
                                   "MDD_TESTEMUNHA_DTNASC = @Mdd_testemunha_dtnasc, MDD_DATAHORA_ASSINATURA_TESTEMUNHA = " +
                                   "@Mdd_datahora_testemunha_assinatura, MDD_TESTEMUNHA_HOSTNAME_ASSINATURA = @Mdd_hostname_testemunha, " +
                                   "MDD_TESTEMUNHA_IP_ASSINATURA = @Mdd_testemunha_ip, MDD_TESTEMUNHA_GEOLOCALIZACAO_ASSINATURA = @Mdd_testemunha_geolocalizacao WHERE MDD_MDRCODIGO = @MdrCodigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Mdd_testemunha_nrreg", objMedidaAssinaturaTestemunha.MddTestemunhaNrreg);
                        cmd.Parameters.AddWithValue("@Mdd_nome_testemunha", objMedidaAssinaturaTestemunha.MddNomeTestemunha);
                        cmd.Parameters.AddWithValue("@Mdd_testemunha_cpf", objMedidaAssinaturaTestemunha.MddTestemunhaCPF);
                        cmd.Parameters.AddWithValue("@Mdd_testemunha_dtnasc", objMedidaAssinaturaTestemunha.MddTestemunhaDtNasc);
                        cmd.Parameters.AddWithValue("@Mdd_datahora_testemunha_assinatura", DateTime.Now);
                        cmd.Parameters.AddWithValue("@MdrCodigo", objMedidaAssinaturaTestemunha.MddMdrCodigo);
                        cmd.Parameters.AddWithValue("@Mdd_hostname_testemunha", maquinaInfo.hostname);
                        cmd.Parameters.AddWithValue("@Mdd_testemunha_ip", maquinaInfo.ip);
                        cmd.Parameters.AddWithValue("@Mdd_testemunha_geolocalizacao", maquinaInfo.geoLocalizacao);

                        await conn.OpenAsync();

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        return linhasAfetadas > 1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao confirmar assinatura do empregado: {ex.Message}");
            }
        }


        public async Task<AssinaturasMedidaDisciplinarGerenteContasDTO> BuscaAssinaturaAssinadaMedidaDisciplinarGerenteDAL(int mdrCodigo)
        {
            //var medidasDisciplinaresAssinaturaGerente = new List<AssinaturasMedidaDisciplinarGerenteContasDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT MDD_NOME_GERENTE_CONTAS, MDD_GERENTE_CONTAS_CPF, MDD_DATAHORA  FROM TB_MEDIDA_DISCIPLINAR_DIGITAL WITH(NOLOCK) WHERE MDD_MDRCODIGO = @MDRCODIGO";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MDRCODIGO", mdrCodigo);
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                var medidaDisciplinarAssinada = new AssinaturasMedidaDisciplinarGerenteContasDTO
                                {
                                    MddNomeGerenteContas = reader["MDD_NOME_GERENTE_CONTAS"].ToString() ?? "",
                                    MddGerenteContasCpf = reader["MDD_GERENTE_CONTAS_CPF"].ToString() ?? "",
                                    MddGerenteAssinaturaDtHora = Convert.ToDateTime(reader["MDD_DATAHORA"]),
                                    Assinado = true
                                };
                                //medidasDisciplinaresAssinaturaGerente.Add(medidaDisciplinarAssinada);
                                return medidaDisciplinarAssinada;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados das assinaturas na DAL: {ex.Message}");
            }
        }

        public async Task<AssinaturasMedidaDisciplinarEmpregadoDTO> BuscaAssinaturaAssinadaMedidaDisciplinarEmpregadoDAL(int mdrCodigo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT MDD_NOME_EMPREGADO, MDD_EMPREGADO_CPF, MDD_DATAHORA_ASSINATURA_EMPREGADO  FROM TB_MEDIDA_DISCIPLINAR_DIGITAL WITH(NOLOCK) WHERE MDD_MDRCODIGO = @MDRCODIGO";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MDRCODIGO", mdrCodigo);
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var medidaDisciplinarAssinadaEmpregado = new AssinaturasMedidaDisciplinarEmpregadoDTO
                                {
                                    MddNomeEmpregado = reader["MDD_NOME_EMPREGADO"].ToString() ?? "",
                                    MddEmpregadoCPF = reader["MDD_EMPREGADO_CPF"].ToString() ?? "",
                                    MddEmpregadoAssinaturaDtHora = reader["MDD_DATAHORA_ASSINATURA_EMPREGADO"] != DBNull.Value
                                ? Convert.ToDateTime(reader["MDD_DATAHORA_ASSINATURA_EMPREGADO"])
                                : (DateTime?)null,
                                    Assinado = true
                                };

                                if (medidaDisciplinarAssinadaEmpregado.MddEmpregadoAssinaturaDtHora.Equals(null))
                                {
                                    medidaDisciplinarAssinadaEmpregado.Assinado = false;
                                    return medidaDisciplinarAssinadaEmpregado;
                                }
                                else
                                {
                                    return medidaDisciplinarAssinadaEmpregado;
                                }
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados das assinaturas na DAL: {ex.Message}");
            }
        }

        public async Task<AssinaturaMedidaDisciplinarTestemunhaDTO> BuscaAssinaturaAssinadaMedidaDisciplinarTestemunhaDAL(int mdrCodigo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    string query = "SELECT MDD_NOME_TESTEMUNHA, MDD_TESTEMUNHA_CPF, MDD_DATAHORA_ASSINATURA_TESTEMUNHA FROM TB_MEDIDA_DISCIPLINAR_DIGITAL WITH(NOLOCK) WHERE MDD_MDRCODIGO = @MDRCODIGO";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MDRCODIGO", mdrCodigo);
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var medidaDisciplinarAssinadaTestemunha = new AssinaturaMedidaDisciplinarTestemunhaDTO
                                {
                                    MddNomeTestemunha = reader["MDD_NOME_TESTEMUNHA"].ToString() ?? "",
                                    MddTestemunhaCPF = reader["MDD_TESTEMUNHA_CPF"].ToString() ?? "",
                                    MddTestemunhaAssinaturaDtHora = reader["MDD_DATAHORA_ASSINATURA_TESTEMUNHA"] != DBNull.Value
                                ? Convert.ToDateTime(reader["MDD_DATAHORA_ASSINATURA_TESTEMUNHA"])
                                : (DateTime?)null,
                                    Assinado = true
                                };

                                if (medidaDisciplinarAssinadaTestemunha.MddTestemunhaAssinaturaDtHora.Equals(null))
                                {
                                    medidaDisciplinarAssinadaTestemunha.Assinado = false;
                                    return medidaDisciplinarAssinadaTestemunha;
                                }
                                else
                                {
                                    return medidaDisciplinarAssinadaTestemunha;
                                }
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os dados das assinaturas na DAL: {ex.Message}");
            }
        }
    }
}
