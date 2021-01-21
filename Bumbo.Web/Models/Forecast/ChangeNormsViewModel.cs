using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Web.Models.Forecast
{
    public class ChangeNormsViewModel : IValidatableObject
    {

        public int BranchId;

        [Range(1, 1000)]
        public int ForecastStandardValue;

        public SortedDictionary<ForecastActivity, int> Standards { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var (key, value) in Standards)
            {
                if (value < 1 || value > 1000)
                {
                    yield return new ValidationResult($"{key} needs to be between 1 and 1000");
                }
            }
        }
    }
}
