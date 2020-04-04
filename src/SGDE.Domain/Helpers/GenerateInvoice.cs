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

        public InvoiceViewModel _invoiceViewModel = new InvoiceViewModel();
        public InvoiceResponseViewModel _invoiceResponseViewModel = new InvoiceResponseViewModel();
        protected Document _pdf = new Document(PageSize.Letter);
        protected Client _client;
        protected Work _work;
        protected User _worker;

        protected InvoiceQueryViewModel _invoiceQueryViewModel;
        protected ISupervisor _supervisor;
        
        public GenerateInvoice(ISupervisor supervisor, InvoiceQueryViewModel invoiceQueryViewModel)
        {
            _supervisor = supervisor;
            _invoiceQueryViewModel = invoiceQueryViewModel;
        }

        public void Process()
        {
            if (!Validate())
                throw new Exception("No se puede validar la Factura");



            _pdf.AddTitle("Factura Cliente");
            _pdf.AddCreator(_CREATOR);
            _pdf.Open();

            _pdf.Add(GetHeader());
            _pdf.Add(GetTableNumberInvoice());
        }

        protected abstract bool Validate();

        private PdfPTable GetHeader()
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

        protected abstract PdfPTable GetTableNumberInvoice();

        protected PdfPTable GetTableClient()
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
            return null;
        }
    }
}
