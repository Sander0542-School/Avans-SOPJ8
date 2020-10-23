using System;
using System.ComponentModel.DataAnnotations.Schema;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class Forecast
    {
        public int BranchId { get; set; }

        public DateTime Date { get; set; }

        public Department Department { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double WorkingHours { get; set; }


        public Branch Branch { get; set; }
    }
}