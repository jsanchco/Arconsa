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

    public class GenerateInvoiceCustom : GenerateInvoice
    {
        public GenerateInvoiceCustom(Supervisor supervisor, InvoiceQueryViewModel invoiceQueryViewModel) : base(supervisor, invoiceQueryViewModel) { }

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

        protected override PdfPTable GetTableClient()
        {
            return base.GetTableClient();
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

            return true;
        }

        protected override PdfPTable GetAllRowsDetailInvoice(Document pdf)
        {
            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            if (_invoice.InvoiceToCancelId != null)
                return pdfPTable;

            var cont = 0;
            foreach (var detailInvoice in _invoice.DetailsInvoice)
            {
                if ((double)detailInvoice.PriceUnity == 0)
                {
                    if (cont != 0)
                    {
                        AddRowClearToDetailInvoice(pdfPTable);
                    }

                    var pdfCell = new PdfPCell(new Phrase(detailInvoice.ServicesPerformed, _STANDARFONT_10_BOLD))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingTop = 2f,
                        PaddingBottom = 6f,
                        BorderWidth = 0
                    };
                    pdfPTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingTop = 2f,
                        PaddingBottom = 6f,
                        BorderWidth = 0
                    };
                    pdfPTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingTop = 2f,
                        PaddingBottom = 6f,
                        BorderWidth = 0
                    };
                    pdfPTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingTop = 2f,
                        PaddingBottom = 6f,
                        BorderWidth = 0
                    };
                    pdfPTable.AddCell(pdfCell);
                }
                else
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
                cont++;
            }

            return pdfPTable;
        }
    }
}
