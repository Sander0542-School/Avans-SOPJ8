using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Schedule
{
	public class UserAdditionalWorkViewModel : ValidationAttribute
	{
		public List<UserAdditionalWork> Schedule { get; set; }
		public UserAdditionalWork Work { get; set; }

		public ValidationResult Validate(ValidationContext validationContext)
        {
			var additionalWork = (UserAdditionalWork)validationContext.ObjectInstance;

			if (additionalWork.StartTime > additionalWork.EndTime)
            {
				return new ValidationResult("Starttime must be before endtime");
            }

			return ValidationResult.Success;
        }

		public class InputAdditionalWork : AdditionalWork
		{
			public static readonly DayOfWeek[] DaysOfWeek =
			{
				DayOfWeek.Monday,
				DayOfWeek.Tuesday,
				DayOfWeek.Wednesday,
				DayOfWeek.Thursday,
				DayOfWeek.Friday,
				DayOfWeek.Saturday,
				DayOfWeek.Sunday
			};
		}
        
		public abstract class AdditionalWork
		{
			public int UserId { get; set; }

			[Display(Name = "Day")]
			public DayOfWeek Day { get; set; }

			[Display(Name = "StartTime")]
			[DataType(DataType.Time)]
			public TimeSpan StartTime { get; set; }

			[Display(Name = "EndTime")]
			[DataType(DataType.Time)]
			public TimeSpan EndTime { get; set; }
		}
	}
}