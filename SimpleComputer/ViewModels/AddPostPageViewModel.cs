using System.Windows.Input;
using Windows.Storage.Pickers;
using Prism.Commands;
using SimpleComputer.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using Windows.Management.Deployment;

namespace SimpleComputer.ViewModels
{
	public class AddPostPageViewModel : BaseViewModel
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string ImagePath { get; set; }
		public ICommand UploadImageCommand { get; set; }
		public ICommand CreatePostCommand { get; set; }

		public AddPostPageViewModel()
		{
			UploadImageCommand = new DelegateCommand(UploadImage);
			CreatePostCommand = new DelegateCommand(CreatePost);
		}

		private async void UploadImage()
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
					ImagePath = blob.Uri.ToString();
				}
			}
		}

		private async void CreatePost()
		{
			var post = new PostEntity(Title, Content)
			{
				ImagePath = ImagePath
			};

			var table = App.TableClient.GetTableReference("Post");
			await table.CreateIfNotExistsAsync();
			var insertOperation = TableOperation.Insert(post);
			await table.ExecuteAsync(insertOperation);
		}
	}
}