using System;

namespace SGDE.Domain.ViewModels
{
    public class IndirectCostViewModel : BaseEntityViewModel
    {
        public DateTime date { get; set; }
        public int year => date.Year;
        public string month => new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }[date.Month];
        public string key => $"{year}/{date.Month + 1}:00";
        public string accountNumber { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
    }
}
