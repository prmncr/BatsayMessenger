using System.Windows.Controls;
using VkNet.Model;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class MessageViewer : UserControl
	{
		public MessageViewer()
		{
			InitializeComponent();
		}

		public Message Message { get; set; }

		public string SenderName
		{
			get => TextBlockSenderName.Text;
			set => TextBlockSenderName.Text = value;
		}
	}
}