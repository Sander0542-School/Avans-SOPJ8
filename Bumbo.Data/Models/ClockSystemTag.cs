using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class ClockSystemTag : IEntity
    {
        [Key]
        [StringLength(20)]
        public string SerialNumber { get; set; }
        
        public int UserId { get; set; }
        
        
        public User User { get; set; }
    }
}