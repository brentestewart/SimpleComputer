using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SimpleComputer.Models;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleComputer.Controls
{
	public sealed partial class CalendarControl : UserControl
	{
		public static readonly DependencyProperty DaysProperty = DependencyProperty.Register(
			"Days",
			typeof(List<CalendarDay>),
			typeof(CalendarControl),
			new PropertyMetadata(new List<CalendarDay>())
		);

		public List<CalendarDay> Days
		{
			get => (List<CalendarDay>)GetValue(DaysProperty);
			set => SetValue(DaysProperty, value);
		}

		public string Test => "A";

		public CalendarControl()
		{
			this.InitializeComponent();
		}

	}
}
