using System.ComponentModel.DataAnnotations;
namespace Bumbo.Data.Models.Validators
{
    public class BuildingNumberAttribute : RegularExpressionAttribute
    {
        // TODO: Make regex case insensitive
        public BuildingNumberAttribute() : base(@"(^[1-9][0-9]{0,4}[a-z]{0,2}$)")
        {
            ErrorMessage = "The building number is not valid (example: 104a)";
        }
    }
}
