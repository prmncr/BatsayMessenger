using System.Windows.Controls;
using Batsay_Messenger.Data;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class GroupViewer : UserControl, IOverlayControl
	{
		public GroupViewer()
		{
			InitializeComponent();
			LabelName.Text = Singleton.GroupName;
			BorderPublicPhoto.Background = UriToBrushConverter.Convert(Singleton.GroupPhoto50);
		}
	}
}