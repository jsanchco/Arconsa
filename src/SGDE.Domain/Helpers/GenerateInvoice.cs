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
    using SGDE.Domain.Entities;
    using System.Linq;

    #endregion

    public abstract class GenerateInvoice
    {
        public const string _CREATOR = "Servicio de Report (ARCONSA)";
        public static readonly Font _STANDARFONT_10 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        public static readonly Font _STANDARFONT_8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);
        public static readonly Font _STANDARFONT_10_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

        public static readonly Font _STANDARFONT_10_BOLD_CUSTOMCOLOR =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(144, 54, 25));

        public static readonly Font _STANDARFONT_12_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        public static readonly Font _STANDARFONT_14_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);

        public static readonly Font _STANDARFONT_12_BOLD_WHITE =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.White);
        public static readonly Font _STANDARFONT_14_BOLD_WHITE =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.White);

        public InvoiceViewModel _invoiceViewModel;
        protected Document _pdf;
        public InvoiceResponseViewModel _invoiceResponseViewModel;

        protected Client _client;
        protected Work _work;
        protected User _worker;
        protected Invoice _invoice;

        protected InvoiceQueryViewModel _invoiceQueryViewModel;
        protected ISupervisor _supervisor;
        protected int _invoiceId;
        
        public GenerateInvoice(ISupervisor supervisor, InvoiceQueryViewModel invoiceQueryViewModel)
        {
            _supervisor = supervisor;
            _invoiceQueryViewModel = invoiceQueryViewModel;

            _invoiceResponseViewModel = new InvoiceResponseViewModel
            {
                typeFile = "application/pdf",
                data = new InvoiceViewModel
                {
                    startDate = invoiceQueryViewModel.startDate,
                    endDate = invoiceQueryViewModel.endDate
                },
                typeInvoiceId = invoiceQueryViewModel.typeInvoice
            };
        }

        public GenerateInvoice(ISupervisor supervisor, int invoiceId)
        {
            _supervisor = supervisor;
            _invoiceId = invoiceId;

            _invoiceResponseViewModel = new InvoiceResponseViewModel
            {
                typeFile = "application/pdf"
            };
        }

        protected abstract bool Validate();
        protected abstract PdfPTable GetAllRowsDetailInvoice(Document pdf);
        protected abstract PdfPTable GetTableNumberInvoice();

        public void Process()
        {
            if (!Validate())
                throw new Exception("No se puede validar la Factura");

            _invoice = AddInvoice();

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
            _pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
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

            FillDataResponse();
        }

        protected PdfPTable GetHeader()
        {
            var companyData = _supervisor.GetSettingByName("COMPANY_DATA");
            if (companyData == null)
                throw new Exception("No existen datos de tu Empresa para poder realizar la factura");
            var jsonCompanyData = JObject.Parse(companyData.data);

            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };

            var image = Image.GetInstance(Directory.GetCurrentDirectory() + "\\assets\\images\\arconsa.png");
            image.ScalePercent(75f);
            var pdfCellImage = new PdfPCell(image)
            {
                Rowspan = 4,
                BorderWidth = 0,
                PaddingLeft = 20
            };
            pdfPTable.AddCell(pdfCellImage);

            var pdfCell = new PdfPCell(new Phrase(jsonCompanyData["companyName"].ToString(), _STANDARFONT_12_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(jsonCompanyData["cif"].ToString(), _STANDARFONT_12_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(jsonCompanyData["address"].ToString(), _STANDARFONT_12_BOLD))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(jsonCompanyData["phoneNumber"].ToString(), _STANDARFONT_12_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected virtual PdfPTable GetTableClient()
        {
            if (_client == null)
                throw new Exception("No existen datos para este Cliente");

            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };
            var widths = new[] { 15f, 85f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Empresa", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.Name, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("CIF/NIF", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.Cif, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Dirección", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.Address, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Teléfono", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.PhoneNumber, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Obra", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected double GetPriceHourSale(int? type, int? professionId)
        {
            if (type == null || professionId == null)
                return 0;

            var professionInClient = _client.ProfessionInClients.FirstOrDefault(x => x.ProfessionId == professionId);
            if (professionInClient == null)
                return 0;

            switch (type)
            {
                case 1:
                    return (double)professionInClient.PriceHourSaleOrdinary;
                case 2:
                    return (double)professionInClient.PriceHourSaleExtra;
                case 3:
                    return (double)professionInClient.PriceHourSaleFestive;

                default:
                    return 0;
            }
        }

        protected Invoice AddInvoice()
        {
            var invoice = _supervisor.AddInvoiceFromQuery(_invoiceQueryViewModel);
            return invoice;
        }

        protected PdfPTable GetTableTitle()
        {
            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("SERVICIOS REALIZADOS", _STANDARFONT_12_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("UNIDAD", _STANDARFONT_12_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("PRECIO UNIDAD", _STANDARFONT_12_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("IMPORTE", _STANDARFONT_12_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected Chunk GetLineSeparator()
        {
            return new Chunk(
                new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.LightGray, Element.ALIGN_LEFT, 1));
        }

        protected PdfPTable GetTableTitleInvoice()
        {
            var listMonths = new[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
            var monthIni = _invoice.StartDate.Month;
            var monthEnd = _invoice.EndDate.Month;
            var yearIni = _invoice.StartDate.Year;
            var yearEnd = _invoice.EndDate.Year;

            var title = string.Empty;
            if (_invoice.InvoiceToCancelId == null)
            {
                if (_invoice.TypeInvoice == 1)
                {
                    if (monthIni == monthEnd && yearIni == yearEnd)
                    {
                        title = $"HORAS POR ADMINISTRACION SEGÚN SERVICIOS PRESTADOS EN LA OBRA DE REFERENCIA CORRESPONIENTES AL MES DE {listMonths[monthIni - 1]} {yearIni}";
                    }
                    else
                    {
                        title = $"HORAS POR ADMINISTRACION SEGÚN SERVICIOS PRESTADOS EN LA OBRA DE REFERENCIA CORRESPONIENTES ENTRE LOS MESES DE {listMonths[monthIni - 1]} {yearIni} Y {listMonths[monthEnd - 1]} {yearEnd}";
                    }
                }
            }
            else
            {
                var invoiceParent = _supervisor.GetInvoice((int)_invoice.InvoiceToCancelId);
                title = $"FACTURA ABONO CORRESPONDIENTE A LA FACTURA Nº {invoiceParent.Name}";
            }

            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_10))
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
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
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
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected PdfPTable GetTableTitle1(string title)
        {
            var pdfPTable = new PdfPTable(1) { WidthPercentage = 100 };
            var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_14_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected PdfPTable GetPayment()
        {
            var pdfPTable = new PdfPTable(5) { WidthPercentage = 100 };
            var widths = new[] { 20f, 20f, 10f, 20f, 30f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Método de Pago", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.WayToPay, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº de Cuenta", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(_client.AccountNumber, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Base Imponible", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{Math.Round((double)_invoice.TaxBase, 2).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            if (!_work.PassiveSubject)
            {
                pdfCell = new PdfPCell(new Phrase("I.V.A.", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase($"{_invoice.IvaTaxBase.ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
            }

            var total = GetInvoceToOrigen(pdfPTable);
            total = _invoice.Total - total;

            pdfCell = new PdfPCell(new Phrase("Total Factura", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{total.ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        protected PdfPTable GetSignAndStamp()
        {
            var pdfPTable = new PdfPTable(1) { WidthPercentage = 40, HorizontalAlignment = 2 };

            var pdfCell = new PdfPCell(new Phrase("Firma y Sello", _STANDARFONT_12_BOLD))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            var image = Image.GetInstance(Directory.GetCurrentDirectory() + "\\assets\\images\\FirmAndSign.png");
            image.ScalePercent(50);
            var pdfCellImage = new PdfPCell(image)
            {
                BorderWidthTop = 0,
                PaddingTop = 2,
                PaddingBottom = 2,
                PaddingRight = 2,
                PaddingLeft = 30,
            };
            pdfPTable.AddCell(pdfCellImage);

            return pdfPTable;
        }

        private void FillDataResponse()
        {
            _invoiceResponseViewModel.data.name = _invoice.Name;
            _invoiceResponseViewModel.data.invoiceNumber = _invoice.InvoiceNumber;
            _invoiceResponseViewModel.data.taxBase = (double)_invoice.TaxBase;
            _invoiceResponseViewModel.data.ivaTaxBase = _invoice.IvaTaxBase;
            _invoiceResponseViewModel.data.retentions = _invoice.Retentions;
            _invoiceResponseViewModel.data.startDate = _invoice.StartDate.ToString("dd/MM/yyyy");
            _invoiceResponseViewModel.data.endDate = _invoice.EndDate.ToString("dd/MM/yyyy");
            _invoiceResponseViewModel.data.issueDate = _invoice.IssueDate.ToString("dd/MM/yyyy");
            _invoiceResponseViewModel.data.iva = _invoice.Iva;
            _invoiceResponseViewModel.data.total = _invoice.Total;
            _invoiceResponseViewModel.data.workId = _invoice.WorkId;
            _invoiceResponseViewModel.data.clientId = _invoice.ClientId;
            _invoiceResponseViewModel.data.userId = _invoice.UserId;
        }

        private double GetInvoceToOrigen(PdfPTable pdfPTable)
        {
            if (!_work.InvoiceToOrigin)
                return 0;

            PdfPCell pdfCell;
            if (_work.TotalContract != 0)
            {
                pdfCell = new PdfPCell(new Phrase("Total Contrato", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase($"{Math.Round((double)_work.TotalContract, 2).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
            }

            var sumInvoices = _work.Invoices.Where(x => x.Id != _invoice.Id).Select(x => x.TaxBase).Sum();
            var result = sumInvoices + _invoice.TaxBase;

            pdfCell = new PdfPCell(new Phrase("Certificación a Origen", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{Math.Round((double)result, 2).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Certificación Anterior", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{Math.Round((double)sumInvoices, 2).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            var percentageTaxBase = ((double)_invoice.TaxBase) * ((double)_work.PercentageRetention);
            var titlePercentageTaxBase = percentageTaxBase == 0 ? "-" : percentageTaxBase.ToFormatSpain();

            pdfCell = new PdfPCell(new Phrase($"{Math.Round((double)_work.PercentageRetention * 100)}% de Retención", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{titlePercentageTaxBase} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return percentageTaxBase;
        }
    }
}
