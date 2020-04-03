namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.Linq;
    using SGDE.Domain.Helpers;

    #endregion

    public partial class Supervisor
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

        public InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();

        public InvoiceResponseViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var invoiceResponseViewModel = new InvoiceResponseViewModel
            {
                typeFile = "application/pdf",
                data = new InvoiceViewModel
                {
                    startDate = invoiceQueryViewModel.startDate,
                    endDate = invoiceQueryViewModel.endDate
                }
            };
            SetInvoiceName(invoiceResponseViewModel);
            invoiceResponseViewModel.fileName = $"Fact_{invoiceResponseViewModel.data.name.Replace("/", "_")}.pdf";

            switch (invoiceQueryViewModel.typeInvoice)
            {
                case 1:
                    invoiceResponseViewModel.file = GetInvoiceType1(invoiceQueryViewModel);
                    break;

                default:
                    invoiceResponseViewModel.file = GetInvoiceType1(invoiceQueryViewModel);
                    break;
            }
            invoiceResponseViewModel.data.taxBase = InvoiceViewModel.taxBase;
            invoiceResponseViewModel.data.iva = InvoiceViewModel.iva;
            invoiceResponseViewModel.data.total = InvoiceViewModel.total;
            invoiceResponseViewModel.data.workId = (int)invoiceQueryViewModel.workId;

            AddInvoice(invoiceResponseViewModel.data);

            return invoiceResponseViewModel;
        }

        private byte[] GetInvoiceType1(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            try
            {
                var work = GetWorkById((int)invoiceQueryViewModel.workId);
                if (work == null)
                    throw new Exception("Factura mal configurada");

                var client = GetClientById(work.clientId);
                if (client == null)
                    throw new Exception("Factura mal configurada");

                var companyData = GetSettingByName("COMPANY_DATA");
                if (companyData == null)
                    throw new Exception("No existen datos de tu Empresa para poder realizar la factura");
                var jsonCompanyData = JObject.Parse(companyData.data);

                var pdf = new Document(PageSize.Letter);
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\assets\\pdf");
                var memoryStream = new MemoryStream();
                var pdfWriter = PdfWriter.GetInstance(pdf, memoryStream);

                pdf.AddTitle("Factura Cliente");
                pdf.AddCreator(_CREATOR);
                pdf.Open();

                pdf.Add(GetHeader(jsonCompanyData));
                pdf.Add(GetTable_NInvoice(invoiceQueryViewModel, client));
                pdf.Add(GetTableClient(client, work));
                pdf.Add(new Paragraph(" ", _STANDARFONT_8));
                pdf.Add(GetLineSeparator());
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle());
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitleInvoice(invoiceQueryViewModel));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetAllRowsDetailInvoice(invoiceQueryViewModel, pdf));
                pdf.Add(new Paragraph(" ", _STANDARFONT_8));
                pdf.Add(GetLineSeparator());
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle1("FORMA DE PAGO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetPayment(client, work));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetSignAndStamp());

                pdf.Close();
                pdfWriter.Close();

                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private PdfPTable GetHeader(JObject jsonCompanyData)
        {
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

        private PdfPTable GetTable_NInvoice(InvoiceQueryViewModel invoiceQueryViewModel, ClientViewModel client)
        {
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 30, HorizontalAlignment = 2 };

            var pdfCell = new PdfPCell(new Phrase("Nº Factura", _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255),
                BorderWidthRight = 0,
                BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(invoiceQueryViewModel.invoiceNumber, _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255),
                BorderWidthLeft = 0,
                BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Fecha", _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(invoiceQueryViewModel.issueDate, _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº Cliente", _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(client.id.ToString(), _STANDARFONT_10_BOLD))
            { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableClient(ClientViewModel client, WorkViewModel work)
        {
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };
            var widths = new[] { 15f, 85f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Empresa", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.name, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("CIF/NIF", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.cif, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Dirección", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.address, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Teléfono", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.phoneNumber, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Obra", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(work.name, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableTitle()
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

        private PdfPTable GetTableTitle1(string title)
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

        private PdfPTable GetTableTitleInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var listMonths = new[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
            var monthIni = DateTime.ParseExact(invoiceQueryViewModel.startDate, "d/MM/yyyy", null).Month;
            var monthEnd = DateTime.ParseExact(invoiceQueryViewModel.endDate, "d/MM/yyyy", null).Month;
            var yearIni = DateTime.ParseExact(invoiceQueryViewModel.startDate, "d/MM/yyyy", null).Year;
            var yearEnd = DateTime.ParseExact(invoiceQueryViewModel.endDate, "d/MM/yyyy", null).Year;

            var title = "";
            if (monthIni == monthEnd && yearIni == yearEnd)
            {
                title = $"HORAS POR ADMINISTRACION SEGÚN SERVICIOS PRESTADOS EN LA OBRA DE REFERENCIA CORRESPONIENTES AL MES DE {listMonths[monthIni - 1]} {yearIni}";
            }
            else
            {
                title = $"HORAS POR ADMINISTRACION SEGÚN SERVICIOS PRESTADOS EN LA OBRA DE REFERENCIA CORRESPONIENTES ENTRE LOS MESES DE {listMonths[monthIni - 1]} {yearIni} Y {listMonths[monthEnd - 1]} {yearEnd}";
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

        private PdfPTable GetAllRowsDetailInvoice(InvoiceQueryViewModel invoiceQueryViewModel, Document pdf)
        {
            var listReportResultViewModel = GetHoursByWork(new ReportQueryViewModel
            {
                startDate = invoiceQueryViewModel.startDate,
                endDate = invoiceQueryViewModel.endDate,
                workId = invoiceQueryViewModel.workId
            });

            var work = _workRepository.GetById((int)invoiceQueryViewModel.workId);

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

            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            InvoiceViewModel.taxBase = 0;
            foreach (var itemByProfession in listGroupedByProfessionId)
            {
                var priceUnity = GetPriceHourSale(work.Client, (int)itemByProfession.HourTypeId, (int)itemByProfession.ProfessionId);
                double priceTotal = (double)priceUnity * (double)itemByProfession.Hours;
                var professionName = itemByProfession.ProfessionName;
                InvoiceViewModel.taxBase += Convert.ToDecimal(priceTotal);

                var title = $"{itemByProfession.HourTypeName.ToString().ToUpper()} {professionName}";
                var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{itemByProfession.Hours.ToFormatSpain()} horas", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{((double)priceUnity).ToFormatSpain()} €", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase($"{priceTotal.ToFormatSpain()} €", _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

            }

            var countRows = listGroupedByProfessionId.Count();
            if (countRows > 5)
            {
                pdf.NewPage();
            }
            else
            {
                for (var i = 0; i < (4 - countRows); i++)
                {
                    var pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10))
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
            }

            return pdfPTable;
        }

        private PdfPTable GetPayment(ClientViewModel client, WorkViewModel work)
        {
            var pdfPTable = new PdfPTable(5) { WidthPercentage = 100 };
            var widths = new[] { 20f, 20f, 10f, 20f, 30f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Método de Pago", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.wayToPay, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº de Cuenta", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.accountNumber, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Base Imponible", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{Math.Round(Convert.ToDouble(InvoiceViewModel.taxBase), 2).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            InvoiceViewModel.iva = Convert.ToDecimal(Math.Round(Convert.ToDouble(InvoiceViewModel.taxBase) * 0.21, 2));
            if (!work.passiveSubject)
            {
                pdfCell = new PdfPCell(new Phrase("I.V.A.", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase($"{Convert.ToDouble(InvoiceViewModel.iva).ToFormatSpain()} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
                pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
            }
            else
            {
                InvoiceViewModel.iva = 0;
                //pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
                //pdfPTable.AddCell(pdfCell);
                //pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
                //pdfPTable.AddCell(pdfCell);
                //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                //pdfPTable.AddCell(pdfCell);
                //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                //pdfPTable.AddCell(pdfCell);
                //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
                //pdfPTable.AddCell(pdfCell);
            }

            pdfCell = new PdfPCell(new Phrase("Total Factura", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            InvoiceViewModel.total = Convert.ToDecimal(Math.Round(Convert.ToDouble(InvoiceViewModel.iva) + Convert.ToDouble(InvoiceViewModel.taxBase), 2).ToFormatSpain());
            pdfCell = new PdfPCell(new Phrase($"{InvoiceViewModel.total} €", _STANDARFONT_10)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetSignAndStamp()
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

            //pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthBottom = 0,
            //    BorderWidthTop = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthBottom = 0,
            //    BorderWidthTop = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthBottom = 0,
            //    BorderWidthTop = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthBottom = 0,
            //    BorderWidthTop = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthBottom = 0,
            //    BorderWidthTop = 0
            //};
            //pdfPTable.AddCell(pdfCell);
            //pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            //{
            //    BorderWidthTop = 0,
            //};
            //pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Chunk GetLineSeparator()
        {
            return new Chunk(
                new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.LightGray, Element.ALIGN_LEFT, 1));
        }

        private decimal GetPriceHourSale(Client client, int? type, int? professionId)
        {
            if (type == null || professionId == null)
                return 0;

            var professionInClient = client.ProfessionInClients.FirstOrDefault(x => x.ProfessionId == professionId);
            if (professionInClient == null)
                return 0;

            switch (type)
            {
                case 1:
                    return professionInClient.PriceHourSaleOrdinary;
                case 2:
                    return professionInClient.PriceHourSaleExtra;
                case 3:
                    return professionInClient.PriceHourSaleFestive;

                default:
                    return 0;
            }
        }

        private void SetInvoiceName(InvoiceResponseViewModel invoiceResponseViewModel)
        {
            var countInvoice = _invoiceRepository.GetAll().Count;
            var invoiceNumber = countInvoice == 0 ? 1063 : (1063 + countInvoice);

            invoiceNumber++;

            invoiceResponseViewModel.data.invoiceNumber = (int)invoiceNumber;
            invoiceResponseViewModel.data.name = $"{invoiceNumber}/{DateTime.ParseExact(invoiceResponseViewModel.data.startDate, "dd/MM/yyyy", null).Year.ToString().Substring(2, 2)}";
        }
    }
}
