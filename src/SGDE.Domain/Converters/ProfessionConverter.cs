namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public static class ProfessionConverter
    {
        public static ProfessionViewModel Convert(Profession profession)
        {
            if (profession == null)
                return null;

            var professionViewModel = new ProfessionViewModel
            {
                id = profession.Id,
                addedDate = profession.AddedDate,
                modifiedDate = profession.ModifiedDate,
                iPAddress = profession.IPAddress,

                name = profession.Name,
                description = profession.Description,
                users = profession.UserProfessions.Select(x => x.UserId).ToList()
            };

            return professionViewModel;
        }

        public static List<ProfessionViewModel> ConvertList(IEnumerable<Profession> professions)
        {
            return professions?.Select(profession =>
                {
                    var model = new ProfessionViewModel
                    {
                        id = profession.Id,
                        addedDate = profession.AddedDate,
                        modifiedDate = profession.ModifiedDate,
                        iPAddress = profession.IPAddress,

                        name = profession.Name,
                        description = profession.Description,
                        users = profession.UserProfessions.Select(x => x.UserId).ToList()
                    };
                    return model;
                })
                .ToList();
        }
    }
}
