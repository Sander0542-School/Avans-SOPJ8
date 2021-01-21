using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

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
                },
                
                
                new Branch
                {
                    Id = AcceptanceTestData.HinthamId,
                    Name = "Hintham",
                    ZipCode = "5246br",
                    HouseNumber = "16"
                }
            };
        }
        public List<BranchManager> GetManagers()
        {
            return new List<BranchManager>
            {
                new()
                {
                    UserId = TestDataSeeder.SuperId, BranchId = 1
                },
                new()
                {
                    UserId = TestDataSeeder.SuperId, BranchId = 3
                },
                new()
                {
                    UserId = TestDataSeeder.ManagerId, BranchId = 1
                },
                new()
                {
                    UserId = TestDataSeeder.ManagerId, BranchId = 2
                },
            };
        }

        public List<UserBranch> GetEmployees()
        {
            return new List<UserBranch>
            {
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Department = Department.VAK,
                    BranchId = 1
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Department = Department.KAS,
                    BranchId = 1
                },
                
                
                new()
                {
                    UserId = AcceptanceTestData.StijnId,
                    Department = Department.VAK,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.JosId,
                    Department = Department.VAK,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.JosId,
                    Department = Department.VER,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.StefanId,
                    Department = Department.KAS,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.StefanId,
                    Department = Department.VER,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.EricId,
                    Department = Department.KAS,
                    BranchId = AcceptanceTestData.HinthamId
                },
                new()
                {
                    UserId = AcceptanceTestData.EricId,
                    Department = Department.VER,
                    BranchId = AcceptanceTestData.HinthamId
                }
            };
        }
    }
}
