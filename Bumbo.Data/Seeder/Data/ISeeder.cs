using System.Collections.Generic;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Seeder.Data
{
    public interface ISeeder<T> where T : class,IEntity
    {
        List<T> Get();
    }
}