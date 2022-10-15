using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkClosePageViewModel
    {
        public string openDate { get; set; }
        public string closeDate { get; set; }
        public string workName { get; set; }
        public string workAddress { get; set; }
        public int workId { get; set; }
        public string clientName { get; set; }
        public int clientId { get; set; }
        public string workBudgetsName { get; set; }
        public string workBudgetsSumFormat { get; set; }
        public double workBudgetsSum { get; set; }
        public double invoicesSum { get; set; }
        public double workCostsSum { get; set; }
        public double authorizeCancelWorkersCostsSum { get; set; }
        public double authorizeCancelWorkersCostsSalesSum { get; set; }
        public double indirectCostsSum { get; set; }

        public double total
        {
            get
            {
                return invoicesSum - workCostsSum - authorizeCancelWorkersCostsSum - indirectCostsSum;
            }
        }
    }
}
