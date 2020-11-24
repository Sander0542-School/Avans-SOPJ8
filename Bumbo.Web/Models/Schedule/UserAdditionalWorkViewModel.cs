using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.CompilerServices;

namespace Bumbo.Web.Models.Schedule
{
	public class UserAdditionalWorkViewModel
	{
		public IEnumerable<UserAdditionalWork> Schedule { get; set; }
		public UserAdditionalWork Work { get; set; }

		public class InputAdditionalWork : AdditionalWork
		{
			public String[] weekdays = new string[]
			{
				"Monday",
				"Tuesday",
				"Wednesday",
				"Thursday",
				"Friday",
				"Saturday",
				"Sunday",
			};
		}
        
		public abstract class AdditionalWork
		{
			public int UserId { get; set; }

			[DisplayName("Dag")]
			public MondayFirstDayOfWeek Day { get; set; }

			[DisplayName("Starttijd")]
			[DataType(DataType.Time)]
			public TimeSpan StartTime { get; set; }

			[DisplayName("Eindtijd")]
			[DataType(DataType.Time)]
			public TimeSpan EndTime { get; set; }
		}
	}
}