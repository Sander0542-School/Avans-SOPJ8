using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Forecast
{
    public class ChangeNormsViewModel
    {
        public SortedDictionary<ForecastActivity, int> Standards { get; set; }

        [Range(1, 30)]
        public int ForecastStandardValue;

        public int BranchId;
    }
}
