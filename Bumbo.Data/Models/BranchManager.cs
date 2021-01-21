using Bumbo.Data.Models.Common;
namespace Bumbo.Data.Models
{
    public class BranchManager : IEntity
    {
        public int UserId { get; set; }

        public int BranchId { get; set; }


        public User User { get; set; }

        public Branch Branch { get; set; }
    }
}
