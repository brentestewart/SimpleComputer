using System.ComponentModel;

namespace SimpleComputer.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void SetupGpio() {}
	}
}
