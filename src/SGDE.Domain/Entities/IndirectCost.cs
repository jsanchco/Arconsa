using System;

namespace SGDE.Domain.Entities
{
    public class IndirectCost : BaseEntity
    {
        public DateTime Date { get; set; }
        public int Year => Date.Year;
        public string Month => new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }[Date.Month];
        public string Key => $"{Year}/{Date.Month + 1}:00";
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
