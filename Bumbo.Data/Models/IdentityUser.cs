using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Models
{
    public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        /// <summary>
        /// Gets or sets the first name for this user.
        /// </summary>
        [ProtectedPersonalData]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets the middle name for this user.
        /// </summary>
        [ProtectedPersonalData]
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Gets or sets the last name for this user.
        /// </summary>
        [ProtectedPersonalData]
        public string LastName { get; set; }
        
        /// <summary>
        /// Gets or sets the birthday for this user.
        /// </summary>
        [ProtectedPersonalData]
        public DateTime Birthday { get; set; }
        
        /// <summary>
        /// Gets or sets the birthday for this user.
        /// </summary>
        [ProtectedPersonalData]
        [StringLength(7, MinimumLength = 6)]
        [RegularExpression(@"^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-zA-Z]{2}$")]
        [Required(ErrorMessage = "Zip Code is Required.")]
        public string ZipCode { get; set; }
        
        /// <summary>
        /// Gets or sets the house number for this user.
        /// </summary>
        [ProtectedPersonalData]
        [StringLength(7, MinimumLength = 1)]
        [RegularExpression(@"^[1-9][0-9]{0,4}[a-z]{0,2}$")]
        public string HouseNumber { get; set; }
        
        
        public List<UserAvailability> UserAvailabilities { get; set; }
        
        public List<UserAdditionalWork> UserAdditionalWorks { get; set; }
    }
}