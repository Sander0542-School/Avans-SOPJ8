using Bumbo.Data.Models;
using Bumbo.Data.Models.Validators;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Bumbo.Web.Models.Users
{
    public class ContractViewModel
    {
            [Required]
            [Display(Name = "User Id")]
            public int UserId { get; set; }

            [Required]
            [Display(Name = "StartDate")]
            public DateTime StartDate { get; set; }

            [Required]
            [Display(Name = "EndDate")]
            public DateTime EndDate { get; set; }

            [Required]
            [Display(Name = "Function")]
            public string Function { get; set; }

            [Required]
            [Display(Name = "Scale")]
            public int Scale { get; set; }


            public User User { get; set; }
    }


}

