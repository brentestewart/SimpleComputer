using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using SimpleComputer.Models;

namespace SimpleComputer.ViewModels
{
	public class CalendarPageViewModel : BaseViewModel
	{
		public string DayOfTheWeek => DateTime.Now.DayOfWeek.ToString();
		public string Date => DateTime.Now.ToString("M");
		public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();

		public List<CalendarDay> Days { get; set; } = new List<CalendarDay>();

		public CalendarPageViewModel()
		{
			RefreshAppointments();
			BuildDays();
		}

		private void BuildDays()
		{
			var currentYear = DateTime.Now.Year;
			var currentMonth = DateTime.Now.Month;
			var currentDayOfMonth = DateTime.Now.Day;
			var numOfStartPadDays = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek;
			var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
			var numOfEndPadDays = 42 - numOfStartPadDays - daysInMonth;

			for (int i = 0; i < numOfStartPadDays; i++)
			{
				Days.Add(new CalendarDay() { Text = "" });
			}

			for (int i = 0; i < daysInMonth; i++)
			{
				Days.Add(new CalendarDay { Text = (i + 1).ToString(), Date = new DateTime(currentYear, currentMonth, i+1) });
			}

			for (int i = 0; i < numOfEndPadDays; i++)
			{
				Days.Add(new CalendarDay() { Text = "" });
			}
		}

		private void RefreshAppointments()
		{
			Appointments.Clear();
			Appointments.Add(new Appointment()
			{
				Time = DateTime.Now.Date.AddHours(17),
				Title = "Go shopping with Carolyn"
			});
		}
	}
}