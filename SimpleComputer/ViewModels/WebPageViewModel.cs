using Prism.Commands;
using Prism.Windows.Navigation;
using System;
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
	    public ICommand CreatePostCommand { get; set; }
	    public ICommand CalendarCommand { get; set; }
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
			CreatePostCommand = new DelegateCommand(ShowCreatePost);
			CalendarCommand = new DelegateCommand(ShowCalendar);
			GoBackCommand = new DelegateCommand(GoBack);
			NavigationService = navigationService;
		}

	    private void ShowCalendar()
	    {
		    NavigationService.Navigate("Calendar", null);
	    }

	    private void ShowCreatePost()
	    {
		    NavigationService.Navigate("AddPost", null);
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
					uri = GenerateSearhUri(@"https://kansascity.craigslist.org/search/sss?query={0}&sort=rel", SearchText);
					break;
				case "LUCKY":
					uri = GenerateSearhUri(@"https://google.com/search?btnI=3564&q={0}", SearchText);
					break;
				default:
					uri = GenerateSearhUri(@"https://google.com/search?q={0}", SearchText);
					break;
			}
			GotoPage(uri);
		}

		private Uri GenerateSearhUri(string searchPath, string searchTerms)
		{
			searchTerms = searchTerms ?? String.Empty;
			var uriEncodedTerms = Uri.EscapeUriString(searchTerms);
			return new Uri(string.Format(searchPath, uriEncodedTerms));
		}
	}
}
