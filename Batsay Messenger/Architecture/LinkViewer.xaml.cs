namespace BatsayMessenger.Architecture
{
	[ViewType(ViewType.OverlayView)]
	public partial class LinkViewer
	{
		public LinkViewer(long url)
		{
			InitializeComponent();
			DataContext = new LinkExplorerViewModel(url);
		}
	}
}