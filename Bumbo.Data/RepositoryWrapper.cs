using Bumbo.Data.Repositories;

namespace Bumbo.Data
{
    public class RepositoryWrapper
    {
        private readonly ApplicationDbContext _repositoryContext;

        private BranchRepository _branch;
        private ClockSystemTagRepository _clockSystemTag;
        private ForecastRepository _forecast;
        private UserRepository _user;
        private ShiftRepository _shift;
        private UserAdditionalWorkRepository _userAdditionalWork;
        private UserAvailabilityRepository _userAvailability;
        private WeekScheduleRepository _weekSchedule;
        private WorkedShiftRepository _workedShift;
        private BranchForecastStandardRepository _branchForecastStandard;

        public BranchRepository Branch => _branch ??= new BranchRepository(_repositoryContext);
        public ClockSystemTagRepository ClockSystemTag => _clockSystemTag ??= new ClockSystemTagRepository(_repositoryContext);
        public ForecastRepository Forecast => _forecast ??= new ForecastRepository(_repositoryContext);
        public UserRepository User => _user ??= new UserRepository(_repositoryContext);
        public ShiftRepository Shift => _shift ??= new ShiftRepository(_repositoryContext);
        public UserAdditionalWorkRepository UserAdditionalWork => _userAdditionalWork ??= new UserAdditionalWorkRepository(_repositoryContext);
        public UserAvailabilityRepository UserAvailability => _userAvailability ??= new UserAvailabilityRepository(_repositoryContext);
        public WeekScheduleRepository WeekSchedule => _weekSchedule ??= new WeekScheduleRepository(_repositoryContext);
        public WorkedShiftRepository WorkedShift => _workedShift ??= new WorkedShiftRepository(_repositoryContext);
        public BranchForecastStandardRepository BranchForecastStandard => _branchForecastStandard ??= new BranchForecastStandardRepository(_repositoryContext);


        public RepositoryWrapper(ApplicationDbContext context)
        {
            _repositoryContext = context;
        }
    }
}