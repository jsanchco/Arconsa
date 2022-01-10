namespace SGDE.Domain.Entities
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class Invoice : BaseEntity
    {
        public int InvoiceNumber { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime IssueDate { get; set; }
        public Decimal TaxBase { get; set; }
        public double IvaTaxBase { get; set; }
        public double Total { get; set; }
        public bool Iva { get; set; }
        public int TypeInvoice { get; set; } // 1 = por horas, 2 = custom
        public string KeyOrder => $"{StartDate.Year.ToString().Substring(2, 2)}{InvoiceNumber:0000}";

        public Decimal Retentions { get; set; }
        public int State { get; set; } // 0 = nada, 1 = Añaddido, 2 = Modificado, 3 = Existe con los mismos datos => no hago nada

        public int? WorkId { get; set; }
        public virtual Work Work { get; set; }
        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? InvoiceToCancelId { get; set; }
        public virtual Invoice InvoiceToCancel { get; set; }

        public virtual ICollection<DetailInvoice> DetailsInvoice { get; set; } = new HashSet<DetailInvoice>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
