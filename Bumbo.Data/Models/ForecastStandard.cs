using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class ForecastStandard : IEntity
    {
        [Key]
        public ForecastActivity Activity { get; set; }


        /// <summary>
        /// Value for converting activities to a prognosis. Depending on the activity this value has a different meaning.
        /// Value means the following for these <c>ForecastActivity</c>
        /// UNLOAD_COLI: minutes per coli
        /// STOCK_SHELVES: minutes per coli
        /// CASHIER: customers for one cashier per hour
        /// PRODUCE_DEPARTMENT: customers per employee per hour
        /// FACE_SHELVES: seconds per meter of faced shelf
        /// </summary>
        public int Value { get; set; }
        
        
        public IList<BranchForecastStandard> BranchForecastStandards { get; set; }
    }
}