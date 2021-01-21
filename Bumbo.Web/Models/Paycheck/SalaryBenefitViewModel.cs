using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Logic.PayCheck;
namespace Bumbo.Web.Models.Paycheck
{
    public class SalaryBenefitViewModel
    {
        public Dictionary<User, PayCheck> PayChecks { get; set; }
    }
}
