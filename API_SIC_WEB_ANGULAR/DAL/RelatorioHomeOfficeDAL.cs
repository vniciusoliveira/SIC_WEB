using API_SIC_WEB_ANGULAR.DBinfo;
using API_SIC_WEB_ANGULAR.Model;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;

namespace API_SIC_WEB_ANGULAR.DAL
{
    public class RelatorioHomeOfficeDAL
    {
        DbUtility _dbUtility = new();
        private readonly string _connectionString;

        public RelatorioHomeOfficeDAL()
        {
            _connectionString = _dbUtility.StrConnSICWEB();
            _dbUtility = new DbUtility();
        }

        public List<RelatorioHomeOffice> GetRelatorioHomeOffices(string dataIni, string dataFim)
        {
            List<RelatorioHomeOffice> relatorios = new();

            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                List<string> whereKey = new()
                {
                    "WHERE",
                    "WHERE1",
                    "WHERE2"
                };
                List<string> whereValue = new()
                {
                    "true",
                    dataIni.ToString(),
                    dataFim.ToString()
                };
                string query = _dbUtility.Select(DbUtility.SelectTabelas_RelatorioHomeOffice(DbUtility.NovoValor("SELECT", "false")), DbUtility.SelectTabelas_RelatorioHomeOffice(DbUtility.NovoValor("FROM", "false")), DbUtility.SelectTabelas_RelatorioHomeOffice(DbUtility.NovoValor(whereKey, whereValue)));
                using SqlCommand cmd = new(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RelatorioHomeOffice obj = new()
                    {
                        Matricula = (reader["MATRICULA"].ToString()) ?? "",
                        Nome = (reader["NOME"].ToString()) ?? "",
                        Cargo = (reader["CARGO"].ToString()) ?? "",
                        MatriculaSupervisor = (reader["MATRICULA SUPERVISOR"].ToString()) ?? "",
                        MatriculaGerente = (reader["MATRICULA GERENTE"].ToString()) ?? "",
                        ProdutoCodigo = (reader["PRODUTO CODIGO"].ToString()) ?? "",
                        Produto = (reader["PRODUTO"].ToString()) ?? "",
                        Entrada = (reader["ENTRADA"].ToString()) ?? ""
                    };

                    relatorios.Add(obj);
                }
            }
            return relatorios;
        }

        public List<RelatorioHomeOfficeAvaliativo> GetRelatorioHomeOfficeOpAvaliativo(Dictionary<string,string> request)
        {
            List<RelatorioHomeOfficeAvaliativo> returnList = new();

            string conn = _connectionString;
            using SqlConnection conexao = new(conn);
            using SqlCommand comando = new();

            comando.Connection = conexao;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText= "STP_RELATORIO_PREMIACAO_HOMEOFFICE_OPERACAO_AVALIATIVO";
            comando.CommandTimeout = 3000;

            comando.Parameters.AddWithValue("@MOVIMENTO", request["MOVIMENTO"]);
            comando.Parameters.AddWithValue("@NRREG_SEARCH", request["NRREG_SEARCH"]);

            try { 
                conexao.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RelatorioHomeOfficeAvaliativo relatorio = new RelatorioHomeOfficeAvaliativo
                        {
                            Codigo = reader["Codigo"] is DBNull ? null : reader["Codigo"].ToString(),
                            Operador = reader["Operador"] is DBNull ? null : reader["Operador"].ToString(),
                            Sup_matricula = reader["Sup_matricula"] is DBNull ? null : reader["Sup_matricula"].ToString(),
                            Supervisor = reader["Supervisor"] is DBNull ? null : reader["Supervisor"].ToString(),
                            Dias_Escalados = Convert.ToInt32(reader["Dias_Escalados"] is DBNull ? null : reader["Dias_Escalados"]),
                            Dias_Trabalhados = Convert.ToInt32(reader["Dias_Trabalhados"] is DBNull ? null : reader["Dias_Trabalhados"]),
                            Cargo = reader["Cargo"] is DBNull ? null : reader["Cargo"].ToString(),
                            Produto = reader["Produto"] is DBNull ? null : reader["Produto"].ToString(),
                            Media_Tempo_Disponivel = reader["Media_Tempo_Disponivel"] is DBNull ? TimeSpan.Zero : TimeSpan.FromSeconds(Convert.ToInt32(reader["Media_Tempo_Disponivel"])),
                            Media_Tempo_Disponivel_Possivel = reader["Media_Tempo_Disponivel_Possivel"] is DBNull ? TimeSpan.Zero : TimeSpan.FromSeconds(Convert.ToInt32(reader["Media_Tempo_Disponivel_Possivel"])),
                            Apto_A_Premiacao = reader["Apto_A_Premiacao"] is DBNull ? null : reader["Apto_A_Premiacao"].ToString(),
                            Premiacao = Convert.ToDouble(reader["Premiacao"] is DBNull ? null : Math.Round(Convert.ToDouble(reader["Premiacao"]),1)),

                        };
                        returnList.Add(relatorio);
                    }
                       return returnList;               
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Movimento> GetMovCodigoAtivo()
        {
            List<Movimento> returnList = new();

            string conn = _connectionString;
            using SqlConnection conexao = new(conn);
            conexao.Open();
            using SqlCommand comando = new();
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = "SELECT TOP 2 MOV_CODIGO, MOV_DATA_INICIO, MOV_DATA_FIM FROM SIC_WEB.DBO.TB_MOVIMENTO ORDER BY 1 DESC";
            comando.CommandTimeout = 3000;

            using SqlDataReader reader = comando.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Movimento relatorio = new Movimento
                    {
                        MOV_CODIGO = Convert.ToInt32(reader["MOV_CODIGO"] is DBNull ? null :  reader["MOV_CODIGO"]),
                        MOV_DATA_INICIO = Convert.ToDateTime(reader["MOV_DATA_INICIO"] is DBNull ? null : reader["MOV_DATA_INICIO"]),
                        MOV_DATA_FIM = Convert.ToDateTime(reader["MOV_DATA_FIM"] is DBNull ? null : reader["MOV_DATA_FIM"]),
                    };
                 returnList.Add(relatorio);
                }
                return returnList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}