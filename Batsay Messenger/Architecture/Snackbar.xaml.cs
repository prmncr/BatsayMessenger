using System.Windows;
using System.Windows.Media;

namespace BatsayMessenger.Architecture
{
	public partial class Snackbar
	{
		public Snackbar(bool isError)
		{
			BackgroundBrush = isError
				? new SolidColorBrush(Colors.Red)
				: (SolidColorBrush) Application.Current.TryFindResource("MaterialDesignToolTipBackground");
			InitializeComponent();
		}

		public SolidColorBrush BackgroundBrush { get; }
	}
}