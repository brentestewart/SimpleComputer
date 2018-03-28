using SimpleComputer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleComputer.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class WebPage : Page
	{
		public WebPage()
		{
			this.InitializeComponent();
			this.DataContextChanged += UpdateViewModel;
		}

		private void UpdateViewModel(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			if (!(DataContext is WebPageViewModel vm)) return;

			vm.SearchAction = NavigateToWebsite;
			vm.GoBackAction = GoBack;
			vm.InitializeSearch = InitializeSearch;
		}

		private void InitializeSearch()
		{
			Task.Factory.StartNew(
				() => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
					() => SearchTextBox.Focus(FocusState.Programmatic)));
		}

		public void NavigateToWebsite(Uri site)
		{
			try
			{
				MainWebView.Navigate(site);
			}
			catch (Exception)
			{
			}
		}

		public void GoBack()
		{
			if (MainWebView.CanGoBack)
			{
				MainWebView.GoBack();
			}
		}

		private void SearchBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (!(DataContext is WebPageViewModel vm)) return;

			if(e.Key == Windows.System.VirtualKey.Enter)
			{
				var be = SearchTextBox.GetBindingExpression(TextBox.TextProperty);
				be.UpdateSource();
				vm.SearchCommand?.Execute(null);
			}
		}
	}
}
