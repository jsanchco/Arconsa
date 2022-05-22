using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkBudgetDataViewModel : BaseEntityViewModel
    {
        public DateTime date { get; set; }
        public string reference { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
    }
}
