namespace BatsayMessenger.Architecture.Components.Auth
{
	public partial class AuthView : IAppScreen
	{
		public AuthView()
		{
			DataContext = new AuthViewModel();
			InitializeComponent();
		}
	}
}