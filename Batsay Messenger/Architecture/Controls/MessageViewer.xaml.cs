using VkNet.Model;

namespace BatsayMessenger.Architecture.Controls
{
	public partial class MessageViewer
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