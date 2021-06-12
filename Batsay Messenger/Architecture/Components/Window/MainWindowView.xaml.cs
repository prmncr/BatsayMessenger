using System.Reflection;
using Batsay_Messenger.Data;

namespace Batsay_Messenger.Architecture.Components.Window
{
	public partial class MainWindowView
	{
		private static MainWindowView _instance;
		
		public MainWindowView()
		{
			typeof(Singleton).GetField("_viewInstance", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, this);
			InitializeComponent();
			typeof(Singleton).GetField("_viewModelInstance", BindingFlags.NonPublic | BindingFlags.Static)
				?.SetValue(null, DataContext as MainWindowViewModel);
		}
	}
}