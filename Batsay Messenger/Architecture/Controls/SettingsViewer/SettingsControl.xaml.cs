using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Batsay_Messenger.Architecture.Controls.SettingsViewer
{
	public partial class SettingsControl : UserControl, IOverlayControl
	{
		public SettingsControl()
		{
			InitializeComponent();
			DataContext = new SettingsControlViewModel();
		}

		private void Grid_MouseLeftButtonUp(object sender, RoutedEventArgs e)
		{
		}

		private void SettingsControl_Loaded(object sender, RoutedEventArgs e)
		{
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