using SimpleComputer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Prism.Unity;
using Prism.Unity.Windows;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Prism.Windows.AppModel;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimpleComputer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
		private static readonly StorageCredentials cred = new StorageCredentials("simplecomputer", "XWz6oYC8qIhG+bEln/bjBEVjI1HzmsianUEmlexgRccYatqn2jEcU/RmkU4POir8LxVqlBxgydn3uHb1GQFBCA==");
		private static readonly string tableConnectionString = @"DefaultEndpointsProtocol=https;AccountName=simplecomputer;AccountKey=XWz6oYC8qIhG+bEln/bjBEVjI1HzmsianUEmlexgRccYatqn2jEcU/RmkU4POir8LxVqlBxgydn3uHb1GQFBCA==;TableEndpoint=https://simplecomputer.table.core.windows.net/;";
		public static readonly CloudBlobContainer AzureContainer = new CloudBlobContainer(new Uri("http://simplecomputer.blob.core.windows.net/imagescontainer/"), cred);
		public static CloudTableClient TableClient { get; set; }
	    //public static readonly CloudTable AzureTable = Azure.CreateTableService("simplecomputer"), cred);
		//public static readonly CloudTable AzureTable = Azure.CreateTableService CloudTable(new Uri("https://simplecomputer.table.core.windows.net/Post"), cred);
		
		/// <summary>
		/// /// Initializes the singleton application object.  This is the first line of authored code
		/// /// executed, and as such is the logical equivalent of main() or WinMain().
																																														/// </summary>
		public App()
		{
			ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
			this.InitializeComponent();
			var account = CloudStorageAccount.Parse(tableConnectionString);
			TableClient = account.CreateCloudTableClient();
			//this.Suspending += OnSuspending;
		}

		protected override UIElement CreateShell(Frame rootFrame)
		{
			var shell = Container.Resolve<Shell>();
			shell.SetContentFrame(rootFrame);
			return shell;
		}


		protected override Task OnInitializeAsync(IActivatedEventArgs args)
		{
			Container.RegisterInstance(NavigationService);
			return base.OnInitializeAsync(args);
		}

		protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
		{
			var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
			appView.TitleBar.BackgroundColor = Windows.UI.Colors.Black; // or {a: 255, r: 0, g: 0, b: 0}
			appView.TitleBar.InactiveBackgroundColor = Windows.UI.Colors.Black;
			appView.TitleBar.ButtonBackgroundColor = Windows.UI.Colors.Black;
			appView.TitleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.Black;
			appView.TitleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.Black;
			appView.TitleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Black;

			NavigationService.Navigate("Web", null);
			return Task.CompletedTask;	
		}
	}
}
