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
		public DateTime Date { get; set; }
		public bool IsToday => DateTime.Now.Date == Date.Date;
		public string Text { get; set; }
	}
}