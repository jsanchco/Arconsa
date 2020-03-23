namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using IronPdf;
    using System.Text.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;

    #endregion

    public partial class Supervisor
    {
        public InvoiceViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var htmlToPdf = new HtmlToPdf();
            var html = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets\\html\\Invoice.html"));
            var pdf = htmlToPdf.RenderHtmlAsPdf(html);
            pdf.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "HtmlToPdfExample1.Pdf"));

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
            var companyData = GetSettingByName("COMPANY_DATA");
            if (companyData == null)
            {
                throw new Exception("No existen datos de tu Empresa para poder realizar la factura");
            }
            var jsonCompanyData = JObject.Parse(companyData.data);
            var html = GetHeaderMyCompany(jsonCompanyData);
            //var pdf = htmlToPdf.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
            var pdf = htmlToPdf.RenderHtmlAsPdf(html);

            var OutputPath = "HtmlToPDF.pdf";
            pdf.SaveAs(OutputPath);

            return pdf.BinaryData;
        }

        private string GetHeaderMyCompany(JObject jsonCompanyData)
        {
            var routeImage = Directory.GetCurrentDirectory() + "\\assets\\images\\arconsa.png";
            var header = 
                "<div class='row'>" +
                    "<div class='col-xs-2'>" +
                        "<a href='#'><img alt='Arconsa' src='" + routeImage + "'></a>" +
                    "</div>" +
                    "<div class='col-xs-offset-8 col-sm-2'>" +
                        "<div class='row' style='font-size: 12px;'>" +
                            "<div>" + jsonCompanyData["companyName"] + "</div>" +
                            "<div>" + jsonCompanyData["cif"] + "</div>" +
                            "<div>" + jsonCompanyData["address"] + "</div>" +
                            "<div>" + jsonCompanyData["phoneNumber"] + "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";

            return header;
        }

        private string GetHeaderClient(JObject jsonCompanyData)
        {
            var routeImage = Directory.GetCurrentDirectory() + "\\assets\\images\\arconsa.png";
            var header =
                "<div class='row'>" +
                    "<div class='col-xs-2'>" +
                        "<a href='#'><img alt='Arconsa' src='" + routeImage + "'></a>" +
                    "</div>" +
                    "<div class='col-xs-offset-8 col-sm-2'>" +
                        "<div class='row' style='font-size: 12px;'>" +
                            "<div>" + jsonCompanyData["companyName"] + "</div>" +
                            "<div>" + jsonCompanyData["cif"] + "</div>" +
                            "<div>" + jsonCompanyData["address"] + "</div>" +
                            "<div>" + jsonCompanyData["phoneNumber"] + "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";

            return header;
        }
    }
}
