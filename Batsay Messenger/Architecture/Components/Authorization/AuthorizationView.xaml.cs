namespace Batsay_Messenger.Architecture.Components.Authorization
{
	public partial class AuthorizationView : IAppScreen
	{
		public AuthorizationView()
		{
			DataContext = new AuthorizationViewModel();
			InitializeComponent();
		}
	}
}