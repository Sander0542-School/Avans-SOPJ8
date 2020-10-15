using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models.Common
{
    /// <summary>
    /// Base class for all non many to many based models.
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}