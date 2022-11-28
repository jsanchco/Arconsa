using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkStatusHistoryViewModel : BaseEntityViewModel
    {
        public DateTime dateChange { get; set; }
        public string value { get; set; }
        public string observations { get; set; }

        public int workId { get; set; }
        public string workName { get; set; }
    }
}
