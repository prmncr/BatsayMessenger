namespace BatsayMessenger.Components;

[ViewType(ViewType.OverlayView)]
public partial class LinkViewer
{
	public LinkViewer(long url)
	{
		InitializeComponent();
		DataContext = new LinkExplorerViewModel(url);
	}
}