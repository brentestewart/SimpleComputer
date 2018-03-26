using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SimpleComputer.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var invert = (parameter is string parm && parm == "inverse");

			if (!(value is bool show)) return invert ? Visibility.Visible : Visibility.Collapsed;

			show = invert ? !show : show;
			return show ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
