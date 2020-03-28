namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using Domain.Helpers;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<ProfessionInClientViewModel> GetAllProfessionInClient(int skip = 0, int take = 0, string filter = null, int professionId = 0, int clientId = 0)
        {
            var queryResult = _professionInClientRepository.GetAll(skip, take, filter, professionId, clientId);
            return new QueryResult<ProfessionInClientViewModel>
            {
                Data = ProfessionInClientConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public ProfessionInClientViewModel GetProfessionInClientById(int id)
        {
            var professionInClientViewModel = ProfessionInClientConverter.Convert(_professionInClientRepository.GetById(id));

            return professionInClientViewModel;
        }

        public ProfessionInClientViewModel AddProfessionInClient(ProfessionInClientViewModel newProfessionInClientViewModel)
        {
            var professionInClient = new ProfessionInClient
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newProfessionInClientViewModel.iPAddress,

                PriceHourOrdinary = newProfessionInClientViewModel.priceHourOrdinary,
                PriceHourExtra = newProfessionInClientViewModel.priceHourExtra,
                PriceHourFestive = newProfessionInClientViewModel.priceHourFestive,
                PriceHourSaleOrdinary = newProfessionInClientViewModel.priceHourSaleOrdinary,
                PriceHourSaleExtra = newProfessionInClientViewModel.priceHourSaleExtra,
                PriceHourSaleFestive = newProfessionInClientViewModel.priceHourSaleFestive,
                ClientId = newProfessionInClientViewModel.clientId,
                ProfessionId = newProfessionInClientViewModel.professionId
            };

            _professionInClientRepository.Add(professionInClient);
            return newProfessionInClientViewModel;
        }

        public bool UpdateProfessionInClient(ProfessionInClientViewModel professionInClientViewModel)
        {
            if (professionInClientViewModel.id == null)
                return false;

            var professionInClient = _professionInClientRepository.GetById((int)professionInClientViewModel.id);

            if (professionInClient == null) return false;

            professionInClient.ModifiedDate = DateTime.Now;
            professionInClient.IPAddress = professionInClientViewModel.iPAddress;

            professionInClient.PriceHourOrdinary = professionInClientViewModel.priceHourOrdinary;
            professionInClient.PriceHourExtra = professionInClientViewModel.priceHourExtra;
            professionInClient.PriceHourFestive = professionInClientViewModel.priceHourFestive;
            professionInClient.PriceHourSaleOrdinary = professionInClientViewModel.priceHourSaleOrdinary;
            professionInClient.PriceHourSaleExtra = professionInClientViewModel.priceHourSaleExtra;
            professionInClient.PriceHourSaleFestive = professionInClientViewModel.priceHourSaleFestive;
            professionInClient.ClientId = professionInClientViewModel.clientId;
            professionInClient.ProfessionId = professionInClientViewModel.professionId;

            return _professionInClientRepository.Update(professionInClient);
        }

        public bool DeleteProfessionInClient(int id)
        {
            return _professionInClientRepository.Delete(id);
        }
    }
}
