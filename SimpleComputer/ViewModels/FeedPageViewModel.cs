using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleComputer.Models;

namespace SimpleComputer.ViewModels
{
	public class FeedPageViewModel : BaseViewModel
	{
		public INavigationService NavigationService { get; }
		public ICommand WebCommand { get; set; }
		public ICommand RefreshCommand { get; set; }
		public ICommand PreviousCommand { get; set; }
		public ICommand NextCommand { get; set; }
		public ObservableCollection<PostEntity> Posts { get; set; } = new ObservableCollection<PostEntity>();
		public PostEntity CurrentPost { get; set; }

		public FeedPageViewModel(INavigationService navigationService)
		{
			NavigationService = navigationService;
			WebCommand = new DelegateCommand(ShowWeb);
			RefreshCommand = new DelegateCommand(RefreshList);
			PreviousCommand = new DelegateCommand(PreviousPost);
			NextCommand = new DelegateCommand(NextPost);
			RefreshList();
		}

		private void NextPost()
		{
			var index = Posts.IndexOf(CurrentPost);
			if (index < Posts.Count-1)
			{
				CurrentPost = Posts[++index];
			}
		}

		private void PreviousPost()
		{
			var index = Posts.IndexOf(CurrentPost);
			if (index > 0)
			{
				CurrentPost = Posts[--index];
			}

		}

		private async void RefreshList()
		{
			try
			{
				Posts.Clear();
				var table = App.TableClient.GetTableReference("Post");
				await table.CreateIfNotExistsAsync();
				var query = new TableQuery<PostEntity>();
				var allPosts = await table.ExecuteQuerySegmentedAsync(query, null);
				foreach (var post in allPosts.OrderByDescending(p => p.PostDate))
				{
					Posts.Add(post);
				}

				CurrentPost = Posts.First();
			}
			catch (Exception ex)
			{				
			}
		}

		private void ShowWeb()
		{
			NavigationService.Navigate("Web", null);
		}
	}
}
