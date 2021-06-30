using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace BatsayMessenger.Architecture.Controls.SettingsViewer
{
	public partial class SettingsControl : IOverlayControl
	{
		public SettingsControl()
		{
			InitializeComponent();
			DataContext = new SettingsControlViewModel();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void TopMostCheckBox_Click(object sender, RoutedEventArgs e)
		{
		}

		private void WindowHeaderCheckBox_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}