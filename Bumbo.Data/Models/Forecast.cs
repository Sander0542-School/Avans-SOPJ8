using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class Forecast : IEntity
    {
        public int BranchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }

        public Department Department { get; set; }
        
        [Column(TypeName = "decimal(5, 2)")]
        public decimal WorkingHours { get; set; }

        public Branch Branch { get; set; }
    }
}