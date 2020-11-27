using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Schedule
{
	public class UserAdditionalWorkViewModel
	{
		public IEnumerable<UserAdditionalWork> Schedule { get; set; }
		public UserAdditionalWork Work { get; set; }

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

			[Display(Name = "Dag")]
			public DayOfWeek Day { get; set; }

			[Display(Name = "Starttijd")]
			[DataType(DataType.Time)]
			public TimeSpan StartTime { get; set; }

			[Display(Name = "Eindtijd")]
			[DataType(DataType.Time)]
			public TimeSpan EndTime { get; set; }
		}
	}
}