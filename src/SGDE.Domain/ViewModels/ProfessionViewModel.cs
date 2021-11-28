// ReSharper disable InconsistentNaming
using System.Collections.Generic;

namespace SGDE.Domain.ViewModels
{
    public class ProfessionViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<int> users { get; set; }
    }
}
