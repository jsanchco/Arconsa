namespace SGDE.Tests
{
    #region Using

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SGDE.Domain.Entities;
    using SGDE.Domain.Repositories;
    using SGDE.Domain.Supervisor;
    using SGDE.Domain.ViewModels;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    [TestClass]
    public class SupervisorUserTests
    {
        private Supervisor _supervisor;

        [TestInitialize]
        public void TestInitialize()
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            //userRepositoryMock
            //    .Setup(
            //        x => x.AddAsync(
            //                        It.IsAny<User>(),
            //                        default))
            //    .Callback((
            //         User a,
            //         CancellationToken b) =>
            //    {
            //        var t = a;
            //        var t1 = b;
            //    })
            //    .ReturnsAsync((User x) => x);

            userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).Returns((User x) => x);

            var professionRepositoryMock = new Mock<IProfessionRepository>();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var trainingRepositoryMock = new Mock<ITrainingRepository>();
            var userHiringRepositoryMock = new Mock<IUserHiringRepository>();
            var workRepositoryMock = new Mock<IWorkRepository>();
            var promoterRepositoryMock = new Mock<IPromoterRepository>();
            var userDocumentRepositoryMock = new Mock<IUserDocumentRepository>();
            var typeDocumentRepositoryMock = new Mock<ITypeDocumentRepository>();
            var typeClientRepositoryMock = new Mock<ITypeClientRepository>();
            var dailySigningRepositoryMock = new Mock<IDailySigningRepository>(); 
            var settingRepositoryMock = new Mock<ISettingRepository>();
            var professionInClientRepositoryMock = new Mock<IProfessionInClientRepository>();
            var hourTypeRepositoryMock = new Mock<IHourTypeRepository>();
            var costWorkerRepositoryMock = new Mock<ICostWorkerRepository>();
            var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            var detailInvoiceRepositoryMock = new Mock<IDetailInvoiceRepository>();
            var userProfessionRepository = new Mock<IUserProfessionRepository>();
            var embargoRepository = new Mock<IEmbargoRepository>();
            var detailEmbargoRepository = new Mock<IDetailEmbargoRepository>();
            var sSHiringRepository = new Mock<ISSHiringRepository>();
            var workCostRepository = new Mock<IWorkCostRepository>();
            var workBudgetDataRepository = new Mock<IWorkBudgetDataRepository>();
            var workBudgetRepository = new Mock<IWorkBudgetRepository>();
            var indirectCostRepository = new Mock<IIndirectCostRepository>();
            var advanceRepository = new Mock<IAdvanceRepository>();
            var libraryRepository = new Mock<ILibraryRepository>();
            var companyDataRepository = new Mock<ICompanyDataRepository>();
            var workHistoryRepository = new Mock<IWorkHistoryRepository>();
            var workStatusHistoryRepository = new Mock<IWorkStatusHistoryRepository>();
            var invoicePaymentHistoryRepository = new Mock<IInvoicePaymentHistoryRepository>();
            var enterpriseRepository = new Mock<IEnterpriseRepository>();
            var userEnterpriseRepository = new Mock<IUserEnterpriseRepository>();

            _supervisor = new Supervisor(
                userRepositoryMock.Object,
                professionRepositoryMock.Object,
                clientRepositoryMock.Object,
                roleRepositoryMock.Object,
                trainingRepositoryMock.Object,
                userHiringRepositoryMock.Object,
                workRepositoryMock.Object, 
                promoterRepositoryMock.Object,
                userDocumentRepositoryMock.Object,
                typeDocumentRepositoryMock.Object,
                typeClientRepositoryMock.Object,
                dailySigningRepositoryMock.Object,
                settingRepositoryMock.Object,
                professionInClientRepositoryMock.Object,
                hourTypeRepositoryMock.Object,
                costWorkerRepositoryMock.Object, 
                invoiceRepositoryMock.Object, 
                detailInvoiceRepositoryMock.Object,
                userProfessionRepository.Object,
                embargoRepository.Object,
                detailEmbargoRepository.Object,
                sSHiringRepository.Object,
                workCostRepository.Object,
                workBudgetDataRepository.Object,
                workBudgetRepository.Object,
                indirectCostRepository.Object,
                advanceRepository.Object,
                libraryRepository.Object,
                companyDataRepository.Object,
                workHistoryRepository.Object,
                workStatusHistoryRepository.Object,
                invoicePaymentHistoryRepository.Object,
                enterpriseRepository.Object,
                userEnterpriseRepository.Object);
        }

        [TestMethod]
        public void AddUser_InputUserViewModel_ReturnUserViewModel()
        {
            var userViewModel = new UserViewModel
            {
                name = "Jes�s",
                surname = "S�nchez Corzo"
            };
            var newUserViewModel = _supervisor.AddUser(userViewModel);

            Assert.AreNotEqual(newUserViewModel.roleId, 0);
        }

        [TestMethod]
        public async Task AddUserAsync_InputUserViewModel_ReturnUserViewModel()
        {
            var userViewModel = new UserViewModel
            {
                name = "Jes�s",
                surname = "S�nchez Corzo"
            };
            var newUserViewModel = await _supervisor.AddUserAsync(userViewModel);

            Assert.AreNotEqual(newUserViewModel.roleId, 0);
        }
    }
}
