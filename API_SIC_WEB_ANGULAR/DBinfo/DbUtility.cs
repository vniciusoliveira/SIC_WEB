namespace API_SIC_WEB_ANGULAR.DBinfo
{
    public class DbUtility
    {
        public string StrConnSGIWEB()
        {
            return "Data Source=TMKTBDSQL15;Initial Catalog=SGI_WEB;User ID=ADM;Password=ADMUSR";
        }
        public string StrConnSICWEB()
        {
            return "Data Source=TMKTBDSQL15;Initial Catalog=SIC_WEB;User ID=ADM;Password=ADMUSR";
        }
        public string StrConnEPONTO()
        {
            return "Data Source=TMKTBDSQL05;Initial Catalog=EPONTO;User ID=ADM;Password=ADMUSR";
        }

        #region novo valor
        public static Dictionary<string, string> NovoValor(string indice, string valor)
        {
            Dictionary<string, string> data = new()
            {
                [indice] = valor
            };
            return data;
        }
        public static Dictionary<string, string> NovoValor(List<string> indice, List<string> valores)
        {
            Dictionary<string, string> data = new();
            for (int i = 0; i < indice.Count ; i++)
            {
                data[indice[i]] = valores[i];
            }

            return data;
        }
        #endregion 

        #region select functions
        public string Select(string[] select, string[] from)
        {
            string query = "SELECT ";
            for (int i = 0; i < select.Length; i++)
            {
                query += select[i];
            }
            query += " FROM ";
            for (int i = 0; i < from.Length; i++)
            {
                query += from[i];
            }
            return query;
        }

        public string Select(string[] select, string[] from, string[] where)
        {
            string query = "SELECT ";
            for (int i = 0; i < select.Length; i++)
            {
                query += i == 0 ? select[i] : ","+ select[i];
            }
            query += " FROM ";
            for (int i = 0; i < from.Length; i++)
            {
                query += i == 0 ? from[i] : " AND "+ from[i];
            }
            query += " WHERE ";
            for (int i = 0; i < where.Length; i++)
            {
                query += i == 0 ? where[i] : " AND "+where[i];
            }
            return query;
        }

        #endregion select functions


        public static string[] SelectTabelas_RelatorioHomeOffice(Dictionary<string, string> parametro)
        {
            var retorno = new List<string>();
            if (parametro.ContainsKey("SELECT"))
            {
                retorno.Add(" A.RPD_MATRICULA AS MATRICULA");
                retorno.Add(" B.NOME");
                retorno.Add(" C.CgoDescricao AS CARGO");
                retorno.Add(" A.RPD_SUPERVISOR AS 'MATRICULA SUPERVISOR'");
                retorno.Add(" A.RPD_GERENTE AS 'MATRICULA GERENTE'");
                retorno.Add(" A.RPD_CTRCODIGO AS 'PRODUTO CODIGO'");
                retorno.Add(" D.CTR_DESCRICAO AS PRODUTO");
                retorno.Add(" A.RPD_HOME_OFFICE_ENTRADA AS ENTRADA");

                if (parametro["SELECT"] == "true")
                {
                    foreach (string key in parametro.Keys)
                    {
                        if (!key.Equals("true"))
                        {
                            retorno.Add(key);
                        }
                    }
                }

                return retorno.ToArray();
            }
            else if (parametro.ContainsKey("FROM"))
            {
                retorno.Add(" SGI_WEB.DBO.TB_RESULTADO_PONTO_DIA AS A WITH(NOLOCK)");
                retorno.Add(" INNER JOIN TMKT.DBO.OPERADORES          AS B WITH(NOLOCK) ON A.RPD_MATRICULA = B.NRREG");
                retorno.Add(" INNER JOIN TMKT.dbo.TB_CARGO            AS C WITH(NOLOCK) ON B.CGOCODIGO = C.CgoCodigo");
                retorno.Add(" INNER JOIN TMKT.DBO.TB_CENTRO_RESULTADO AS D WITH(NOLOCK) ON A.RPD_CTRCODIGO = D.CTR_CODIGO");
                return retorno.ToArray();
            }
            else /*parametro.Equals("WHERE")*/
            {
                retorno.Add(" RPD_HOME_OFFICE = 1");
                retorno.Add($" AND RPD_DATA BETWEEN ('{parametro["WHERE1"]}') AND ('{parametro["WHERE2"]}')");

                return retorno.ToArray();
            }

        }
    }
}
