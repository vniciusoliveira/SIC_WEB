namespace API_SIC_WEB_ANGULAR.Model
{
    public class RelatorioHomeOffice
    {
        private string matricula = "", nome = "", cargo = "", matriculaSupervisor = "", matriculaGerente = "",
       produtoCodigo = "", produto = "", entrada = "";

        public string Matricula { get { return matricula; } set { matricula = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public string Cargo { get { return cargo; } set { cargo = value; } }
        public string MatriculaSupervisor { get { return matriculaSupervisor; } set { matriculaSupervisor = value; } }
        public string MatriculaGerente { get { return matriculaGerente; } set { matriculaGerente = value; } }
        public string ProdutoCodigo { get { return produtoCodigo; } set { produtoCodigo = value; } }
        public string Produto { get { return produto; } set { produto = value; } }
        public string Entrada { get { return entrada; } set { entrada = value; } }
    }
}
