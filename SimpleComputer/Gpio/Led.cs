using Windows.Devices.Gpio;

namespace SimpleComputer.Gpio
{
	public class Led
	{
		public int LedPinNumber { get; private set; }
		public GpioPin LedPin { get; set; }
		public bool IsOn { get; private set; }

		public Led(GpioController controller, int ledPinNumber, bool isOn = false)
		{
			LedPinNumber = ledPinNumber;
			IsOn = isOn;
			InitializeLed(controller);
		}

		private void InitializeLed(GpioController controller)
		{
			LedPin = controller.OpenPin(LedPinNumber);
			LedPin.SetDriveMode(GpioPinDriveMode.Output);
			LedPin.Write(IsOn ? GpioPinValue.High : GpioPinValue.Low);
		}

		public void Toggle()
		{
			if (IsOn)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
		}

		public void TurnOn()
		{
			LedPin.Write(GpioPinValue.High);
			IsOn = true;
		}

		public void TurnOff()
		{
			LedPin.Write(GpioPinValue.Low);
			IsOn = false;
		}
	}
}