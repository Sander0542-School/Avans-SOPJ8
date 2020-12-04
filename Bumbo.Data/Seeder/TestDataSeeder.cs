using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Seeder.Data;

namespace Bumbo.Data.Seeder
{
    public class TestDataSeeder
    {
        private List<Branch> _branches;
        private List<User> _users;
        private List<BranchSchedule> _shifts;

        public List<Branch> Branches => _branches ??= new BranchSeeder().Get();
        public List<User> Users => _users ??= new UserSeeder().Get();
        public List<BranchSchedule> Shifts => _shifts ??= new ShiftSeeder().Get();
    }
}