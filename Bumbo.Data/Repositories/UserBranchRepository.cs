﻿using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
namespace Bumbo.Data.Repositories
{
    public class UserBranchRepository : RepositoryBase<UserBranch>
    {
        public UserBranchRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<UserBranch> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(ub => ub.User);
        }
    }
}
