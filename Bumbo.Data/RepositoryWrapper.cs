using Bumbo.Data.Repositories;

namespace Bumbo.Data
{
    public class RepositoryWrapper
    {
        private readonly ApplicationDbContext _repositoryContext;

        private BranchRepository _branch;
        private ClockSystemTagRepository _clockSystemTag;
        private ForecastRepository _forecastRepository;
        private UserRepository _user;
        private ShiftRepository _shift;
        private UserAdditionalWorkRepository _userAdditionalWork;
        private UserAvailabilityRepository _userAvailability;
        private WorkedShiftRepository _workedShift;

        public BranchRepository Branch => _branch ??= new BranchRepository(_repositoryContext);
        public ClockSystemTagRepository ClockSystemTag => _clockSystemTag ??= new ClockSystemTagRepository(_repositoryContext);
        public ForecastRepository ForecastRepository => _forecastRepository ??= new ForecastRepository(_repositoryContext);
        public UserRepository User => _user ??= new UserRepository(_repositoryContext);
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