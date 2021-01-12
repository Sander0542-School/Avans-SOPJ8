using System;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class Furlough : BaseEntity
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }

        [StringLength(60, MinimumLength = 5)]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]

        public DateTime EndDate { get; set; }

        public bool IsAllDay { get; set; }

        public FurloughStatus Status { get; set; }

        public double Balance { get; set; }

        public User User { get; set; }
    }
}
