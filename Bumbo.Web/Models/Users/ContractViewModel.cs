using System;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;


namespace Bumbo.Web.Models.Users
{
    public class ContractViewModel
    {
        [Required]
        [Display(Name = "User Id")]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
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

