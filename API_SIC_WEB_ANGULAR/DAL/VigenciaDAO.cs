using API_SIC_WEB_ANGULAR.Model;
using System.Data.SqlClient;


namespace API_SIC_WEB_ANGULAR.DAL
{
    public class VigenciaDAO
    {
        private readonly string _connectionString;

        public VigenciaDAO(string strConn)
        {
            _connectionString = strConn;
        }

        public List<Vigencia> getVigencia()
        {
            List<Vigencia> vigencias = new List<Vigencia>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PLV_CODIGO, PLV_DATAINI, PLV_DATAFIM FROM SIC_WEB.DBO.TB_PREMIACAO_LIDERANCA_VIGENCIA WHERE PLV_FECHADO = 0";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vigencia obj = new Vigencia();
                            obj.Id = Convert.ToInt32(reader["PLV_CODIGO"]);
                            obj.DataInicial = Convert.ToDateTime(reader["PLV_DATAINI"]);
                            obj.DataFinal = Convert.ToDateTime(reader["PLV_DATAFIM"]);

                            vigencias.Add(obj);
                        }
                    }
                }
            }

            return vigencias;
        }
    }
}
