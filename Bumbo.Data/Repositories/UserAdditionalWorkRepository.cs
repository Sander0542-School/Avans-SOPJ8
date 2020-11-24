using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class UserAdditionalWorkRepository : RepositoryBase<UserAdditionalWork>
    {
        public UserAdditionalWorkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}