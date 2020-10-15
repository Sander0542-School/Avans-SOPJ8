using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models.Common
{
    /// <summary>
    /// Base class for a model that facilitates a many to many relationship between 2 <see cref="BaseEntity"/> classes
    /// </summary>
    public abstract class ManyToManyBaseEntity
    {
        /// <summary>
        /// Foreign key of <see cref="Type1"/>
        /// </summary>
        [Key]
        public int Type1Id { get; set; }

        /// <summary>
        /// Foreign key of <see cref="Type2"/>
        /// </summary>
        [Key]
        public int Type2Id { get; set; }

        /// <summary>
        /// The base type should be overriden by one of the two <see cref="BaseEntity"/> types which this class connects.
        /// </summary>
        public virtual BaseEntity Type1 { get; set; }
        /// <inheritdoc cref="Type1"/>
        public virtual BaseEntity Type2 { get; set; }
    }
}