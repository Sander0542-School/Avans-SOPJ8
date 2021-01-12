using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Data.Models.Common
{
    public interface IForecastStandard : IEntity
    {
        /// <summary>
        ///     Enum for the type of activity
        /// </summary>
        [DataType("int")]
        public ForecastActivity Activity { get; set; }

        /// <summary>
        ///     Value for converting activities to a prognosis. Depending on the activity this value has a different meaning.
        ///     Value means the following for these <c>ForecastActivity</c>
        ///     UNLOAD_COLI: minutes per coli
        ///     STOCK_SHELVES: minutes per coli
        ///     CASHIER: customers for one cashier per hour
        ///     PRODUCE_DEPARTMENT: customers per employee per hour
        ///     FACE_SHELVES: seconds per meter of faced shelf
        /// </summary>
        public int Value { get; set; }
    }
}
