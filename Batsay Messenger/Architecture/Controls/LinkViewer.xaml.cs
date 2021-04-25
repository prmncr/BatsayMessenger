using System.Windows.Controls;
using Batsay_Messenger.Architecture.Components.Window;

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
