using Bumbo.Data.Models;
using System;

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
    }
}