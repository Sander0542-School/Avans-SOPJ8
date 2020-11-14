using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Models
{
    public class User : IdentityUser<int>, IEntity
    {
        /// <summary>
        /// Gets or sets the first name for this user.
        /// </summary>
        [PersonalData]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets the middle name for this user.
        /// </summary>
        [PersonalData]
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Gets or sets the last name for this user.
        /// </summary>
        [PersonalData]
        public string LastName { get; set; }
        
        /// <summary>
        /// Gets or sets the birthday for this user.
        /// </summary>
        [PersonalData]
        public DateTime Birthday { get; set; }
        
        /// <summary>
        /// Gets or sets the birthday for this user.
        /// </summary>
        [PersonalData]
        [StringLength(7, MinimumLength = 6)]
        public string ZipCode { get; set; }
        
        /// <summary>
        /// Gets or sets the house number for this user.
        /// </summary>
        [PersonalData]
        [StringLength(7, MinimumLength = 1)]
        public string HouseNumber { get; set; }
        
        
        public IList<UserAvailability> UserAvailabilities { get; set; }
        
        public IList<UserAdditionalWork> UserAdditionalWorks { get; set; }
        
        public IList<ClockSystemTag> ClockSystemTags { get; set; }
        
        public IList<Shift> Shifts { get; set; }
        
        public IList<UserBranch> Branches { get; set; }
    }
}