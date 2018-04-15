using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Prism.Windows.Navigation;

namespace SimpleComputer.Gpio
{
	public static class GpioManager
	{
		#region Properties and member variables
		public static GpioController Controller { get; set; }

		private const int WhiteButtonPinNumber = 18;
		private const int WhiteButtonLedPinNumber = 23;
		public static LightedButton WhiteButton { get; set; }

		private const int YellowButtonPinNumber = 24;
		private const int YellowButtonLedPinNumber = 25;
		public static LightedButton YellowButton { get; set; }

		private const int GreenButtonPinNumber = 12;
		private const int GreenButtonLedPinNumber = 16;
		public static LightedButton GreenButton { get; set; }

		private const int RedButtonPinNumber = 20;
		private const int RedButtonLedPinNumber = 21;
		public static LightedButton RedButton { get; set; }

		private const int BlueLedPinNumber = 4;
		public static Led BlueLed { get; set; }

		private const int GreenLedPinNumber = 17;
		public static Led GreenLed { get; set; }

		private const int RedLedPinNumber = 22;
		public static Led RedLed { get; set; }
		#endregion

		static GpioManager()
		{
			Controller = GpioController.GetDefault();
			if(Controller == null) throw new Exception("Device does not support GPIO");

			InitializeButtons();
			InitializeLeds();
		}

		private static void InitializeButtons()
		{
			WhiteButton = new LightedButton(Controller, WhiteButtonPinNumber, WhiteButtonLedPinNumber);
			YellowButton = new LightedButton(Controller, YellowButtonPinNumber, YellowButtonLedPinNumber);
			GreenButton = new LightedButton(Controller, GreenButtonPinNumber, GreenButtonLedPinNumber);
			RedButton = new LightedButton(Controller, RedButtonPinNumber, RedButtonLedPinNumber);
		}

		private static void InitializeLeds()
		{
			BlueLed = new Led(Controller, BlueLedPinNumber);
			GreenLed = new Led(Controller, GreenLedPinNumber);
			RedLed = new Led(Controller, RedLedPinNumber);
		}
	}
}
