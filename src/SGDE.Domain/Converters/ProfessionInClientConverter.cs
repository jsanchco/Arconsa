namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class ProfessionInClientConverter
    {
        public static ProfessionInClientViewModel Convert(ProfessionInClient professionInClient)
        {
            if (professionInClient == null)
                return null;

            var professionInClientViewModel = new ProfessionInClientViewModel
            {
                id = professionInClient.Id,
                addedDate = professionInClient.AddedDate,
                modifiedDate = professionInClient.ModifiedDate,
                iPAddress = professionInClient.IPAddress,

                priceHourSaleOrdinary = professionInClient.PriceHourSaleOrdinary,
                priceHourSaleExtra = professionInClient.PriceHourSaleExtra,
                priceHourSaleFestive = professionInClient.PriceHourSaleFestive,
                priceHourSaleNocturnal = professionInClient.PriceHourSaleNocturnal,
                priceDailySale = professionInClient.PriceDailySale,

                clientId = professionInClient.ClientId,
                clientName = professionInClient.Client.Name,
                professionId = professionInClient.ProfessionId,
                professionName = professionInClient.Profession.Name
            };

            return professionInClientViewModel;
        }

        public static List<ProfessionInClientViewModel> ConvertList(IEnumerable<ProfessionInClient> professionInClients)
        {
            return professionInClients?.Select(professionInClient =>
            {
                var model = new ProfessionInClientViewModel
                {
                    id = professionInClient.Id,
                    addedDate = professionInClient.AddedDate,
                    modifiedDate = professionInClient.ModifiedDate,
                    iPAddress = professionInClient.IPAddress,

                    priceHourSaleOrdinary = professionInClient.PriceHourSaleOrdinary,
                    priceHourSaleExtra = professionInClient.PriceHourSaleExtra,
                    priceHourSaleFestive = professionInClient.PriceHourSaleFestive,
                    priceHourSaleNocturnal = professionInClient.PriceHourSaleNocturnal,
                    priceDailySale = professionInClient.PriceDailySale,

                    clientId = professionInClient.ClientId,
                    clientName = professionInClient.Client.Name,
                    professionId = professionInClient.ProfessionId,
                    professionName = professionInClient.Profession.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
