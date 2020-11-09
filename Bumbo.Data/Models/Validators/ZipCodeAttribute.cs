using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models.Validators
{
    public class ZipCodeAttribute : RegularExpressionAttribute
    {
        public ZipCodeAttribute() : base(@"(?i:^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$)")
        {
        }
    }
}