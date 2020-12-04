using System.Collections.Generic;
using Bumbo.Data.Models;

namespace Bumbo.Data.Seeder.Data
{
    public class BranchSeeder : ISeeder<Branch>
    {
        public List<Branch> Get()
        {
            return new List<Branch>
            {
                new Branch
                {
                    Id = 1,
                    Name = "Test Bumbo 1",
                    ZipCode = "1234 AB",
                    HouseNumber = "1",
                },
                new Branch
                {
                    Id = 2,
                    Name = "Test Bumbo 2",
                    ZipCode = "2345 BC",
                    HouseNumber = "2a"
                },
                new Branch
                {
                    Id = 3,
                    Name = "Test Bumbo 3",
                    ZipCode = "3456 CD",
                    HouseNumber = "3b"
                }
            };
        }
    }
}