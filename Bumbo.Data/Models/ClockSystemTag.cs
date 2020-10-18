using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models
{
    public class ClockSystemTag
    {
        [Key]
        [StringLength(20)]
        public string SerialNumber { get; set; }
        
        public int UserId { get; set; }
        
        
        public IdentityUser User { get; set; }
    }
}