using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models.Validators
{
    public class BuildingNumberAttribute : RegularExpressionAttribute
    {
        public BuildingNumberAttribute() : base(@"(?i:^[1-9][0-9]{0,4}[a-z]{0,2}$)")
        {
        }
    }
}