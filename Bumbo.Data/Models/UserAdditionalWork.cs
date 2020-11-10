using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class UserAdditionalWork : IEntity
    {
        public int UserId { get; set; }

        [Range(1, 7)]
        public int Day { get; set; }

        [Range(0, 24)]
        public double Hours { get; set; }


        public User User { get; set; }
    }
}