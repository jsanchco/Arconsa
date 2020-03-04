namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class UserHiringViewModel : BaseEntityViewModel
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public int workId { get; set; }
        public string workName { get; set; }

        public int userId { get; set; }
        public string userName { get; set; }
    }
}
