using System.Collections.Generic;
using Bumbo.Data.Models;

namespace Bumbo.Data.Seeder.Data
{
    public class BranchSeeder : ISeeder<Branch>
    {
        public List<Branch> Get()
        {
            return new()
            {
                new Branch
                {
                    Id = 1,
                    Name = "Test Geldrop",
                    ZipCode = "5664 ex",
                    HouseNumber = "103",
                },
                new Branch
                {
                    Id = 2,
                    Name = "Test Veldhoven",
                    ZipCode = "5508vl",
                    HouseNumber = "22a"
                },
                new Branch
                {
                    Id = 3,
                    Name = "Test Groningen",
                    ZipCode = "3456 CD",
                    HouseNumber = "184c"
                }
            };
        }
    }
}
