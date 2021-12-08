using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;
using System.Linq;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<DetailEmbargoViewModel> GetAllDetailEmbargo(int skip = 0, int take = 0, int embargoId = 0)
        {
            var queryResult = _detailEmbargoRepository.GetAll(skip, take, embargoId);
            return new QueryResult<DetailEmbargoViewModel>
            {
                Data = DetailEmbargoConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public DetailEmbargoViewModel GetDetailEmbargoById(int id)
        {
            var detailEmbargoViewModel = DetailEmbargoConverter.Convert(_detailEmbargoRepository.GetById(id));

            return detailEmbargoViewModel;
        }

        public DetailEmbargoViewModel AddDetailEmbargo(DetailEmbargoViewModel newDetailEmbargo)
        {
            var detailEmbargo = new DetailEmbargo
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newDetailEmbargo.iPAddress,

                DatePay = newDetailEmbargo.datePay,
                Observations = newDetailEmbargo.observations,
                Amount = newDetailEmbargo.amount,
                EmbargoId = newDetailEmbargo.embargoId
            };

            ValidateAddDetailEmbargo(detailEmbargo, out bool isPaid);

            _detailEmbargoRepository.Add(detailEmbargo, isPaid);

            return GetDetailEmbargoById(detailEmbargo.Id);
        }

        public bool UpdateDetailEmbargo(DetailEmbargoViewModel detailEmbargoViewModel)
        {
            if (detailEmbargoViewModel.id == null)
                return false;

            var detailEmbargo = _detailEmbargoRepository.GetById((int)detailEmbargoViewModel.id);

            if (detailEmbargo == null) return false;

            detailEmbargo.ModifiedDate = DateTime.Now;
            detailEmbargo.IPAddress = detailEmbargoViewModel.iPAddress;

            detailEmbargo.DatePay = detailEmbargoViewModel.datePay;
            detailEmbargo.Observations = detailEmbargoViewModel.observations;
            detailEmbargo.Amount = detailEmbargoViewModel.amount;
            detailEmbargo.EmbargoId = detailEmbargoViewModel.embargoId;

            ValidateUpdateDetailEmbargo(detailEmbargo, out bool isPaid);

            return _detailEmbargoRepository.Update(detailEmbargo, isPaid);
        }

        public bool DeleteDetailEmbargo(int id)
        {
            return _detailEmbargoRepository.Delete(id);
        }

        #region Auxiliary Methods

        private void ValidateAddDetailEmbargo(DetailEmbargo detailEmbargo, out bool isPaid)
        {
            var embargo = _embargoRepository.GetById(detailEmbargo.EmbargoId);
            if (embargo == null)
                throw new Exception($"Detalle Embargo mal configurado. No existe el Embargo asociado [{detailEmbargo.EmbargoId}]");

            var sumLastDetails = embargo.DetailEmbargos.Sum(x => x.Amount);
            if ((sumLastDetails + detailEmbargo.Amount) > embargo.Total)
                throw new Exception($"Detalle Embargo mal configurado. La suma anterior, si la hubiere, y el nuevo valor excede del Total");

            isPaid = (sumLastDetails + detailEmbargo.Amount) == embargo.Total;
        }

        private void ValidateUpdateDetailEmbargo(DetailEmbargo detailEmbargo, out bool isPaid)
        {
            var embargo = _embargoRepository.GetById(detailEmbargo.EmbargoId);
            if (embargo == null)
                throw new Exception($"Detalle Embargo mal configurado. No existe el Embargo asociado [{detailEmbargo.EmbargoId}]");

            var sumLastDetails = embargo.DetailEmbargos
                .Where(x => x.Id != detailEmbargo.Id)
                .Sum(x => x.Amount);
            if ((sumLastDetails + detailEmbargo.Amount) > embargo.Total)
                throw new Exception($"Detalle Embargo mal configurado. La suma anterior, si la hubiere, y el nuevo valor excede del Total");

            isPaid = (sumLastDetails + detailEmbargo.Amount) == embargo.Total;
        }

        #endregion
    }
}
