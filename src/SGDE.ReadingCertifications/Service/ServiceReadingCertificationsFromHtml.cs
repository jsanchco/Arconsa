using HtmlAgilityPack;
using SGDE.ReadingCertifications.Models;
using System.IO;

namespace SGDE.ReadingCertifications.Service
{
    public class ServiceReadingCertificationsFromHtml
    {
        public CertificarionXLS CertificarionXLS { get; } = new CertificarionXLS();

        public void ReadFile()
        {
            var filePath = "c:\\Temp\\CERT N°4 OFERTA 133.V1D.2022.html";
            var html = File.ReadAllText(filePath);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var nodesDetailCertification = htmlDocument.DocumentNode.SelectNodes("//td/span[@class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-5' or @class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-6']");
            var nodeBase = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='crystalstyle']");

            foreach (var node in nodeBase.ChildNodes)
            {
                if (node.OuterHtml.Contains("Section6"))
                {

                }
            }
        }
    }
}
