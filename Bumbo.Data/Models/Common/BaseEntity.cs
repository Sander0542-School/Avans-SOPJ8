using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}