using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimpleComputer.ViewModels
{
	public class FeedPageViewModel : BaseViewModel
	{
		public INavigationService NavigationService { get; }
		public ICommand WebCommand { get; set; }
		public ICommand UploadCommand { get; set; }
		public ICommand RefreshCommand { get; set; }
		public ObservableCollection<CloudBlockBlob> Picts { get; set; } = new ObservableCollection<CloudBlockBlob>();

		public FeedPageViewModel(INavigationService navigationService)
		{
			NavigationService = navigationService;
			WebCommand = new DelegateCommand(ShowWeb);
			UploadCommand = new DelegateCommand(Upload);
			RefreshCommand = new DelegateCommand(RefreshList);
		}

		private async void Upload()
		{
			FileOpenPicker openPicker = new FileOpenPicker();
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".png");
			openPicker.FileTypeFilter.Add(".jpeg");
			var file = await openPicker.PickSingleFileAsync();

			if (file != null)
			{
				using (var fileStream = await file.OpenSequentialReadAsync())
				{
					await App.AzureContainer.CreateIfNotExistsAsync();
					var blob = App.AzureContainer.GetBlockBlobReference(file.Name);
					await blob.DeleteIfExistsAsync();
					await blob.UploadFromStreamAsync(fileStream.AsStreamForRead());
				}
			}
		}

		private async void CreatePost()
		{
			await App.AzureTable.CreateIfNotExistsAsync();
			App.AzureTable.get

		}

		//private async void Download()
		//{
		//	var blob = App.AzureContainer.GetBlockBlobReference(item.Name);
		//	StorageFile file;
		//	try
		//	{
		//		Windows.Storage.StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
		//		file = await temporaryFolder.CreateFileAsync(item.Name,
		//		   CreationCollisionOption.ReplaceExisting);

		//		ProcessBegin();

		//		var downloadTask = blob.DownloadToFileAsync(file);

		//		await downloadTask;

		//		downloadTask.Completed = (IAsyncAction asyncInfo, AsyncStatus asyncStatus) =>
		//		{
		//			ProcessEnd();
		//		};

		//		// Make sure it's an image file. 
		//		imgBlobItem.Source = new BitmapImage(new Uri(file.Path));
		//	}
		//	catch (Exception ex)
		//	{
		//		statusText.Text = (ex.Message + "\n");
		//	}
		//}

		private async void RefreshList()
		{
			try
			{
				Picts.Clear();
				await App.AzureContainer.CreateIfNotExistsAsync();
				BlobContinuationToken token = null;
				var blobsSegmented = await App.AzureContainer.ListBlobsSegmentedAsync(token);
				var picts = blobsSegmented.Results;
				foreach (var pict in picts)
				{
					if (pict is CloudBlockBlob blob)
					{						
						Picts.Add(blob);
					}
				}
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

	public class PostEntity : TableEntity
	{
		public string Text { get; set; }
		public PostEntity()
		{
			PartitionKey = DateTime.Now.Year.ToString();
			RowKey = DateTime.Now.ToString("MMddhhmm");
		}
	}
}
