using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.Model;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace API_SIC_WEB_ANGULAR.DAL.PermissaoDeAcessoEloginAntigo
{
    public class GetInfoColaboradoresDAL
    {
        private readonly DbUtility _dbUtility;
        public GetInfoColaboradoresDAL()
        {
            _dbUtility = new DbUtility();
        }

        public async Task<ColaboradorInfos> getInfoColaborador(string matricula)
        {
            try
            {
                ColaboradorInfos colaborador = new ColaboradorInfos();

                string query = @"
                               SELECT NRREG,NOME,CTR_DESCRICAO,EQUIPE_NRREG,CGO_CODIGO, CGO_DESCRICAO FROM tmkt_ori.dbo.operadores WITH(NOLOCK)
		                       INNER JOIN tmkt_ori.dbo.tb_centro_resultado WITH(NOLOCK) ON (CTRCODIGO = CTR_CODIGO)
                               INNER JOIN TMKT_ORI.DBO.TB_CARGO WITH(NOLOCK) ON (CgoCodigo = CGO_CODIGO)
                               WHERE STATINDI = 'A'
		                       AND NRREG NOT LIKE '%99%'
                               AND NRREG = @matricula";

                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnSICWEB()))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@matricula", matricula);
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                colaborador = new ColaboradorInfos
                                {
                                    Matricula = reader["nrreg"].ToString() ?? "",
                                    Nome = reader["nome"].ToString() ?? "",
                                    Cr = reader["ctr_descricao"].ToString() ?? "",
                                    MatriculaSupervisor = reader["equipe_nrreg"].ToString() ?? "",
                                    CgoCodigo = Convert.ToInt32(reader["cgo_codigo"]) ,
                                    CgoDescricao = reader["cgo_descricao"].ToString() ?? ""
                                };
                            }
                        }
                    }
                }
                return colaborador;
            }
            catch (Exception ex)
            {
                throw new Exception(matricula, ex);
            }         
        }
        
        public async Task<bool> InsereDesbloqueioOperadorDAO(ColaboradorInfos colaboradorObj)
        {
            string queryInsert = "INSERT INTO TB_MATRICULA_EXCESSAO_BLOQUEIO_ELOGIN_ANTIGO (TMB_NRREG_BLOQUEIO_EXCESSAO, TMB_DATA, TMB_MOTIVO_EXCESSAO, TMB_MATRICULA_LOG) VALUES (@Matricula, GETDATE(), @MotivoDesbloqueio, @MatriculaLog)";
            try
            {
                using (SqlConnection conn = new SqlConnection(_dbUtility.StrConnEPONTO()))
                {
                    await conn.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(queryInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@Matricula", colaboradorObj.Matricula);
                        cmd.Parameters.AddWithValue("@MotivoDesbloqueio", colaboradorObj.motivoDesbloqueio);
                        cmd.Parameters.AddWithValue("@MatriculaLog", "N/D");

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if(rowsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("ERR",ex);
            }          
        }
    }
}
