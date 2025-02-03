using OfficeOpenXml;

namespace API_SIC_WEB_ANGULAR.Utility
{
    public class ExellUtility
    {
        public  ExellUtility()
        {
            // Defina a propriedade LicenseContext antes de usar o EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public async Task<byte[]> CriarExcelAsync(List<object> lista)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Planilha1");

            // Adicione cabeçalhos
            for (int i = 0; i < lista[0].GetType().GetProperties().Length; i++)
            {
                var headerName = lista[0].GetType().GetProperties()[i].Name.ToUpper();

                // Remove o prefixo "mop_" do nome da propriedade, se presente
                if (headerName.StartsWith("MOP_"))
                {
                    headerName = headerName.Substring(4);
                }

                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = headerName;

                // Define o estilo da célula do cabeçalho
                var headerStyle = headerCell.Style;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                headerStyle.Font.Color.SetColor(System.Drawing.Color.White);
                headerStyle.Font.Bold = true;
            }

            // Preencha os dados
            for (int i = 0; i < lista.Count; i++)
            {
                for (int j = 0; j < lista[i].GetType().GetProperties().Length; j++)
                {

                    var property = lista[i].GetType().GetProperties()[j];
                    var value = property.GetValue(lista[i]);
                    // Verifica se o valor é uma data e formata adequadamente
                    if (value is DateTime)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        worksheet.Cells[i + 2, j + 1].Value = value;
                    }


                    var cellStyle = worksheet.Cells[i + 2, j + 1].Style;
                    if (i % 2 == 0)
                    {
                        // Define a cor de fundo da célula como branco para linhas pares
                        cellStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        cellStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                    }
                    else
                    {
                        // Define a cor de fundo da célula como cinza claro para linhas ímpares
                        cellStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        cellStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                }
            }


            return package.GetAsByteArray();
        }

    }
}
