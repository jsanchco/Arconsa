using System;

namespace SGDE.Domain.ViewModels
{
    public class DetailEmbargoViewModel : BaseEntityViewModel
    {
        public DateTime datePay { get; set; }
        public string observations { get; set; }
        public double amount { get; set; }
        public int embargoId { get; set; }
    }
}
