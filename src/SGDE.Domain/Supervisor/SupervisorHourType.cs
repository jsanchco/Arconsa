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
        public List<HourTypeViewModel> GetAllHourType()
        {
            return HourTypeConverter.ConvertList(_hourTypeRepository.GetAll());
        }

        public HourTypeViewModel GetHourTypeById(int id)
        {
            var hourTypeViewModel = HourTypeConverter.Convert(_hourTypeRepository.GetById(id));

            return hourTypeViewModel;
        }

        public HourTypeViewModel AddHourType(HourTypeViewModel newHourTypeViewModel)
        {
            var hourType = new HourType
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newHourTypeViewModel.iPAddress,

                Name = newHourTypeViewModel.name
            };

            _hourTypeRepository.Add(hourType);
            return newHourTypeViewModel;
        }

        public bool UpdateHourType(HourTypeViewModel hourTypeViewModel)
        {
            if (hourTypeViewModel.id == null)
                return false;

            var hourType = _hourTypeRepository.GetById((int)hourTypeViewModel.id);

            if (hourType == null) return false;

            hourType.ModifiedDate = DateTime.Now;
            hourType.IPAddress = hourTypeViewModel.iPAddress;

            hourType.Name = hourTypeViewModel.name;

            return _hourTypeRepository.Update(hourType);
        }

        public bool DeleteHourType(int id)
        {
            return _hourTypeRepository.Delete(id);
        }
    }
}
