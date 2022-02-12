using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace BatsayMessenger.Components.SettingsViewer;

[ViewType(ViewType.OverlayView)]
public partial class SettingsControl
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