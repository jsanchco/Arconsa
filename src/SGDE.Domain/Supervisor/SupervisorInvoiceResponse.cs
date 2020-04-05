namespace SGDE.Domain.Supervisor
{
    #region Using

    using ViewModels;
    using SGDE.Domain.Helpers;

    #endregion

    public partial class Supervisor
    {
        public InvoiceResponseViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel)
        {
            GenerateInvoice generateInvoice;
            switch (invoiceQueryViewModel.typeInvoice)
            {
                case 1:
                    generateInvoice = new GenerateInvoiceByWork(this, invoiceQueryViewModel);
                    break;
                case 2:
                    generateInvoice = new GenerateInvoiceCustom(this, invoiceQueryViewModel);
                    break;

                default:
                    generateInvoice = new GenerateInvoiceByWork(this, invoiceQueryViewModel);
                    break;
            }

            generateInvoice.Process();
            return generateInvoice._invoiceResponseViewModel;
        }
    }
}
