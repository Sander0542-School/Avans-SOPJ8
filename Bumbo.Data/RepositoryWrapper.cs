using Bumbo.Data.Repositories;

namespace Bumbo.Data
{
    public class RepositoryWrapper
    {
        private readonly ApplicationDbContext _repositoryContext;

        private BranchForecastStandardRepository _branchForecastStandard;
        private BranchRepository _branch;
        private ClockSystemTagRepository _clockSystemTag;
        private ForecastRepository _forecastRepository;
        private ForecastStandardRepository _forecastStandard;
        private IdentityUserRepository _identityUser;
        private ShiftRepository _shift;
        private UserAdditionalWorkRepository _userAdditionalWork;
        private UserAvailabilityRepository _userAvailability;
        private WorkedShiftRepository _workedShift;

        public BranchForecastStandardRepository BranchForecastStandard => _branchForecastStandard ??= new BranchForecastStandardRepository(_repositoryContext);
        public BranchRepository Branch => _branch ??= new BranchRepository(_repositoryContext);
        public ClockSystemTagRepository ClockSystemTag => _clockSystemTag ??= new ClockSystemTagRepository(_repositoryContext);
        public ForecastRepository ForecastRepository => _forecastRepository ??= new ForecastRepository(_repositoryContext);
        public ForecastStandardRepository ForecastStandard => _forecastStandard ??= new ForecastStandardRepository(_repositoryContext);
        public IdentityUserRepository IdentityUser => _identityUser ??= new IdentityUserRepository(_repositoryContext);
        public ShiftRepository Shift => _shift ??= new ShiftRepository(_repositoryContext);
        public UserAdditionalWorkRepository UserAdditionalWork => _userAdditionalWork ??= new UserAdditionalWorkRepository(_repositoryContext);
        public UserAvailabilityRepository UserAvailability => _userAvailability ??= new UserAvailabilityRepository(_repositoryContext);
        public WorkedShiftRepository WorkedShift => _workedShift ??= new WorkedShiftRepository(_repositoryContext);

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _repositoryContext = context;
        }
    }
}