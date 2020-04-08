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

        public List<DetailInvoiceViewModel> GetDetailInvoice()
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
                    servicesPerformed = $"{itemByProfession.HourTypeName.ToString()} {itemByProfession.ProfessionName}",
                    priceUnity = GetPriceHourSale((int)itemByProfession.HourTypeId, (int)itemByProfession.ProfessionId),
                    units = itemByProfession.Hours,
                    nameUnit = "horas"
                });
            }

            return _invoiceQueryViewModel.detailInvoice;
        }

        protected override PdfPTable GetTableNumberInvoice()
        {
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 30, HorizontalAlignment = 2 };

            var pdfCell = new PdfPCell(new Phrase("Nº Factura", _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255),
                BorderWidthRight = 0,
                BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_invoice.Name, _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255),
                BorderWidthLeft = 0,
                BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Fecha", _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_invoice.IssueDate.ToString("dd/MM/yyyy"), _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº Cliente", _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);

            var clientNameCustom = _client.Cif;
            clientNameCustom = string.IsNullOrEmpty(clientNameCustom)
                ? _client.Id.ToString()
                : clientNameCustom.Substring(clientNameCustom.Length - 5, 5);

            pdfCell = new PdfPCell(new Phrase(clientNameCustom, _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        public override bool Validate()
        {
            if (_invoiceQueryViewModel.workId == null)
                return false;

            _work = _supervisor.GetWork((int)_invoiceQueryViewModel.workId);
            if (_work == null)
                return false;

            _client = _work.Client;
            if (_client == null)
                return false;

            _invoiceQueryViewModel.detailInvoice = GetDetailInvoice();

            return true;
        }

        protected override PdfPTable GetTableClient()
        {
            return base.GetTableClient();
        }

        protected override PdfPTable GetAllRowsDetailInvoice(Document pdf)
        {
            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            if (_invoice.InvoiceToCancelId != null)
                return pdfPTable;

            foreach (var detailInvoice in _invoice.DetailsInvoice)
            {
                var pdfCell = new PdfPCell(new Phrase(detailInvoice.ServicesPerformed, _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{((double)detailInvoice.Units).ToFormatSpain()} {detailInvoice.NameUnit}", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{((double)detailInvoice.PriceUnity).ToFormatSpain()} €", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{detailInvoice.Total.ToFormatSpain()} €", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

            }

            //var countRows = _invoice.DetailsInvoice.Count();
            //if (countRows > 5)
            //{
            //    pdf.NewPage();
            //}
            //else
            //{
            //    for (var i = 0; i < (4 - countRows); i++)
            //    {
            //        var pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
            //        {
            //            HorizontalAlignment = Element.ALIGN_RIGHT,
            //            VerticalAlignment = Element.ALIGN_MIDDLE,
            //            PaddingTop = 2f,
            //            PaddingBottom = 6f,
            //            BorderWidth = 0
            //        };
            //        pdfPTable.AddCell(pdfCell);
            //        pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
            //        {
            //            HorizontalAlignment = Element.ALIGN_RIGHT,
            //            VerticalAlignment = Element.ALIGN_MIDDLE,
            //            PaddingTop = 2f,
            //            PaddingBottom = 6f,
            //            BorderWidth = 0
            //        };
            //        pdfPTable.AddCell(pdfCell);
            //        pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
            //        {
            //            HorizontalAlignment = Element.ALIGN_RIGHT,
            //            VerticalAlignment = Element.ALIGN_MIDDLE,
            //            PaddingTop = 2f,
            //            PaddingBottom = 6f,
            //            BorderWidth = 0
            //        };
            //        pdfPTable.AddCell(pdfCell);
            //        pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
            //        {
            //            HorizontalAlignment = Element.ALIGN_RIGHT,
            //            VerticalAlignment = Element.ALIGN_MIDDLE,
            //            PaddingTop = 2f,
            //            PaddingBottom = 6f,
            //            BorderWidth = 0
            //        };
            //        pdfPTable.AddCell(pdfCell);
            //    }
            //}

            return pdfPTable;
        }
    }
}
