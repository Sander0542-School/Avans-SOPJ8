using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Data.Models
{
    public class UserBranch : IEntity
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }

        public Department Department { get; set; }

        public User User { get; set; }
        public Branch Branch { get; set; }
    }
}
