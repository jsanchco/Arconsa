using ExcelDataReader;
using System.Data;
using System.IO;

namespace SGDE.ReadingCertifications
{
    public class ServiceReadingCertifications
    {
        public void ReadFile()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var filePath = "c:\\Temp\\CERT N°4 OFERTA 133.V1D.2022.xls";
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                //var conf = new ExcelDataSetConfiguration
                //{
                //    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                //    {
                //        UseHeaderRow = true
                //    }
                //};

                var dataSet = reader.AsDataSet();

                var dataTable = dataSet.Tables[0];
                var rows = dataTable.Rows;
                var numColumns = dataTable.Columns.Count;
                foreach (DataRow row in rows)
                {
                    for (int col = 0; col < numColumns; col++)
                    {
                        var cell = row[col].ToString();
                        System.Diagnostics.Debug.Write($"{cell};");
                    }
                    System.Diagnostics.Debug.WriteLine(string.Empty);
                }
            }
        }
    }
}
