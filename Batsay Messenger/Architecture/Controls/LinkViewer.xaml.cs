using System.Windows.Controls;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class LinkViewer : UserControl, IOverlayControl
	{
		public LinkViewer(long url)
		{
			InitializeComponent();
			DataContext = new LinkExplorerViewModel(url);
		}
	}
}