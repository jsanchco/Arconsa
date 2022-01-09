namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class IndirectCostConverter
    {
        public static IndirectCostViewModel Convert(IndirectCost indirectCost)
        {
            if (indirectCost == null)
                return null;

            var indirectCostViewModel = new IndirectCostViewModel
            {
                id = indirectCost.Id,
                addedDate = indirectCost.AddedDate,
                modifiedDate = indirectCost.ModifiedDate,
                iPAddress = indirectCost.IPAddress,

                date = indirectCost.Date,
                description = indirectCost.Description,
                accountNumber = indirectCost.AccountNumber
            };

            return indirectCostViewModel;
        }

        public static List<IndirectCostViewModel> ConvertList(IEnumerable<IndirectCost> indirectCosts)
        {
            return indirectCosts?.Select(indirectCost =>
            {
                var model = new IndirectCostViewModel
                {
                    id = indirectCost.Id,
                    addedDate = indirectCost.AddedDate,
                    modifiedDate = indirectCost.ModifiedDate,
                    iPAddress = indirectCost.IPAddress,

                    date = indirectCost.Date,
                    description = indirectCost.Description,
                    accountNumber = indirectCost.AccountNumber
                };
                return model;
            })
                .ToList();
        }
    }
}
