using System.ComponentModel.DataAnnotations;
namespace Bumbo.Data.Models.Validators
{
    public class ZipCodeAttribute : RegularExpressionAttribute
    {
        // TODO: Make regex case insensitive
        public ZipCodeAttribute() : base(@"(^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$)")
        {
            ErrorMessage = "The zip code is not a valid zip code (example: 1234 ab)";
        }
    }
}
