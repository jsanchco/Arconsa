namespace SGDE.Domain.Helpers
{
    #region Using

    using SGDE.Domain.ViewModels;
    using SGDE.Domain.Supervisor;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.IO;
    using System;
    using SGDE.Domain.Converters;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class GenerateInvoiceByWork : GenerateInvoice
    {
        public GenerateInvoiceByWork(Supervisor supervisor, InvoiceQueryViewModel invoiceQueryViewModel) : base(supervisor, invoiceQueryViewModel) { }

        protected List<DetailInvoiceViewModel> GetDetailInvoice()
        {
            var listReportResultViewModel = _supervisor.GetHoursByWork(new ReportQueryViewModel
            {
                startDate = _invoiceQueryViewModel.startDate,
                endDate = _invoiceQueryViewModel.endDate,
                workId = _invoiceQueryViewModel.workId
            });

            var listGroupedByProfessionId = listReportResultViewModel.GroupBy(x => new { x.professionId, x.hourTypeId })
                .Select(
                        x => new
                        {
                            Key = x.Key,
                            ProfessionName = x.Select(y => y.professionName).First(),
                            ProfessionId = x.Select(y => y.professionId).First(),
                            HourTypeId = x.Select(y => y.hourTypeId).First(),
                            HourTypeName = x.Select(y => y.hourTypeName).First(),
                            Hours = x.Sum(y => y.hours)
                        })
                .OrderBy(x => x.HourTypeId)
                .OrderBy(x => x.ProfessionId);

            foreach (var itemByProfession in listGroupedByProfessionId)
            {
                _invoiceQueryViewModel.detailInvoice.Add(new DetailInvoiceViewModel
                {
                    servicesPerformed = itemByProfession.HourTypeName,
                    priceUnity = GetPriceHourSale((int)itemByProfession.HourTypeId, (int)itemByProfession.ProfessionId),
                    units = itemByProfession.Hours
                });
            }

            return _invoiceQueryViewModel.detailInvoice;
        }

        protected override PdfPTable GetTableNumberInvoice()
        {
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 30, HorizontalAlignment = 2 };

            //var pdfCell = new PdfPCell(new Phrase("Nº Factura", _STANDARFONT_10_BOLD))
            //{
            //    BackgroundColor = new BaseColor(204, 204, 255),
            //    BorderWidthRight = 0,
            //    BorderWidthBottom = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(invoiceQueryViewModel.invoiceNumber, _STANDARFONT_10_BOLD))
            //{
            //    BackgroundColor = new BaseColor(204, 204, 255),
            //    BorderWidthLeft = 0,
            //    BorderWidthBottom = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase("Fecha", _STANDARFONT_10_BOLD))
            //{ BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(invoiceQueryViewModel.issueDate, _STANDARFONT_10_BOLD))
            //{ BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase("Nº Cliente", _STANDARFONT_10_BOLD))
            //{ BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0 };
            //pdfPTable.AddCell(pdfCell);

            //pdfCell = new PdfPCell(new Phrase(client.id.ToString(), _STANDARFONT_10_BOLD))
            //{ BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0 };
            //pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected override bool Validate()
        {
            if (_invoiceQueryViewModel.workId == null)
                return false;

            _work = _supervisor.GetWork((int)_invoiceQueryViewModel.workId);
            if (_work == null)
                return false;

            _invoiceQueryViewModel.detailInvoice = GetDetailInvoice();

            return true;
        }
    }
}
