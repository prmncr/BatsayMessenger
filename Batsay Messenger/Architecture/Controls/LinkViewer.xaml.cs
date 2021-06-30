namespace BatsayMessenger.Architecture.Controls
{
	public partial class LinkViewer : IOverlayControl
	{
		public LinkViewer(long url)
		{
			InitializeComponent();
			DataContext = new LinkExplorerViewModel(url);
		}
	}
}