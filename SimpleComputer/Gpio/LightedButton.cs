using System;
using Windows.Devices.Gpio;
using Windows.Foundation;

namespace SimpleComputer.Gpio
{
	public class LightedButton
	{
		public Led Led { get; set; }
		public GpioPin ButtonPin { get; set; }
		public int ButtonPinNumber { get; private set; }
		private TypedEventHandler<GpioPin, GpioPinValueChangedEventArgs> ButtonAction { get; set; }

		public LightedButton(GpioController controller, int buttonPinNumber, int ledPinNumber, Action buttonAction = null)
		{
			ButtonPinNumber = buttonPinNumber;
			Led = new Led(controller, ledPinNumber, true);
			InitializeButton(controller);

			if (buttonAction != null)
			{
				SetPressedAction(buttonAction);
			}
		}

		private void InitializeButton(GpioController controller)
		{
			ButtonPin = controller.OpenPin(ButtonPinNumber);
			ButtonPin.SetDriveMode(ButtonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
				? GpioPinDriveMode.InputPullUp
				: GpioPinDriveMode.Input);
			ButtonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
		}

		public void SetPressedAction(Action buttonAction)
		{
			if (ButtonAction != null)
			{
				ButtonPin.ValueChanged -= ButtonAction;
			}

			ButtonAction = (pin, args) =>
			{
				if (args.Edge == GpioPinEdge.FallingEdge)
				{
					buttonAction();
				}
			};
			ButtonPin.ValueChanged += ButtonAction;
		}

		public void ToggleLed()
		{
			Led.Toggle();
		}

		public void TurnLedOn()
		{
			Led.TurnOn();
		}

		public void TurnLedOff()
		{
			Led.TurnOff();
		}
	}
}