using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Furlough
{
    public class FurloughViewModel
    {
        public List<Data.Models.Furlough> Furloughs { get; set; }

        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Description { get; set; }

            [DataType(DataType.DateTime)]
            [Required]
            public DateTime StartDate { get; set; }

            [DataType(DataType.DateTime)]
            [Required]
            public DateTime EndDate { get; set; }

            public bool IsAllDay { get; set; }
        }
    }
}
