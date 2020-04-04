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
        public List<DetailInvoiceViewModel> GetAllDetailInvoice()
        {
            return DetailInvoiceConverter.ConvertList(_detailInvoiceRepository.GetAll());
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
                PriceUnity = (decimal)newDetailInvoiceViewModel.priceUnity
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
            detailInvoice.PriceUnity = (decimal)detailInvoiceViewModel.priceUnity;

            return _detailInvoiceRepository.Update(detailInvoice);
        }

        public bool DeleteDetailInvoice(int id)
        {
            return _detailInvoiceRepository.Delete(id);
        }
    }
}
