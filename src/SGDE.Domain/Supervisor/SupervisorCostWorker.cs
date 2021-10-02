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
        public QueryResult<CostWorkerViewModel> GetAllCostWorker(int skip = 0, int take = 0, string filter = null, int userId = 0)
        {
            var queryResult = _costWorkerRepository.GetAll(skip, take, filter, userId);
            return new QueryResult<CostWorkerViewModel>
            {
                Data = CostWorkerConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public CostWorkerViewModel GetCostWorkerById(int id)
        {
            var costWorkerViewModel = CostWorkerConverter.Convert(_costWorkerRepository.GetById(id));

            return costWorkerViewModel;
        }

        public CostWorkerViewModel AddCostWorker(CostWorkerViewModel newCostWorkerViewModel)
        {
            var costWorker = new CostWorker
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newCostWorkerViewModel.iPAddress,

                PriceHourOrdinary = newCostWorkerViewModel.priceHourOrdinary,
                PriceHourExtra = newCostWorkerViewModel.priceHourExtra,
                PriceHourFestive = newCostWorkerViewModel.priceHourFestive,

                StartDate = DateTime.Parse(newCostWorkerViewModel.startDate),

                EndDate = string.IsNullOrEmpty(newCostWorkerViewModel.endDate)
                ? null :
                (DateTime?)DateTime.Parse(newCostWorkerViewModel.endDate),

                Observations = newCostWorkerViewModel.observations,

                UserId = newCostWorkerViewModel.userId
            };

            if (!_costWorkerRepository.ValidateCostWorker(costWorker))
                throw new Exception("Fechas mal configuradas");

            _costWorkerRepository.Add(costWorker);
            return newCostWorkerViewModel;
        }

        public bool UpdateCostWorker(CostWorkerViewModel costWorkerViewModel)
        {
            if (costWorkerViewModel.id == null)
                return false;

            var costWorker = _costWorkerRepository.GetById((int)costWorkerViewModel.id);

            if (costWorker == null) return false;

            costWorker.ModifiedDate = DateTime.Now;
            costWorker.IPAddress = costWorkerViewModel.iPAddress;

            costWorker.PriceHourOrdinary = costWorkerViewModel.priceHourOrdinary;
            costWorker.PriceHourExtra = costWorkerViewModel.priceHourExtra;
            costWorker.PriceHourFestive = costWorkerViewModel.priceHourFestive;

            costWorker.StartDate = DateTime.Parse(costWorkerViewModel.startDate);

            costWorker.EndDate = string.IsNullOrEmpty(costWorkerViewModel.endDate)
                ? null
                : (DateTime?)DateTime.Parse(costWorkerViewModel.endDate);

            costWorker.Observations = costWorkerViewModel.observations;

            costWorker.UserId = costWorkerViewModel.userId;

            if (!_costWorkerRepository.ValidateCostWorker(costWorker))
                throw new Exception("Fechas mal configuradas");

            return _costWorkerRepository.Update(costWorker);
        }

        public bool DeleteCostWorker(int id)
        {
            return _costWorkerRepository.Delete(id);
        }
    }
}
