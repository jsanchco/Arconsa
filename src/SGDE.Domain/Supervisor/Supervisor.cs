﻿// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{
    #region Using

    using Repositories;

    #endregion

    public partial class Supervisor : ISupervisor
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IUserHiringRepository _userHiringRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IPromoterRepository _promoterRepository;
        private readonly IUserDocumentRepository _userDocumentRepository;
        private readonly ITypeDocumentRepository _typeDocumentRepository;
        private readonly ITypeClientRepository _typeClientRepository;
        private readonly IDailySigningRepository _dailySigningRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IProfessionInClientRepository _professionInClientRepository;
        private readonly IHourTypeRepository _hourTypeRepository;
        private readonly ICostWorkerRepository _costWorkerRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IDetailInvoiceRepository _detailInvoiceRepository;
        private readonly IUserProfessionRepository _userProfessionRepository;
        private readonly IEmbargoRepository _embargoRepository;
        private readonly IDetailEmbargoRepository _detailEmbargoRepository;
        private readonly ISSHiringRepository _sSHiringRepository;
        private readonly IWorkCostRepository _workCostRepository;
        private readonly IWorkBudgetRepository _workBudgetRepository;
        private readonly IWorkBudgetDataRepository _workBudgetDataRepository;
        private readonly IIndirectCostRepository _indirectCostRepository;
        private readonly IAdvanceRepository _advanceRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly ICompanyDataRepository _companyDataRepository;
        private readonly IWorkHistoryRepository _workHistoryRepository;
        private readonly IWorkStatusHistoryRepository _workStatusHistoryRepository;
        private readonly IInvoicePaymentHistoryRepository _invoicePaymentHistoryRepository;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IUserEnterpriseRepository _userEnterpriseRepository;

        public Supervisor()
        {
        }

        public Supervisor(
            IUserRepository userRepository,
            IProfessionRepository professionRepository,
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            ITrainingRepository trainingRepository,
            IUserHiringRepository userHiringRepository,
            IWorkRepository workRepository,
            IPromoterRepository promoterRepository,
            IUserDocumentRepository userDocumentRepository,
            ITypeDocumentRepository typeDocumentRepository,
            ITypeClientRepository typeClientRepository,
            IDailySigningRepository dailySigningRepository,
            ISettingRepository settingRepository,
            IProfessionInClientRepository professionInClientRepository,
            IHourTypeRepository hourTypeRepository,
            ICostWorkerRepository costWorkerRepository,
            IInvoiceRepository invoiceRepository,
            IDetailInvoiceRepository detailInvoiceRepository,
            IUserProfessionRepository userProfessionRepository,
            IEmbargoRepository embargoRepository,
            IDetailEmbargoRepository detailEmbargoRepository,
            ISSHiringRepository sSHiringRepository,
            IWorkCostRepository workCostRepository,
            IWorkBudgetDataRepository workBudgetDataRepository,
            IWorkBudgetRepository workBudgetRepository,
            IIndirectCostRepository indirectCostRepository,
            IAdvanceRepository advanceRepository,
            ILibraryRepository libraryRepository,
            ICompanyDataRepository companyDataRepository,
            IWorkHistoryRepository workHistoryRepository,
            IWorkStatusHistoryRepository workStatusHistoryRepository,
            IInvoicePaymentHistoryRepository invoicePaymentHistoryRepository, 
            IEnterpriseRepository enterpriseRepository,
            IUserEnterpriseRepository userEnterpriseRepository)
        {
            _userRepository = userRepository;
            _professionRepository = professionRepository;
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
            _trainingRepository = trainingRepository;
            _userHiringRepository = userHiringRepository;
            _workRepository = workRepository;
            _promoterRepository = promoterRepository;
            _userDocumentRepository = userDocumentRepository;
            _typeDocumentRepository = typeDocumentRepository;
            _typeClientRepository = typeClientRepository;
            _dailySigningRepository = dailySigningRepository;
            _settingRepository = settingRepository;
            _professionInClientRepository = professionInClientRepository;
            _hourTypeRepository = hourTypeRepository;
            _costWorkerRepository = costWorkerRepository;
            _invoiceRepository = invoiceRepository;
            _detailInvoiceRepository = detailInvoiceRepository;
            _userProfessionRepository = userProfessionRepository;
            _embargoRepository = embargoRepository;
            _detailEmbargoRepository = detailEmbargoRepository;
            _sSHiringRepository = sSHiringRepository;
            _workCostRepository = workCostRepository;
            _workBudgetDataRepository = workBudgetDataRepository;
            _workBudgetRepository = workBudgetRepository;
            _indirectCostRepository = indirectCostRepository;
            _advanceRepository = advanceRepository;
            _libraryRepository = libraryRepository;
            _companyDataRepository = companyDataRepository;
            _workHistoryRepository = workHistoryRepository;
            _workStatusHistoryRepository = workStatusHistoryRepository;
            _invoicePaymentHistoryRepository = invoicePaymentHistoryRepository;
            _enterpriseRepository = enterpriseRepository;
            _userEnterpriseRepository = userEnterpriseRepository;
        }
    }
}
