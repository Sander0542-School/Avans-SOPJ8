using Bumbo.Data.Repositories;
namespace Bumbo.Data
{
    public class RepositoryWrapper
    {
        private readonly ApplicationDbContext _repositoryContext;

        private BranchRepository _branch;
        private BranchForecastStandardRepository _branchForecastStandard;
        private BranchManagerRepository _branchManager;
        private BranchScheduleRepository _branchSchedule;
        private ClockSystemTagRepository _clockSystemTag;
        private ForecastRepository _forecast;
        private ForecastStandardRepository _forecastStandard;
        private FurloughRepository _furlough;
        private ShiftRepository _shift;
        private UserRepository _user;
        private UserAdditionalWorkRepository _userAdditionalWork;
        private UserAvailabilityRepository _userAvailability;
        private UserBranchRepository _userBranch;
        private WorkedShiftRepository _workedShift;
        public RepositoryWrapper(ApplicationDbContext context)
        {
            _repositoryContext = context;
        }

        public BranchRepository Branch => _branch ??= new BranchRepository(_repositoryContext);
        public BranchScheduleRepository BranchSchedule => _branchSchedule ??= new BranchScheduleRepository(_repositoryContext);
        public ClockSystemTagRepository ClockSystemTag => _clockSystemTag ??= new ClockSystemTagRepository(_repositoryContext);
        public ForecastRepository Forecast => _forecast ??= new ForecastRepository(_repositoryContext);
        public UserRepository User => _user ??= new UserRepository(_repositoryContext);
        public ShiftRepository Shift => _shift ??= new ShiftRepository(_repositoryContext);
        public UserAdditionalWorkRepository UserAdditionalWork => _userAdditionalWork ??= new UserAdditionalWorkRepository(_repositoryContext);
        public UserAvailabilityRepository UserAvailability => _userAvailability ??= new UserAvailabilityRepository(_repositoryContext);
        public WorkedShiftRepository WorkedShift => _workedShift ??= new WorkedShiftRepository(_repositoryContext);
        public BranchForecastStandardRepository BranchForecastStandard => _branchForecastStandard ??= new BranchForecastStandardRepository(_repositoryContext);
        public ForecastStandardRepository ForecastStandard => _forecastStandard ??= new ForecastStandardRepository(_repositoryContext);
        public BranchManagerRepository BranchManager => _branchManager ??= new BranchManagerRepository(_repositoryContext);
        public UserBranchRepository UserBranch => _userBranch ??= new UserBranchRepository(_repositoryContext);
        public FurloughRepository Furlough => _furlough ??= new FurloughRepository(_repositoryContext);
    }
}
