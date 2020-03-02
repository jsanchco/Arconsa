// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{
    #region Using

    using Microsoft.Extensions.Options;
    using Helpers;
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
            IDailySigningRepository dailySigningRepository)
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
        }
    }
}
