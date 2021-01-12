﻿using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
namespace Bumbo.Data.Repositories
{
    public class UserAvailabilityRepository : RepositoryBase<UserAvailability>
    {
        public UserAvailabilityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
