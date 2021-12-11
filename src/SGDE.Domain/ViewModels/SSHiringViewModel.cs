using System;

namespace SGDE.Domain.ViewModels
{
    public class SSHiringViewModel : BaseEntityViewModel
    {
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string observations { get; set; }
        public int userId { get; set; }
    }
}
