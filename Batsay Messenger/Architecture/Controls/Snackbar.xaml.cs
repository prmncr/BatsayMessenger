using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class Snackbar : UserControl
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