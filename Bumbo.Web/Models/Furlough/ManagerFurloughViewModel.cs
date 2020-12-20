using System;
using System.Collections.Generic;

namespace Bumbo.Web.Models.Furlough
{
    public class ManagerFurloughViewModel 
    {
        public Dictionary<User, List<Furlough>> UserFurloughs { get; set; }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Furlough
        {
            public int Id { get; set; }

            public int UserId { get; set; }

            public string Description { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public bool IsAllDay { get; set; }
        }
    }
}
