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
    public class DetailsViewModel
    {

        public class EditUserViewModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            
           

            [Required]
            [Display(Name = "Id")]
            public int Id { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Middle name")]
            public string MiddleName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Birthday")]
            public DateTime Birthday { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [ZipCode]
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [BuildingNumber]
            [Display(Name = "House number")]
            public string HouseNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public IList<UserContract> Contracts { get; set; }

            public IList<UserBranch> UserBranches { get; set; }

            public IList<SelectListItem> Branches { get; set; }

        }

        public class CreateUserViewModel
        {

            

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Middle name")]
            public string MiddleName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Birthday")]
            public DateTime Birthday { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [ZipCode]
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [BuildingNumber]
            [Display(Name = "House number")]
            public string HouseNumber { get; set; }

            public IList<UserContract> Contracts { get; set; }

            public IList<UserBranch> Branches { get; set; }

        }


    }
}
