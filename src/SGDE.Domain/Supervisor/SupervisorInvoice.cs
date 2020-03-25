namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using IronPdf;
    using Newtonsoft.Json.Linq;
    using System.IO;

    //using TheArtOfDev.HtmlRenderer.PdfSharp;
    //using PdfSharp;

    #endregion

    public partial class Supervisor
    {
        public InvoiceViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var htmlToPdf = new HtmlToPdf();

            var invoiceViewModel = new InvoiceViewModel
            {
                fileName = $"Fact_{invoiceQueryViewModel.invoiceNumber}.pdf",
                typeFile = "application/pdf"
            };

            switch (invoiceQueryViewModel.typeInvoice)
            {
                case 1:
                    invoiceViewModel.file = GetInvoiceType1(invoiceQueryViewModel, htmlToPdf);
                    break;

                default:
                    invoiceViewModel.file = GetInvoiceType1(invoiceQueryViewModel, htmlToPdf);
                    break;
            }

            return invoiceViewModel;
        }

        private byte[] GetInvoiceType1(InvoiceQueryViewModel invoiceQueryViewModel, HtmlToPdf htmlToPdf)
        {
            var html = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets\\html\\Invoice.html"));

            var listKeyWord = GetKeyWords(html);

            var dictionaryKeyWords = new Dictionary<string, string>();
            GetDictionaryMyCompany(dictionaryKeyWords);
            GetDictionaryClient(dictionaryKeyWords, invoiceQueryViewModel);

            foreach (var keyWord in listKeyWord)
            {
                if (!dictionaryKeyWords.TryGetValue(keyWord, out string value))
                    continue;

                html = html.Replace($"[{keyWord}]", dictionaryKeyWords[keyWord]);
            }

            //CreatePDF(html);

            var pdf = htmlToPdf.RenderHtmlAsPdf(html);

            return pdf.BinaryData;
        }

        private List<string> GetKeyWords(string fileAsString)
        {
            var listKeyWords = new List<string>();

            var findOpen = 0;
            while (findOpen != -1)
            {
                findOpen = fileAsString.IndexOf("[", findOpen);
                if (findOpen == -1)
                    break;

                var findClose = fileAsString.IndexOf("]", findOpen);
                var keyWord = fileAsString.Substring(findOpen + 1, (findClose - findOpen) - 1);

                listKeyWords.Add(keyWord);

                if (findOpen != -1) findOpen++;
            }

            return listKeyWords;
        }

        private void GetDictionaryMyCompany(Dictionary<string, string> dictionaryKeyWords)
        {
            var companyData = GetSettingByName("COMPANY_DATA");
            if (companyData == null)
            {
                throw new Exception("No existen datos de tu Empresa para poder realizar la factura");
            }
            var jsonCompanyData = JObject.Parse(companyData.data);

            dictionaryKeyWords.Add("routeImage", Directory.GetCurrentDirectory() + "\\assets\\images\\arconsa.png");
            dictionaryKeyWords.Add("companyName", jsonCompanyData["companyName"].ToString());
            dictionaryKeyWords.Add("cif", jsonCompanyData["cif"].ToString());
            dictionaryKeyWords.Add("address", jsonCompanyData["address"].ToString());
            dictionaryKeyWords.Add("phoneNumber", jsonCompanyData["phoneNumber"].ToString());
        }

        private void GetDictionaryClient(Dictionary<string, string> dictionaryKeyWords, InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var work = GetWorkById((int)invoiceQueryViewModel.workId);
            if (work == null)
                return;
            var client = GetClientById(work.clientId);
            if (work == null)
                return;

            dictionaryKeyWords.Add("clientName", client.name);
            dictionaryKeyWords.Add("clientAddress", client.address);
            dictionaryKeyWords.Add("clientPhoneNumber", client.phoneNumber);
            dictionaryKeyWords.Add("clientWorkName", work.name);
        }

        //private void CreatePDF(string html)
        //{
        //    //using (System.IO.StreamReader Reader = new System.IO.StreamReader(@"D:\test.html"))
        //    {
        //        using (FileStream stream = new FileStream(@"D:\test.pdf", FileMode.Create))
        //        {
        //            Document pdfDoc = new Document(PageSize.A4);
        //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            StringReader sr = new StringReader(html);
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //            pdfDoc.Close();
        //            stream.Close();
        //        }
        //    }
        //}
    }
}
