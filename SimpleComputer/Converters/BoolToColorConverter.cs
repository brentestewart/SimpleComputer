using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SimpleComputer.Converters
{
	public class BoolToColorConverter : IValueConverter
	{
		public SolidColorBrush TrueColor { get; set; }
		public SolidColorBrush FalseColor { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((value is bool isTrue) && isTrue) ? TrueColor : FalseColor;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}