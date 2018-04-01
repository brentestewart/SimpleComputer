using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleComputer.Controls
{
	public sealed partial class CalendarDayControl : UserControl
	{
		public CalendarDayControl()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(String),
			typeof(CalendarDayControl),
			new PropertyMetadata(string.Empty)
		);

		public string Text
		{
			get => (String)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}
