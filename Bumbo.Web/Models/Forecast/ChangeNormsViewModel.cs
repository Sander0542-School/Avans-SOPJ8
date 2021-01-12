using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Forecast
{
    public class ChangeNormsViewModel : IValidatableObject
    {
        public SortedDictionary<ForecastActivity, int> Standards { get; set; }

        [Range(1, 30)]
        public int ForecastStandardValue;

        public int BranchId;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var (key, value) in Standards)
            {
                if (value < 1 || value > 30)
                {
                    yield return new ValidationResult($"{key} needs to be between 1 and 30");
                }
            }
        }
    }
}
