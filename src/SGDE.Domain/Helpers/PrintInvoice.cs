namespace SGDE.Domain.Helpers
{
    #region Using

    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using SGDE.Domain.Supervisor;
    using System;
    using System.IO;

    #endregion

    public class PrintInvoice : GenerateInvoice
    {
        public PrintInvoice(Supervisor supervisor, int invoiceId) : base(supervisor, invoiceId) { }

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

                    pdfCell = new PdfPCell(new Phrase($"{Math.Round(detailInvoice.Units * detailInvoice.PriceUnity, 2).ToFormatSpain()} €", _STANDARFONT_10))
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

            if (_invoice.Work.Client.ExpirationDays != 0)
            {
                pdfCell = new PdfPCell(new Phrase("Vencimiento", _STANDARFONT_10_BOLD))
                { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(_invoice.IssueDate.AddDays(_invoice.Work.Client.ExpirationDays).ToString("dd/MM/yyyy"), _STANDARFONT_10_BOLD))
                { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
                pdfPTable.AddCell(pdfCell);
            }

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
            _invoice = _supervisor.GetInvoice(_invoiceId);
            if (_invoice == null)
                return false;

            _work = _invoice.Work;
            if (_work == null)
                return false;

            _client = _work.Client;
            if (_client == null)
                return false;

            return true;
        }

        public void Print()
        {
            if (!Validate())
                throw new Exception("No se puede validar la Factura");

            _pdf = new Document(PageSize.Letter);

            var memoryStream = new MemoryStream();
            var pdfWriter = PdfWriter.GetInstance(_pdf, memoryStream);

            _pdf.AddTitle("Factura Cliente");
            _pdf.AddCreator(_CREATOR);
            _pdf.Open();

            _pdf.Add(GetHeader());
            _pdf.Add(GetTableNumberInvoice());
            _pdf.Add(GetTableClient());
            _pdf.Add(new Paragraph(" ", _STANDARFONT_8));
            _pdf.Add(GetLineSeparator());
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetTableTitle());
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetTableTitleInvoice());
            //_pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetAllRowsDetailInvoice(_pdf));
            _pdf.Add(new Paragraph(" ", _STANDARFONT_8));
            _pdf.Add(GetLineSeparator());
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetTableTitle1("FORMA DE PAGO"));
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetPayment());
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
            _pdf.Add(GetSignAndStamp());

            _pdf.Close();
            pdfWriter.Close();

            _invoiceResponseViewModel.file = memoryStream.ToArray();
            _invoiceResponseViewModel.fileName = $"Fact_{_invoice.Name.Replace("/", "_")}.pdf";
        }
    }
}
