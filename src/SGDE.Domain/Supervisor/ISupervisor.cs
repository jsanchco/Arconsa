// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{   
    #region Using

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ViewModels;
    using Domain.Helpers;
    using SGDE.Domain.Entities;

    #endregion

    public interface ISupervisor
    {
        #region User

        Task<UserViewModel> Authenticate(string user, string password, CancellationToken ct = default(CancellationToken));
        Task<List<UserViewModel>> GetAllUserAsync(CancellationToken ct = default(CancellationToken));
        Task<UserViewModel> GetUserByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<UserViewModel> AddUserAsync(UserViewModel newUserViewModel, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateUserAsync(UserViewModel userViewModel, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteUserAsync(int id, CancellationToken ct = default(CancellationToken));

        QueryResult<UserViewModel> GetAllUsers(int skip = 0, int take = 0, string filter = null, List<int> roles = null);
        UserViewModel GetUserById(int id);
        UserViewModel AddUser(UserViewModel newUserViewModel);
        bool UpdateUser(UserViewModel userViewModel);
        bool UpdatePassword(UserViewModel userViewModel);
        bool RestorePassword(int userId);
        bool UpdateUserPhoto(int userId, byte[] photo);
        bool DeleteUser(int id);

        #endregion

        #region Profession

        Task<List<ProfessionViewModel>> GetAllProfessionAsync(CancellationToken ct = default(CancellationToken));
        Task<ProfessionViewModel> GetProfessionByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<List<UserViewModel>> GetUsersByProfessionIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<ProfessionViewModel> AddAsync(ProfessionViewModel newProfessionViewModel, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(ProfessionViewModel professionViewModel, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken));

        List<ProfessionViewModel> GetAllProfession();
        ProfessionViewModel GetProfessionById(int id);
        List<UserViewModel> GetUsersByProfessionId(int id);
        ProfessionViewModel AddProfession(ProfessionViewModel newProfessionViewModel);
        bool UpdateProfession(ProfessionViewModel professionViewModel);
        bool DeleteProfession(int id);

        #endregion

        #region Client

        QueryResult<ClientViewModel> GetAllClient(int skip = 0, int take = 0, string filter = null);
        ClientViewModel GetClientById(int id);
        ClientViewModel AddClient(ClientViewModel newClientViewModel);
        bool UpdateClient(ClientViewModel clientViewModel);
        bool DeleteClient(int id);

        #endregion

        #region Role

        List<RoleViewModel> GetAllRole();
        RoleViewModel GetRoleById(int id);
        RoleViewModel AddRole(RoleViewModel newRoleViewModel);
        bool UpdateRole(RoleViewModel roleViewModel);
        bool DeleteRole(int id);

        #endregion

        #region Training

        List<TrainingViewModel> GetAllTraining(int userId);
        TrainingViewModel GetTrainingById(int id);
        TrainingViewModel AddTraining(TrainingViewModel newTrainingViewModel);
        bool UpdateTraining(TrainingViewModel trainingViewModel);
        bool DeleteTraining(int id);

        #endregion

        #region UserHiring

        List<UserHiringViewModel> GetAllUserHiring(int userId = 0, int workId = 0);
        UserHiringViewModel GetUserHiringById(int id);
        UserHiringViewModel AddUserHiring(UserHiringViewModel newUserHiringViewModel);
        bool UpdateUserHiring(UserHiringViewModel userHiringViewModel);
        bool DeleteUserHiring(int id);
        bool AssignWorkers(WorkersInWorkViewModel workersInWorkViewModel);
        bool IsProfessionInClient(int? professionId, int workId = 0, int clientId = 0);

        #endregion

        #region Work

        QueryResult<WorkViewModel> GetAllWork(int skip = 0, int take = 0, string filter = null, int clientId = 0);
        WorkViewModel GetWorkById(int id);
        List<UserViewModel> GetUsersByWork(int workId, int state = 0); // 0 = all, 1 = asset, 2 = no asset
        WorkViewModel AddWork(WorkViewModel newWorkViewModel);
        bool UpdateWork(WorkViewModel workViewModel);
        bool UpdateDatesWork(WorkViewModel workViewModel);
        bool DeleteWork(int id);

        #endregion

        #region Promoter

        List<PromoterViewModel> GetAllPromoter();
        PromoterViewModel GetPromoterById(int id);
        PromoterViewModel AddPromoter(PromoterViewModel newPromoterViewModel);
        bool UpdatePromoter(PromoterViewModel promoterViewModel);
        bool DeletePromoter(int id);

        #endregion

        #region UserDocument

        List<UserDocumentViewModel> GetAllUserDocument(int userId);
        UserDocumentViewModel GetUserDocumentById(int id);
        UserDocumentViewModel AddUserDocument(UserDocumentViewModel newUserDocumentViewModel);
        bool UpdateUserDocument(UserDocumentViewModel userDocumentViewModel);
        bool DeleteUserDocument(int id);

        #endregion

        #region TypeDocument

        List<TypeDocumentViewModel> GetAllTypeDocument();
        TypeDocumentViewModel GetTypeDocumentById(int id);
        TypeDocumentViewModel AddTypeDocument(TypeDocumentViewModel newTypeDocumentViewModel);
        bool UpdateTypeDocument(TypeDocumentViewModel typeDocumentViewModel);
        bool DeleteTypeDocument(int id);

        #endregion

        #region TypeClient

        List<TypeClientViewModel> GetAllTypeClient();
        TypeClientViewModel GetTypeClientById(int id);
        TypeClientViewModel AddTypeClient(TypeClientViewModel newTypeClientViewModel);
        bool UpdateTypeClient(TypeClientViewModel typeClientViewModel);
        bool DeleteTypeClient(int id);

        #endregion

        #region DailySigning

        QueryResult<DailySigningViewModel> GetAllDailySigning(int skip = 0, int take = 0, int userId = 0);
        DailySigningViewModel GetDailySigningById(int id);
        DailySigningViewModel AddDailySigning(DailySigningViewModel newDailySigningViewModel);
        bool UpdateDailySigning(DailySigningViewModel dailySigningViewModel);
        bool DeleteDailySigning(int id);
        bool MassiveSigning(MassiveSigningQueryViewModel massiveSigningQueryViewModel);

        #endregion

        #region WorkerHiring

        QueryResult<WorkerHiringViewModel> GetAllWorkerHiring(int skip = 0, int take = 0, string filter = null, int workId = 0);

        #endregion

        #region Report

        List<ReportResultViewModel> GetHoursByUser(ReportQueryViewModel reportViewModel);
        List<ReportResultViewModel> GetHoursByWork(ReportQueryViewModel reportViewModel);
        List<ReportResultViewModel> GetHoursByClient(ReportQueryViewModel reportViewModel);

        #endregion

        #region InvoiceResponse

        InvoiceResponseViewModel GetInvoice(InvoiceQueryViewModel invoiceQueryViewModel);

        #endregion

        #region Setting

        List<SettingViewModel> GetAllSetting();
        SettingViewModel GetSettingById(int id);
        SettingViewModel GetSettingByName(string name);
        SettingViewModel AddSetting(SettingViewModel newSettingViewModel);
        bool UpdateSetting(SettingViewModel settingViewModel);
        bool DeleteSetting(int id);

        #endregion

        #region ProfessionInClient

        QueryResult<ProfessionInClientViewModel> GetAllProfessionInClient(int skip = 0, int take = 0, string filter = null, int professionId = 0, int clientId = 0);
        ProfessionInClientViewModel GetProfessionInClientById(int id);
        ProfessionInClientViewModel AddProfessionInClient(ProfessionInClientViewModel newProfessionInClientViewModel);
        bool UpdateProfessionInClient(ProfessionInClientViewModel professionInClientViewModel);
        bool DeleteProfessionInClient(int id);

        #endregion

        #region HourType

        List<HourTypeViewModel> GetAllHourType();
        HourTypeViewModel GetHourTypeById(int id);
        HourTypeViewModel AddHourType(HourTypeViewModel newHourTypeViewModel);
        bool UpdateHourType(HourTypeViewModel hourTypeViewModel);
        bool DeleteHourType(int id);

        #endregion

        #region CostWorker

        QueryResult<CostWorkerViewModel> GetAllCostWorker(int skip = 0, int take = 0, string filter = null, int userId = 0);
        CostWorkerViewModel GetCostWorkerById(int id);
        CostWorkerViewModel AddCostWorker(CostWorkerViewModel newCostWorkerViewModel);
        bool UpdateCostWorker(CostWorkerViewModel costWorkerViewModel);
        bool DeleteCostWorker(int id);

        #endregion

        #region Invoice

        QueryResult<InvoiceViewModel> GetAllInvoice(int skip = 0, int take = 0, string filter = null, int workId = 0, int clientId = 0);
        InvoiceViewModel GetInvoiceById(int id);
        InvoiceViewModel AddInvoice(InvoiceViewModel newInvoiceViewModel);
        bool UpdateInvoice(InvoiceViewModel invoiceViewModel);
        bool DeleteInvoice(int id);

        #endregion

        #region DetailInvoice

        List<DetailInvoiceViewModel> GetAllDetailInvoice();
        DetailInvoiceViewModel GetDetailInvoiceById(int id);
        DetailInvoiceViewModel AddDetailInvoice(DetailInvoiceViewModel newDetailInvoiceViewModel);
        bool UpdateDetailInvoice(DetailInvoiceViewModel detailInvoiceViewModel);
        bool DeleteDetailInvoice(int id);

        #endregion

        #region Others

        Client GetClient(int clientId);

        Work GetWork(int workId);

        User GetWorker(int userId);

        #endregion
    }
}
