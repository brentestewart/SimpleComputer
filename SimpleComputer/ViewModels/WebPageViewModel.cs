using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleComputer.ViewModels
{
	public class WebPageViewModel : BaseViewModel
    {
		public bool ShowWeb { get; set; }
		public ICommand SearchCommand { get; set; }
		public string SearchText { get; set; }
		public ICommand ShowSearchCommand { get; set; }
		public ICommand Item1Command { get; set; }
		public ICommand FeedCommand { get; set; }
		public ICommand GoBackCommand { get; set; }
		public Action<Uri> SearchAction { get; set; }
		public Action GoBackAction { get; set; }
		public Action InitializeSearch { get; set; }
		public INavigationService NavigationService { get; }

		public WebPageViewModel(INavigationService navigationService)
		{
			SearchCommand = new DelegateCommand<string>(Search);
			ShowSearchCommand = new DelegateCommand(ShowSearch);
			Item1Command = new DelegateCommand(ShowCraigsList);
			FeedCommand = new DelegateCommand(ShowFeed);
			GoBackCommand = new DelegateCommand(GoBack);
			NavigationService = navigationService;
		}

		private void GoBack()
		{
			GoBackAction?.Invoke();
		}

		private void ShowFeed()
		{
			NavigationService.Navigate("Feed", null);
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

		private void Search(string searchType)
		{
			Uri uri;
			switch (searchType)
			{
				case "CRAIGSLIST":
					uri = GenerateSearhUri(SearchText, @"https://kansascity.craigslist.org/search/sss?query={0}&sort=rel");
					break;
				case "LUCKY":
					uri = GenerateSearhUri(SearchText, @"https://google.com/search?btnI=3564&q={0}");
					break;
				default:
					uri = GenerateSearhUri(SearchText, @"https://google.com/search?q={0}");
					break;
			}
			GotoPage(uri);
		}

		private Uri GenerateSearhUri(string searchTerms, string searchPath)
		{
			var uriEncodedTerms = Uri.EscapeUriString(searchTerms);
			return new Uri(string.Format(searchPath, searchTerms));
		}
	}
}
