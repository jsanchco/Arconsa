// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{   
    #region Using

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ViewModels;
    using Domain.Helpers;

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

        QueryResult<UserViewModel> GetAllUsers(int skip = 0, int take = 0, string filter = null);
        UserViewModel GetUserById(int id);
        UserViewModel AddUser(UserViewModel newUserViewModel);
        bool UpdateUser(UserViewModel userViewModel);
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

        List<ClientViewModel> GetAllClient();
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

        List<UserHiringViewModel> GetAllUserHiring();
        UserHiringViewModel GetUserHiringById(int id);
        UserHiringViewModel AddUserHiring(UserHiringViewModel newUserHiringViewModel);
        bool UpdateUserHiring(UserHiringViewModel userHiringViewModel);
        bool DeleteUserHiring(int id);

        #endregion

        #region Work

        List<WorkViewModel> GetAllWork();
        WorkViewModel GetWorkById(int id);
        WorkViewModel AddWork(WorkViewModel newWorkViewModel);
        bool UpdateWork(WorkViewModel workViewModel);
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

        List<DailySigningViewModel> GetAllDailySigning();
        DailySigningViewModel GetDailySigningById(int id);
        DailySigningViewModel AddDailySigning(DailySigningViewModel newDailySigningViewModel);
        bool UpdateDailySigning(DailySigningViewModel dailySigningViewModel);
        bool DeleteDailySigning(int id);

        #endregion
    }
}
