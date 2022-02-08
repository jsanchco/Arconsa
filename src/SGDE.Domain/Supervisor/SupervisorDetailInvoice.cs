namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
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
    }
}
