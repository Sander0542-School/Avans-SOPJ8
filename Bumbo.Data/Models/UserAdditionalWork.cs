using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models
{
    public class UserAdditionalWork
    {
        public int UserId { get; set; }

        [Range(1, 7)]
        public int Day { get; set; }

        [Range(0, 24)]
        public double Hours { get; set; }


        public IdentityUser User { get; set; }
    }
}