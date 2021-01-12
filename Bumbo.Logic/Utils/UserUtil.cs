using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Bumbo.Data.Models;

namespace Bumbo.Logic.Utils
{
    public class UserUtil
    {
        public static int GetAge(User user) => GetAge(user, DateTime.Today);

        public static int GetAge(User user, DateTime date)
        {
            var age = date.Year - user.Birthday.Year;

            if (user.Birthday.Date > date.AddYears(-age)) age--;

            return age;
        }

        public static string GetFullName(User user) => $"{user.FirstName} {(String.IsNullOrWhiteSpace(user.MiddleName) ? "" : user.MiddleName + " ")}{user.LastName}";

        public static IEnumerable<int> GetBranches(ClaimsPrincipal user)
        {
            return GetBranches(user.Claims);
        }

        public static IEnumerable<int> GetBranches(IEnumerable<Claim> claims)
        {
            return claims.Where(claim => claim.Type == "Branch").Select(claim => int.Parse(claim.Value));
        }
    }
}
