using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bumbo.Data.Models
{
    public class Forecast : IEntity
    {
        public int BranchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        public Department Department { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal WorkingHours { get; set; }

        public Branch Branch { get; set; }
    }
}