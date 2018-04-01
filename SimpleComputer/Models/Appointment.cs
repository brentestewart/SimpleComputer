using System;

namespace SimpleComputer.Models
{
	public class Appointment
	{
		public DateTime Time { get; set; }
		public string Title { get; set; }
	}

	public class CalendarDay
	{
		public string Text { get; set; }
	}
}