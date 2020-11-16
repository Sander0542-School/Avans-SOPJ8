using System;
using Bumbo.Data.Models;

namespace Bumbo.Logic.Utils
{
    public class UserUtil
    {
        public static int GetAge(User user)
        {
            return GetAge(user, DateTime.Today);
        }
        
        public static int GetAge(User user, DateTime date)
        {
            var age = date.Year - user.Birthday.Year;

            if (user.Birthday.Date > date.AddYears(-age)) age--;

            return age;
        }
    }
}