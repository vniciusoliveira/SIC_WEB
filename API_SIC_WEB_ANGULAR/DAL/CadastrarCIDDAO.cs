using API_SIC_WEB_ANGULAR.DBinfo;
using System.Data.SqlClient;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class CadastrarCIDDAO
    {
        private readonly DbUtility _dbUtility;

        public CadastrarCIDDAO()
        {
            _dbUtility = new DbUtility();
        }
        public bool Cadastrar(Dictionary<string,string> request)
        {
            string proc = "SIC_WEB.DBO.STP_INSERT_CID_ATESTADO";
            try
            {
                using SqlConnection conn = new(_dbUtility.StrConnSICWEB());
                conn.Open();
                using SqlCommand cmd = new(proc, conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CODIGO_CID", request["CID"]);
                cmd.Parameters.AddWithValue("@DESCRICAO", request["DESC"]);
                cmd.Parameters.AddWithValue("@NRREG_LOG", request["MATRICULA"]);
                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
