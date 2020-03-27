﻿namespace SGDE.Domain.Supervisor
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

        public decimal Total = 0;

        public InvoiceViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            var invoiceViewModel = new InvoiceViewModel
            {
                fileName = $"Fact_{invoiceQueryViewModel.invoiceNumber}.pdf",
                typeFile = "application/pdf"
            };

            switch (invoiceQueryViewModel.typeInvoice)
            {
                case 1:
                    invoiceViewModel.file = GetInvoiceType1(invoiceQueryViewModel);
                    break;

                default:
                    invoiceViewModel.file = GetInvoiceType1(invoiceQueryViewModel);
                    break;
            }

            return invoiceViewModel;
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
                pdf.Add(GetPayment(client));
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

            var listGrouped = listReportResultViewModel.GroupBy(x => x.professionId)
                .Select(
                        x => new
                        {
                            Key = x.Key,
                            ProfessionName = x.Select(y => y.professionName),
                            Hours = x.Sum(y => y.hours)
                        });

            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 20f, 20f, 20f };
            pdfPTable.SetWidths(widths);

            Total = 0;
            foreach (var item in listGrouped)
            {
                decimal priceUnity = 0;
                decimal priceTotal = 0;
                var professionName = string.Empty;
                
                var professionInClient = work.Client.ProfessionInClients.FirstOrDefault(x => x.ProfessionId == item.Key);
                if (professionInClient != null)
                {
                    priceUnity = professionInClient.PriceHourSale;
                    priceTotal = professionInClient.PriceHourSale * (int)item.Hours;
                    professionName = professionInClient.Profession.Name;

                    Total += priceTotal;
                }

                var title = $"HORAS ORDINARIAS {professionName.ToUpper()}";
                var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.Hours.ToString(), _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(priceUnity.ToString(), _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(priceTotal.ToString(), _STANDARFONT_10))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingTop = 2f,
                    PaddingBottom = 6f,
                    BorderWidth = 0
                };
                pdfPTable.AddCell(pdfCell);
            }

            var countRows = listGrouped.Count();
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

        private PdfPTable GetPayment(ClientViewModel client)
        {
            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 20f, 30f, 20f, 30f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Método de Pago", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.wayToPay, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº de Cuenta", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(client.accountNumber, _STANDARFONT_10))
            { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Base Imponible", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase($"{Math.Round(Total, 2)} €", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("I.V.A.", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            var iva = Math.Round(decimal.ToDouble(Total) * 0.21, 2);
            pdfCell = new PdfPCell(new Phrase($"{iva} €", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Total Factura", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            var totalPlusIva = Math.Round(iva + decimal.ToDouble(Total), 2);
            pdfCell = new PdfPCell(new Phrase($"{totalPlusIva} €", _STANDARFONT_10)) { BorderWidth = 0 };
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
            pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_12_BOLD))
            {
                BorderWidthBottom = 0,
                BorderWidthTop = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            {
                BorderWidthBottom = 0,
                BorderWidthTop = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            {
                BorderWidthBottom = 0,
                BorderWidthTop = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            {
                BorderWidthBottom = 0,
                BorderWidthTop = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            {
                BorderWidthBottom = 0,
                BorderWidthTop = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_12_BOLD))
            {
                BorderWidthTop = 0,
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Chunk GetLineSeparator()
        {
            return new Chunk(
                new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.LightGray, Element.ALIGN_LEFT, 1));
        }
    }
}