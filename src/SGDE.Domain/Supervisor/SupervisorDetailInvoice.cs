namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Converters;
    using Entities;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<DetailInvoiceViewModel> GetAllDetailInvoice(int invoiceId = 0, bool previousInvoice = false)
        {
            return DetailInvoiceConverter.ConvertList(_detailInvoiceRepository.GetAll(invoiceId));
        }

        public DetailInvoiceViewModel GetDetailInvoiceById(int id)
        {
            var detailInvoiceViewModel = DetailInvoiceConverter.Convert(_detailInvoiceRepository.GetById(id));

            return detailInvoiceViewModel;
        }

        public DetailInvoiceViewModel AddDetailInvoice(DetailInvoiceViewModel newDetailInvoiceViewModel)
        {
            var detailInvoice = new DetailInvoice
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newDetailInvoiceViewModel.iPAddress,
                
                InvoiceId = newDetailInvoiceViewModel.invoiceId,
                ServicesPerformed = newDetailInvoiceViewModel.servicesPerformed,
                Units = (decimal)newDetailInvoiceViewModel.units,
                UnitsAccumulated = (decimal)newDetailInvoiceViewModel.unitsAccumulated,
                PriceUnity = (decimal)newDetailInvoiceViewModel.priceUnity,
                NameUnit = newDetailInvoiceViewModel.nameUnit
            };

            _detailInvoiceRepository.Add(detailInvoice);
            return newDetailInvoiceViewModel;
        }

        public bool UpdateDetailInvoice(DetailInvoiceViewModel detailInvoiceViewModel)
        {
            if (detailInvoiceViewModel.id == null)
                return false;

            var detailInvoice = _detailInvoiceRepository.GetById((int)detailInvoiceViewModel.id);

            if (detailInvoice == null) return false;

            detailInvoice.ModifiedDate = DateTime.Now;
            detailInvoice.IPAddress = detailInvoiceViewModel.iPAddress;

            detailInvoice.InvoiceId = detailInvoiceViewModel.invoiceId;
            detailInvoice.ServicesPerformed = detailInvoiceViewModel.servicesPerformed;
            detailInvoice.Units = (decimal)detailInvoiceViewModel.units;
            detailInvoice.UnitsAccumulated = (decimal)detailInvoiceViewModel.unitsAccumulated;
            detailInvoice.PriceUnity = (decimal)detailInvoiceViewModel.priceUnity;
            detailInvoice.NameUnit = detailInvoiceViewModel.nameUnit;

            return _detailInvoiceRepository.Update(detailInvoice);
        }

        public bool DeleteDetailInvoice(int id)
        {
            return _detailInvoiceRepository.Delete(id);
        }

        public List<DetailInvoiceViewModel> GetDetailInvoiceFromPreviousInvoice(int invoiceId)
        {
            return DetailInvoiceConverter.ConvertList(_detailInvoiceRepository.UpdateFromPreviousInvoice(invoiceId));
        }

        public List<DetailInvoiceViewModel> GetDetailInvoiceFromWork(int invoiceId)
        {
            var invoice = GetInvoice(invoiceId);
            if (invoice == null)
                throw new Exception("Factura no encontrada");

            var listReportResultViewModel = GetHoursByWork(new ReportQueryViewModel
            {
                startDate = invoice.StartDate,
                endDate = invoice.EndDate,
                workId = invoice.WorkId
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

            var result = _detailInvoiceRepository.UpdateFromWork(
                invoiceId,
                listGroupedByProfessionId.Select(x =>
                new DetailInvoice 
                {
                    ServicesPerformed = $"{x.HourTypeName} {x.ProfessionName}",
                    PriceUnity = (decimal)GetPriceHourSale(x.HourTypeId, x.ProfessionId),
                    Units = (decimal)x.Hours,
                    NameUnit = "horas"
                }).ToList());

            return DetailInvoiceConverter.ConvertList(result);
        }

        public List<DetailInvoiceViewModel> GetEmptyDetails(int invoiceId)
        {
            return DetailInvoiceConverter.ConvertList(_detailInvoiceRepository.UpdateToEmptyDetails(invoiceId));
        }

        #region Auxiliary Methods

        private double GetPriceHourSale(int? type, int? professionId)
        {
            if (type == null || professionId == null)
                return 0;

            var professionInClient = GetProfessionInClientById(professionId.Value);
            if (professionInClient == null)
                return 0;

            switch (type)
            {
                case 1:
                    return (double)professionInClient.priceHourSaleOrdinary;
                case 2:
                    return (double)professionInClient.priceHourSaleExtra;
                case 3:
                    return (double)professionInClient.priceHourSaleFestive;

                default:
                    return 0;
            }
        }

        #endregion
    }
}
