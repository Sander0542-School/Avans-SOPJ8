using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Seeder.Data;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Seeder
{
    public class TestDataSeeder
    {
        private List<Branch> _branches;
        private List<User> _users;
        private List<IdentityUserRole<int>> _userRoles;
        private List<BranchSchedule> _shifts;

        public IEnumerable<Branch> Branches => _branches ??= new BranchSeeder().Get();
        public IEnumerable<User> Users => _users ??= new UserSeeder().Get();
        public IEnumerable<IdentityUserRole<int>> UserRoles => _userRoles ??= new UserSeeder().GetRoles();
        public IEnumerable<BranchSchedule> Shifts => _shifts ??= new ShiftSeeder().Get();
    }
}
