using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bumbo.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
          
        }

        protected override IQueryable<User> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(user => user.Contracts)
                .Include(user => user.Branches);
        }
    }
}