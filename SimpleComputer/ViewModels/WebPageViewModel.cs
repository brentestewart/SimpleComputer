using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleComputer.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WebPageViewModel : BaseViewModel
    {
		public bool ShowWeb { get; set; }
		public ICommand SearchCommand { get; set; }
		public string SearchText { get; set; }
		public ICommand ShowSearchCommand { get; set; }
		public ICommand Item1Command { get; set; }
		public Action<Uri> SearchAction { get; set; }
		public Action InitializeSearch { get; set; }

		public WebPageViewModel()
		{
			SearchCommand = new DelegateCommand(Search);
			ShowSearchCommand = new DelegateCommand(ShowSearch);
			Item1Command = new DelegateCommand(ShowCraigsList);
		}

		private void ShowCraigsList()
		{
			var uri = new Uri(@"https://kansascity.craigslist.org/");
			GotoPage(uri);
		}

		private void GotoPage(Uri uri)
		{
			SearchAction?.Invoke(uri);
			ShowWeb = true;
		}

		private void ShowSearch()
		{
			InitializeSearch?.Invoke();
			SearchText = string.Empty;
			ShowWeb = false;
		}

		private void Search()
		{
			var uri = GenerateSearhUri(SearchText);
			GotoPage(uri);
		}

		private Uri GenerateSearhUri(string searchTerms)
		{
			var uriEncodedTerms = Uri.EscapeUriString(searchTerms);
			return new Uri($@"https://google.com/search?q={uriEncodedTerms}");
		}
	}
}
