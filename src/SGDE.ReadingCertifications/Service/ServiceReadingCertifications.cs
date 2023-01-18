using ExcelDataReader;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SGDE.ReadingCertifications.Models;
using System.Data;
using System.IO;
using System.Text;

namespace SGDE.ReadingCertifications.Service
{
    public class ServiceReadingCertifications
    {
        public CertificarionXLS CertificarionXLS { get; } = new CertificarionXLS();

        public void ReadFile()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var filePath = "c:\\Temp\\CERT N°4 OFERTA 133.V1D.2022.xls";
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[0];
                var rows = dataTable.Rows;
                var numColumns = dataTable.Columns.Count;
                var orderReading = 0;
                foreach (DataRow row in rows)
                {
                    CertificarionXLS.AddPartida(new Partida(row, orderReading));
                    orderReading++;
                    //for (int col = 0; col < numColumns; col++)
                    //{
                    //    var cell = row[col].ToString();
                    //    System.Diagnostics.Debug.Write($"{cell};");
                    //}
                    //System.Diagnostics.Debug.WriteLine(string.Empty);
                }
            }
        }

        private void RemoveColumnsEmpties(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            var numColumns = dataTable.Columns.Count;

            foreach (DataRow row in rows)
            {
                var cell = row[0].ToString();
                if (!string.IsNullOrEmpty(cell))
                {

                }
            }
        }
    }
}
