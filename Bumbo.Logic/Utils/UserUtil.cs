using System;
using System.Linq;
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

        public static string GetFullName(User user) => $"{user.FirstName} {(String.IsNullOrWhiteSpace(user.MiddleName) ? "" : user.MiddleName.Concat(" "))}{user.LastName}";
    }
}