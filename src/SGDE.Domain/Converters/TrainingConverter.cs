namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class TrainingConverter
    {
        public static TrainingViewModel Convert(Training training)
        {
            if (training == null)
                return null;

            var trainingViewModel = new TrainingViewModel
            {
                id = training.Id,
                addedDate = training.AddedDate,
                modifiedDate = training.ModifiedDate,
                iPAddress = training.IPAddress,

                name = training.Name,
                hours = training.Hours,
                center = training.Center,
                address = training.Address,
                file = training.File,
                userId = training.UserId,
                userName = $"{training.User.Name} {training.User.Surname}"
            };

            return trainingViewModel;
        }

        public static List<TrainingViewModel> ConvertList(IEnumerable<Training> trainings)
        {
            return trainings?.Select(training =>
            {
                var model = new TrainingViewModel
                {
                    id = training.Id,
                    addedDate = training.AddedDate,
                    modifiedDate = training.ModifiedDate,
                    iPAddress = training.IPAddress,

                    name = training.Name,
                    hours = training.Hours,
                    center = training.Center,
                    address = training.Address,
                    file = training.File,
                    userId = training.UserId,
                    userName = $"{training.User.Name} {training.User.Surname}"
                };
                return model;
            })
                .ToList();
        }
    }
}
