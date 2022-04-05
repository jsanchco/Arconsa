using System;

namespace SGDE.Domain.ViewModels
{
    public class AdvanceViewModel : BaseEntityViewModel
    {
        public DateTime concessionDate { get; set; }
        public DateTime? payDate { get; set; }
        public double amount { get; set; }
        public bool paid => payDate != null;
        public int userId { get; set; }
        public string userName { get; set; }
    }
}
