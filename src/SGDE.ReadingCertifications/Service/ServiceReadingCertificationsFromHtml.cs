using HtmlAgilityPack;
using SGDE.ReadingCertifications.Extensions;
using SGDE.ReadingCertifications.Models;
using System.IO;
using System.Linq;

namespace SGDE.ReadingCertifications.Service
{
    public class ServiceReadingCertificationsFromHtml
    {
        public CertificarionXLS CertificarionXLS { get; } = new CertificarionXLS();

        public void ReadFile()
        {
            var filePath = "c:\\Temp\\CERT N°4 OFERTA 133.V1D.2022_1.html";
            var html = File.ReadAllText(filePath);
            html = System.Web.HttpUtility.HtmlDecode(html);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // CAPITULOS
            var nodesDetailCertification = htmlDocument.DocumentNode.SelectNodes("//td/span[@class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-5' or @class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-6']");
            var subLists = nodesDetailCertification.ToList().ChunkBy(7);
            subLists = subLists.Where(x => x.Count == 7).ToList();
            foreach (var list in subLists)
            {
                var partida = new Partida(list);
                CertificarionXLS.AddPartida(partida);
            }

            // SUBCAPITULOS
            nodesDetailCertification = htmlDocument.DocumentNode.SelectNodes("//td/span[@class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-7' or @class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-8' or @class='fc1538a29d-f953-4d11-a6b0-46c28732c2d0-9']");
            subLists = nodesDetailCertification.ToList().ChunkBy(14);
            subLists = subLists.Where(x => x.Count == 14).ToList();
            foreach (var list in subLists)
            {
                var partida = new Partida(list);
                CertificarionXLS.AddPartida(partida);
            }
        }
    }
}
