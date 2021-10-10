namespace SGDE.Domain.ViewModels
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class MassiveSigningQueryViewModel
    {
        public string startSigning { get; set; }
        public string endSigning { get; set; }
        public int userHiringId { get; set; }
        public bool includeSaturdays { get; set; }
        public bool includeSundays { get; set; }
        public List<PeriodByHoursViewModel> data { get; set; }
    }
}
