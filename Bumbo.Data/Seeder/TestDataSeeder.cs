using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Seeder.Data;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Seeder
{
    public class TestDataSeeder
    {
        public const int SuperId = 1;
        public const int ManagerId = 2;
        public const int EmployeeId = 3;
        
        private List<User> _users;
        private List<IdentityUserRole<int>> _userRoles;
        private List<UserAvailability> _userAvailabilities;
        private List<UserContract> _userContracts;
        private List<ClockSystemTag> _clockSystemTags;
        
        private List<Branch> _branches;
        private List<BranchManager> _branchManagers;
        private List<UserBranch> _branchEmployees;
        private List<BranchSchedule> _shifts;

        public IEnumerable<User> Users => _users ??= new UserSeeder().Get();
        public IEnumerable<UserAvailability> UserAvailabilities => _userAvailabilities ??= new UserSeeder().GetAvailabilities();
        public IEnumerable<UserContract> UserContracts => _userContracts ??= new UserSeeder().GetContracts();
        public IEnumerable<ClockSystemTag> ClockSystemTags => _clockSystemTags ??= new UserSeeder().GetClockSystemTags();
        
        public IEnumerable<Branch> Branches => _branches ??= new BranchSeeder().Get();
        public IEnumerable<BranchManager> BranchManagers => _branchManagers ??= new BranchSeeder().GetManagers();
        public IEnumerable<UserBranch> BranchEmployees => _branchEmployees ??= new BranchSeeder().GetEmployees();
        public IEnumerable<BranchSchedule> Shifts => _shifts ??= new ShiftSeeder().Get();
    }
}
