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
using Windows.ApplicationModel.Core;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Prism.Windows.AppModel;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleComputer.Gpio;

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
			StartMonitoring();
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
			//appView.TitleBar.BackgroundColor = Windows.UI.Colors.Black; // or {a: 255, r: 0, g: 0, b: 0}
			//appView.TitleBar.InactiveBackgroundColor = Windows.UI.Colors.Black;
			//appView.TitleBar.ButtonBackgroundColor = Windows.UI.Colors.Black;
			//appView.TitleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.Black;
			//appView.TitleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.Black;
			//appView.TitleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Black;

			//ButtonMonitor.StartMonitoring(NavigationService);
			NavigationService.Navigate("Web", null);
			return Task.CompletedTask;	
		}

	    public void StartMonitoring()
	    {
		    // Get the default GPIO controller on the system
		    GpioController gpio = GpioController.GetDefault();
		    if (gpio == null) return; // GPIO not available on this system

		 //   WhiteButton = gpio.OpenPin(WhiteButtonPinNumber);
		 //   WhiteButton.SetDriveMode(WhiteButton.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
			//    ? GpioPinDriveMode.InputPullUp
			//    : GpioPinDriveMode.Input);
		 //   WhiteButton.DebounceTimeout = TimeSpan.FromMilliseconds(50);
		 //   WhiteButton.ValueChanged += (pin, args) =>
		 //   {
			//    if (args.Edge == GpioPinEdge.FallingEdge)
			//    {
			//	    ToggleRedLed();
			//	    CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			//		    (() => { NavigationService.Navigate("Web", null); }));
			//    }
		 //   };


		 //   RedButton = gpio.OpenPin(RedButtonPinNumber);
		 //   RedButton.SetDriveMode(RedButton.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
			//    ? GpioPinDriveMode.InputPullUp
			//    : GpioPinDriveMode.Input);
		 //   RedButton.DebounceTimeout = TimeSpan.FromMilliseconds(50);
		 //   RedButton.ValueChanged += (pin, args) =>
		 //   {
			//    if (args.Edge == GpioPinEdge.FallingEdge)
			//    {
			//	    ToggleRedLed();
			//	    CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			//		    (() => { NavigationService.Navigate("Web", null); }));
			//    }
		 //   };

			//RedLed = gpio.OpenPin(RedLedPinNumber);
			//RedLed.SetDriveMode(GpioPinDriveMode.Output);
		 //   RedLed.Write(GpioPinValue.High);

		 //   GreenLed = gpio.OpenPin(GreenLedPinNumber);
		 //   GreenLed.SetDriveMode(GpioPinDriveMode.Output);
			//GreenLed.Write(GpioPinValue.High);

		    WhiteButtonLed = InitButtonLed(gpio, WhiteButtonLedPinNumber);
		    YellowButtonLed = InitButtonLed(gpio, YellowButtonLedPinNumber);
		    GreenButtonLed = InitButtonLed(gpio, GreenButtonLedPinNumber);
		    RedButtonLed = InitButtonLed(gpio, RedButtonLedPinNumber);

		    WhiteButton = InitButtonSwitch(gpio, WhiteButtonPinNumber, LedColor.Blue);
		    YellowButton = InitButtonSwitch(gpio, YellowButtonPinNumber, LedColor.Blue);
		    GreenButton = InitButtonSwitch(gpio, GreenButtonPinNumber, LedColor.Green);
		    RedButton = InitButtonSwitch(gpio, RedButtonPinNumber, LedColor.Red);

		    BlueLed = InitLed(gpio, BlueLedPinNumber);
		    GreenLed = InitLed(gpio, GreenLedPinNumber);
		    RedLed = InitLed(gpio, RedLedPinNumber);
	    }

	    private GpioPin InitLed(GpioController gpio, int pinNumber)
	    {
		    var pin = gpio.OpenPin(pinNumber);
			pin.SetDriveMode(GpioPinDriveMode.Output);
		    pin.Write(GpioPinValue.Low);
		    return pin;
	    }

	    private GpioPin InitButtonSwitch(GpioController gpio, int pinNumber, LedColor color)
	    {
		    var pin = gpio.OpenPin(pinNumber);
			pin.SetDriveMode(pin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
					? GpioPinDriveMode.InputPullUp
					: GpioPinDriveMode.Input);
			pin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
		    pin.ValueChanged += (p, args) =>
		    {
			    if (args.Edge == GpioPinEdge.FallingEdge)
			    {
					ToggleLed(color);
			    }
		    };

		    return pin;
	    }

	    private GpioPin InitButtonLed(GpioController gpio, int pinNumber)
	    {
		    var pin = gpio.OpenPin(pinNumber);
			pin.SetDriveMode(GpioPinDriveMode.Output);
			pin.Write(GpioPinValue.High);
		    return pin;
	    }

	    private void ToggleLed(LedColor color)
	    {
		    switch (color)
		    {
				case LedColor.Blue:
					BlueLed.Write(BlueLedIsOn ? GpioPinValue.Low : GpioPinValue.High);
					BlueLedIsOn = !BlueLedIsOn;
					break;
				case LedColor.Green:
					GreenLed.Write(GreenLedIsOn ? GpioPinValue.Low : GpioPinValue.High);
					GreenLedIsOn = !GreenLedIsOn;

					GreenButtonLed.Write(GreenButtonLedIsOn ? GpioPinValue.Low : GpioPinValue.High);
					GreenButtonLedIsOn = !GreenButtonLedIsOn;
					break;
				case LedColor.Red:
					RedLed.Write(RedLedIsOn ? GpioPinValue.Low : GpioPinValue.High);
					RedLedIsOn = !RedLedIsOn;

					RedButtonLed.Write(RedButtonLedIsOn ? GpioPinValue.Low : GpioPinValue.High);
					RedButtonLedIsOn = !RedButtonLedIsOn;
					break;
		    }
	    }

		private const int WhiteButtonPinNumber = 18;
		private const int YellowButtonPinNumber = 24;
		private const int GreenButtonPinNumber = 12;
	    private const int RedButtonPinNumber = 20;

	    public static GpioPin WhiteButton { get; set; }
	    public static GpioPin YellowButton { get; set; }
	    public static GpioPin GreenButton { get; set; }
	    public static GpioPin RedButton { get; set; }

	    public static bool WhiteButtonLedIsOn { get; set; } = true;
	    public static bool YellowButtonLedIsOn { get; set; } = true;
		public static bool GreenButtonLedIsOn { get; set; } = true;
		public static bool RedButtonLedIsOn { get; set; } = true;
		public const int WhiteButtonLedPinNumber = 23;
	    public const int YellowButtonLedPinNumber = 25;
	    public const int GreenButtonLedPinNumber = 16;
	    public const int RedButtonLedPinNumber = 21;
	    public GpioPin WhiteButtonLed { get; set; }
	    public GpioPin YellowButtonLed { get; set; }
	    public GpioPin GreenButtonLed { get; set; }
	    public GpioPin RedButtonLed { get; set; }

	    public static bool BlueLedIsOn { get; set; }
	    public static bool GreenLedIsOn { get; set; }
	    public static bool RedLedIsOn { get; set; }
	    private const int BlueLedPinNumber = 4;
	    private const int GreenLedPinNumber = 17;
	    private const int RedLedPinNumber = 22;
	    public static GpioPin BlueLed { get; set; }
	    public static GpioPin GreenLed { get; set; }
	    public static GpioPin RedLed { get; set; }

	}

	public enum LedColor
	{
		Blue,
		Green,
		Red
	}
}
