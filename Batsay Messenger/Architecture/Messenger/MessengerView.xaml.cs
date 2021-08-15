namespace BatsayMessenger.Architecture.Messenger
{
	[ViewType(ViewType.MainView)]
	public partial class MessengerView
	{
		public MessengerView()
		{
			DataContext = new MessengerViewModel();
			InitializeComponent();
		}
	}
}