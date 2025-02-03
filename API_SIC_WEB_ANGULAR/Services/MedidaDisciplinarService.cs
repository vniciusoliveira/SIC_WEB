using API_SIC_WEB_ANGULAR.DAL;
using API_SIC_WEB_ANGULAR.DTO;
using API_SIC_WEB_ANGULAR.Model;
using System.Text.RegularExpressions;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class MedidaDisciplinarService
    {
        private readonly DAL.PermissaoDeAcessoEloginAntigo.GetInfoColaboradoresDAL _colaboradorInfosDal;
        private readonly DAL.MedidaDisciplinarDAL _medidaDisciplinarDAL;

        public MedidaDisciplinarService(DAL.PermissaoDeAcessoEloginAntigo.GetInfoColaboradoresDAL dal, MedidaDisciplinarDAL medidaDisciplinarDAL)
        {
            _colaboradorInfosDal = dal;
            _medidaDisciplinarDAL = medidaDisciplinarDAL;
        }

        public async Task<ColaboradorInfos> BuscaDadosColaboradorMedidaDisciplinar(string matricula)
        {
            // Busca informações do colaborador
            ColaboradorInfos colaborador = await _colaboradorInfosDal.getInfoColaborador(matricula);
            return colaborador;
        }

        public async Task<List<MedidaDisciplinarModel>> BuscarMedidasDisciplinaresPendentes(int mtaCodigo, string matricula)
        {
            try
            {
                return await Task.Run(() => _medidaDisciplinarDAL.GetMedidaDisciplinarDAL(mtaCodigo, matricula));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar as medidas na DAL {ex.Message}, consulte os administradores.");
            }
        }

        public async Task<MedidaDisciplinarModel> GetMedidaDisciplinarDetalhes(MedidaDisciplinarDTO medidaDTO)
        {
            try
            {
                return await Task.Run(() => _medidaDisciplinarDAL.ListaDeparaMedidaDisciplinar(medidaDTO));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os detalhes da medida disciplinar na DAL '{ex.Message}', consulte os administradores.");
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosGerente(ColaboradorInfosDTO colaboradorInfos)
        {
            try
            {
                //return await Task.Run(() => _medidaDisciplinarDAL.GetDadosGerente(colaboradorInfos));
                var dadosGerente = await _medidaDisciplinarDAL.GetDadosGerente(colaboradorInfos);
                return dadosGerente;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os detalhes da medida disciplinar na DAL '{ex.Message}', consulte os administradores.");
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosOperador(ColaboradorInfosDTO colaboradorEmpregadoInfos)
        {
            try
            {
                var dadosOperador = await _medidaDisciplinarDAL.GetDadosOperadorDAL(colaboradorEmpregadoInfos);
                return dadosOperador;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os detalhes da medida disciplinar na DAL '{ex.Message}', consulte os administradores.");
            }
        }

        public async Task<ColaboradorInfosDTO> GetDadosTestemunha(ColaboradorInfosDTO colaboradorTestemunhasInfos)
        {
            try
            {
                var dadosTestemunha = await _medidaDisciplinarDAL.GetDadosTestemunhaDAL(colaboradorTestemunhasInfos);
                return dadosTestemunha;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar os detalhes da medida disciplinar na DAL '{ex.Message}', consulte os administradores.");
            }
        }

        public async Task<bool> ConfirmaAssinaturaOperadorService(AssinaturasMedidaDisciplinarEmpregadoDTO objAssinaturaEmpregado, GetInfoMaquinaColaborador maquinaInfo)
        {
            try
            {
                var assinaturaOperador = await _medidaDisciplinarDAL.ConfirmaAssinaturaOperadorDAL(objAssinaturaEmpregado, maquinaInfo);
                return assinaturaOperador;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<bool> ConfirmaAssinaturaGerenteContasService(AssinaturasMedidaDisciplinarGerenteContasDTO objAssinatura, GetInfoMaquinaColaborador maquinaInfo)
        {
            try
            {
                var assinaturaGerente = await _medidaDisciplinarDAL.ConfirmaAssinaturaGerenteContasDAL(objAssinatura, maquinaInfo);
                return assinaturaGerente;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());               
            }
        }

        public async Task<bool> ConfirmaAssinaturaTestemunhaService(AssinaturaMedidaDisciplinarTestemunhaDTO objAssinaturaTestemunha, GetInfoMaquinaColaborador maquinaInfo)
        {
            try
            {
                var assinaturaGerente = await _medidaDisciplinarDAL.ConfirmaAssinaturaTestemunhaDAL(objAssinaturaTestemunha, maquinaInfo);
                return assinaturaGerente;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public async Task<AssinaturasMedidaDisciplinarEmpregadoDTO> BuscaAssinaturaAssinadaMedidaDisciplinarEmpregadoService(int mdrCodigo)
        {
            try
            {
                var AssinaturasConfirmadasEmpregado = await _medidaDisciplinarDAL.BuscaAssinaturaAssinadaMedidaDisciplinarEmpregadoDAL(mdrCodigo);
                return AssinaturasConfirmadasEmpregado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<AssinaturaMedidaDisciplinarTestemunhaDTO> BuscaAssinaturaAssinadaMedidaDisciplinarTestemunhaService(int mdrCodigo)
        {
            try
            {
                var AssinaturasConfirmadasTestemunha = await _medidaDisciplinarDAL.BuscaAssinaturaAssinadaMedidaDisciplinarTestemunhaDAL(mdrCodigo);
                return AssinaturasConfirmadasTestemunha;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<AssinaturasMedidaDisciplinarGerenteContasDTO> BuscaAssinaturaAssinadaMedidaDisciplinarService(int mdrCodigo)
        {
            try
            {
                var AssinaturasConfirmadas = await _medidaDisciplinarDAL.BuscaAssinaturaAssinadaMedidaDisciplinarGerenteDAL(mdrCodigo);
                return AssinaturasConfirmadas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<int> CalcularMtaCodigo(ColaboradorInfos colaborador)
        {
            int mtaCodigo;

            if (colaborador.Matricula == "874459")
            {
                mtaCodigo = 2;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "LIDER.*OPERA", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 1;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "SUPERVISOR.*ATENDIMENTO", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 1;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "MULTIPLICADOR", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 1;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "GERENTE.*CONTAS", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 2;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "LIDER.*R.*S", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 2;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "COORDE.*OPER", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 2;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "GERENTE.*NEGOCIOS", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 3;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "GERENTE.*OPERA", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 3;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "DIRET.*OPERA", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 3;
            }
            else if (Regex.IsMatch(colaborador.CgoDescricao, "SUPERIN.*OPERA", RegexOptions.IgnoreCase))
            {
                mtaCodigo = 3;
            }
            else if (colaborador.CgoCodigo == 466)
            {
                mtaCodigo = 4;
            }
            else if (colaborador.CgoCodigo == 830)
            {
                mtaCodigo = 5;
            }
            else if (colaborador.CgoCodigo == 819)
            {
                mtaCodigo = 6;
            }
            else
            {
                mtaCodigo = -1;
            }

            return mtaCodigo;
        }
    }
}

