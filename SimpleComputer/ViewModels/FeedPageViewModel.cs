using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ObjectBuilder2;
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
			//RefreshList();
			//StartMonitoring();
			ScanForPosts();
		}

		private async void ScanForPosts()
		{
			var table = App.TableClient.GetTableReference("Post");
			await table.CreateIfNotExistsAsync();
			var query = new TableQuery<PostEntity>();

			var initialPosts = (await table.ExecuteQuerySegmentedAsync(query, null))
				.OrderByDescending(p => p.PostDate)
				.ToList();
			initialPosts.ForEach(p => Posts.Add(p));

			while (true)
			{
				await Task.Delay(10_000);
				var allPosts = (await table.ExecuteQuerySegmentedAsync(query, null))
					.OrderByDescending(p => p.PostDate)
					.ToList();
				var newPostCount = allPosts.Count() - initialPosts.Count();
				if (newPostCount == 0) continue;

				var newPosts = allPosts.Take(newPostCount).ToList();
				for (var i = 0; i < newPostCount; i++)
				{
					Posts.Insert(i, newPosts[i]);
				}

				initialPosts = allPosts;
				CurrentPost = Posts[0];
			}
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

		public void StartMonitoring()
		{
			// Get the default GPIO controller on the system
			GpioController gpio = GpioController.GetDefault();
			if (gpio == null) return; // GPIO not available on this system

			Button = gpio.OpenPin(5);
			Button.SetDriveMode(Button.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
				? GpioPinDriveMode.InputPullUp
				: GpioPinDriveMode.Input);
			Button.DebounceTimeout = TimeSpan.FromMilliseconds(50);
			Button.ValueChanged += (pin, args) =>
			{
				if (args.Edge == GpioPinEdge.FallingEdge)
				{
					CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
						(() => { NavigationService.Navigate("Web", null); }));
					Button.Dispose();
				}
			};
		}

		public GpioPin Button { get; set; }
	}
}
