namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class UserHiringViewModel : BaseEntityViewModel
    {
        public string name 
        {
            get
            {
                var dtStartDate = DateTime.ParseExact(startDate, "MM/dd/yyyy", null);

                return $"{workName} {dtStartDate.ToString("dd/MM/yyyy")}";
            }
        }

        public string startDate { get; set; }
        public string endDate { get; set; }

        public int workId { get; set; }
        public string workName { get; set; }

        public int userId { get; set; }
        public string userName { get; set; }
    }
}
