using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;

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

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "StartDate")]
            public DateTime StartDate { get; set; }

            [Display(Name = "EndDate")]
            public DateTime EndDate { get; set; }

            public FurloughStatus Status { get; set; }

            [Display(Name = "IsAllDay")]
            public bool IsAllDay { get; set; }
        }
    }
}
