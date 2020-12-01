using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class BranchRepository : RepositoryBase<Branch>
    {
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}