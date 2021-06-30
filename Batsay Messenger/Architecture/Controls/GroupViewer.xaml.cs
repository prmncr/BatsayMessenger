using BatsayMessenger.Utils;

namespace BatsayMessenger.Architecture.Controls
{
	public partial class GroupViewer : IOverlayControl
	{
		public GroupViewer()
		{
			InitializeComponent();
			LabelName.Text = VkClasses.Data.GroupName;
			BorderPublicPhoto.Background = UriToBrushConverter.Convert(VkClasses.Data.GroupPhoto50);
		}
	}
}