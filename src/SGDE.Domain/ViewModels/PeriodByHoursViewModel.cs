using System;

namespace SGDE.Domain.ViewModels
{
    public class PeriodByHoursViewModel
    {
        public int id { get; set; }
        public DateTime? startHour { get; set; }
        public DateTime? endHour { get; set; }
        public int hourTypeId { get; set; }
    }
}
