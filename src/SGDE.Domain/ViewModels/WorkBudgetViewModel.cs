using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkBudgetViewModel : BaseEntityViewModel
    {
        public DateTime date { get; set; }
        public string reference { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string nameInWork { get; set; }
        public double totalContract { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public bool hasFile => file != null;
        public int workId { get; set; }
        public string workName { get; set; }
    }
}
