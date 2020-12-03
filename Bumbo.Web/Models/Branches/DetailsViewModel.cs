using Bumbo.Data.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.Web.Models.Branches
{
    public class DetailsViewModel
    {
        public Branch Branch { get; set; }
        public List<User> Managers { get; set; }
        public int CurrentUserId { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Manager's email address")]
        public string UserEmail { get; set; }
    }
}