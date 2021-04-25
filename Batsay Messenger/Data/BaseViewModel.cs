using System.ComponentModel;

namespace Batsay_Messenger.Data
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}