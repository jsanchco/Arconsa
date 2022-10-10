using SGDE.Domain.Entities;
using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkHistoryViewModel : BaseEntityViewModel
    {
        public string reference { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string observations { get; set; }
        public string type { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public string fileName { get; set; }
        public int workId { get; set; }
    }
}
