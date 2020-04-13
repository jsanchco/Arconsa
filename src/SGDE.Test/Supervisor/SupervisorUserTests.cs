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
            userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync((User x) => x);
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
                detailInvoiceRepositoryMock.Object);
        }

        [TestMethod]
        public async Task AddUserAsync_InputUserViewModel_ReturnUserViewModel()
        {
            var userViewModel = new UserViewModel
            {
                name = "Jesús",
                surname = "Sánchez Corzo"
            };
            var newUserViewModel = await _supervisor.AddUserAsync(userViewModel);

            Assert.AreNotEqual(newUserViewModel.roleId, 0);
        }
    }
}
