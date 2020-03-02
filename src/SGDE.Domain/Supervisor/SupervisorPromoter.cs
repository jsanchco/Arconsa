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
        public List<PromoterViewModel> GetAllPromoter()
        {
            return PromoterConverter.ConvertList(_promoterRepository.GetAll());
        }

        public PromoterViewModel GetPromoterById(int id)
        {
            var promoterViewModel = PromoterConverter.Convert(_promoterRepository.GetById(id));

            return promoterViewModel;
        }

        public PromoterViewModel AddPromoter(PromoterViewModel newPromoterViewModel)
        {
            var promoter = new Promoter
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newPromoterViewModel.iPAddress,

                Name = newPromoterViewModel.name,
                Cif = newPromoterViewModel.cif,
                PhoneNumber = newPromoterViewModel.phoneNumber,
                Address = newPromoterViewModel.address
            };

            _promoterRepository.Add(promoter);
            return newPromoterViewModel;
        }

        public bool UpdatePromoter(PromoterViewModel promoterViewModel)
        {
            if (promoterViewModel.id == null)
                return false;

            var promoter = _promoterRepository.GetById((int)promoterViewModel.id);

            if (promoter == null) return false;

            promoter.ModifiedDate = DateTime.Now;
            promoter.IPAddress = promoterViewModel.iPAddress;

            promoter.Name = promoterViewModel.name;
            promoter.Cif = promoterViewModel.cif;
            promoter.PhoneNumber = promoterViewModel.phoneNumber;
            promoter.Address = promoterViewModel.address;

            return _promoterRepository.Update(promoter);
        }

        public bool DeletePromoter(int id)
        {
            return _promoterRepository.Delete(id);
        }
    }
}
