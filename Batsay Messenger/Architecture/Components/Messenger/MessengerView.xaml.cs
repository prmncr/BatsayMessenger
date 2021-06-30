namespace BatsayMessenger.Architecture.Components.Messenger
{
	public partial class MessengerView : IAppScreen
	{
		public MessengerView()
		{
			DataContext = new MessengerViewModel();
			InitializeComponent();
		}
	}
}