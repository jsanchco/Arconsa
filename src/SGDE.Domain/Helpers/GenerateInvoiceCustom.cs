namespace SGDE.Domain.Helpers
{
    #region Using

    using SGDE.Domain.ViewModels;
    using SGDE.Domain.Supervisor;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.IO;
    using System;
    using SGDE.Domain.Converters;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class GenerateInvoiceCustom : GenerateInvoice
    {
        public GenerateInvoiceCustom(Supervisor supervisor, InvoiceQueryViewModel invoiceQueryViewModel) : base(supervisor, invoiceQueryViewModel) { }

        protected override PdfPTable GetTableNumberInvoice()
        {
            throw new NotImplementedException();
        }

        protected override bool Validate()
        {
            if (_invoiceQueryViewModel.workId == null)
                return false;

            _work = _supervisor.GetWork((int)_invoiceQueryViewModel.workId);
            if (_work == null)
                return false;

            return true;
        }
    }
}
