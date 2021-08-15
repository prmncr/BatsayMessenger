using BatsayMessenger.Utils;
using BatsayMessenger.VkClasses;

namespace BatsayMessenger.Architecture
{
	[ViewType(ViewType.OverlayView)]
	public partial class GroupViewer
	{
		public GroupViewer()
		{
			InitializeComponent();
			LabelName.Text = Data.GroupName;
			BorderPublicPhoto.Background = UriToBrushConverter.Convert(Data.GroupPhoto50);
		}
	}
}