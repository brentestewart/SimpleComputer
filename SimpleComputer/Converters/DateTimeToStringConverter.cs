using System;
using Windows.UI.Xaml.Data;

namespace SimpleComputer.Converters
{
	public class DateTimeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(!(value is DateTime dt)) return string.Empty;

			return dt.ToString("M");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}