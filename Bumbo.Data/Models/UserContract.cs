using Bumbo.Data.Models.Common;
using System;

namespace Bumbo.Data.Models
{
    public class UserContract : BaseEntity
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Function { get; set; }

        public int Scale { get; set; }


        public User User { get; set; }
    }
}