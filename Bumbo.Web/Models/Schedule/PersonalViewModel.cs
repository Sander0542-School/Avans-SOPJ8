using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bumbo.Web.Models.Schedule
{
    public class PersonalViewModel
    {
        public InputOfferShiftModel InputOfferShift { get; set; }
        
        public class InputOfferShiftModel
        {
            [Required]
            public int ShiftId { get; set; }
            
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            [DataType(DataType.Date)]
            [Column(TypeName = "Date")]
            [Display(Name = "Date")]
            public DateTime Date { get; set; }

            [Display(Name = "Start Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [Display(Name = "End Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }
        }
    }
}
