namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using IronPdf;

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
            //var OutputPath = "HtmlToPDF.pdf";
            //pdf.SaveAs(OutputPath);

            return invoiceViewModel;
        }

        private byte[] GetInvoiceType1(InvoiceQueryViewModel invoiceQueryViewModel, HtmlToPdf htmlToPdf)
        {
            var pdf = htmlToPdf.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");

            return pdf.BinaryData;
        }
    }
}
