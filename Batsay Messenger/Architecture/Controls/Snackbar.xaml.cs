using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class Snackbar : UserControl
	{
		public SolidColorBrush BackgroundBrush => _backgroundBrush;

		private SolidColorBrush _backgroundBrush;

		public Snackbar(bool isError)
		{
			if (isError) _backgroundBrush = new SolidColorBrush(Colors.Red);
			_backgroundBrush = (SolidColorBrush) Application.Current.TryFindResource("MaterialDesignToolTipBackground");
			InitializeComponent();
		}
	}
}
