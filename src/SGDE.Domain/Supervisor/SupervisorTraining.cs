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
        public List<TrainingViewModel> GetAllTraining(int userId)
        {
            return TrainingConverter.ConvertList(_trainingRepository.GetAll(userId));
        }

        public TrainingViewModel GetTrainingById(int id)
        {
            var trainingViewModel = TrainingConverter.Convert(_trainingRepository.GetById(id));

            return trainingViewModel;
        }

        public TrainingViewModel AddTraining(TrainingViewModel newTrainingViewModel)
        {
            var training = new Training
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newTrainingViewModel.iPAddress,

                Name = newTrainingViewModel.name,
                Hours = newTrainingViewModel.hours,
                Center = newTrainingViewModel.center,
                Address = newTrainingViewModel.address,
                File = newTrainingViewModel.file,
                UserId = newTrainingViewModel.userId
            };

            _trainingRepository.Add(training);
            return newTrainingViewModel;
        }

        public bool UpdateTraining(TrainingViewModel trainingViewModel)
        {
            if (trainingViewModel.id == null)
                return false;

            var training = _trainingRepository.GetById((int)trainingViewModel.id);

            if (training == null) return false;

            training.ModifiedDate = DateTime.Now;
            training.IPAddress = trainingViewModel.iPAddress;

            training.Name = trainingViewModel.name;
            training.Hours = trainingViewModel.hours;
            training.Center = trainingViewModel.center;
            training.Address = trainingViewModel.address;
            training.File = trainingViewModel.file;
            training.UserId = trainingViewModel.userId;

            return _trainingRepository.Update(training);
        }

        public bool DeleteTraining(int id)
        {
            return _trainingRepository.Delete(id);
        }
    }
}
