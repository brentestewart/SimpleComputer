using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Prism.Windows.Navigation;

namespace SimpleComputer.Gpio
{
	public static class ButtonMonitor
	{
		public static void StartMonitoring(INavigationService navigationService)
		{
			// Get the default GPIO controller on the system
			GpioController gpio = GpioController.GetDefault();
			if (gpio == null) return; // GPIO not available on this system

			var button = gpio.OpenPin(13);
			button.SetDriveMode(GpioPinDriveMode.Input);
			button.DebounceTimeout = TimeSpan.FromMilliseconds(100);
			button.ValueChanged += (pin, args) =>
			{
				if (args.Edge == GpioPinEdge.FallingEdge)
				{
					navigationService.Navigate("Web", null);
				}
			};
		}
	}
}
