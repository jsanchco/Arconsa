namespace SGDE.Tests
{
    #region Using

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SGDE.Domain.Entities;
    using SGDE.Domain.Repositories;
    using SGDE.Domain.Supervisor;
    using SGDE.Domain.ViewModels;
    using System.Threading.Tasks;

    #endregion

    [TestClass]
    public class SupervisorUserTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            //coffeeCupRepositoryMock.Setup(x => x.GetCoffeeCupsInStockCountAsync()).ReturnsAsync(_numberOfCupsInStock);
            var userRepositoryMock = new Mock<IUserRepository>();
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

            var supervisorUser = new Supervisor(
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
            //var userRepositoryMock = new Mock<IUserRepository>();
            //userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync((User x) => x);
            //var supervisorUser = new Supervisor(userRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

            //var user = new User 
            //{
            //    Name = "Jesús",
            //    Surname = "Sánchez Corzo"
            //};
            //var newUserViewModel = await supervisorUser.AddUserAsync(user);

            //Assert.AreNotEqual(user.roleId, 0);
        }
    }
}
