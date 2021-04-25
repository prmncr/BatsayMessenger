using System.ComponentModel;
using Batsay_Messenger.Architecture.Components.Window;

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
