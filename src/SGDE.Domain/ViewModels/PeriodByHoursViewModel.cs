namespace SGDE.Domain.ViewModels
{
    public class PeriodByHoursViewModel
    {
        public int id { get; set; }
        public string startHour { get; set; }
        public string endHour { get; set; }
        public int hourTypeId { get; set; }
    }
}
