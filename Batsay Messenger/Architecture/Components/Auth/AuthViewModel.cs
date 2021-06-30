using System.Collections.ObjectModel;
using BatsayMessenger.Architecture.Components.Messenger;
using BatsayMessenger.Architecture.Components.Window;
using BatsayMessenger.VkClasses;
using MVVMBase;

namespace BatsayMessenger.Architecture.Components.Auth
{
	internal class AuthViewModel : BaseViewModel
	{
		private readonly AuthModel _model = new();
		private BaseCommand _authByNewTokenCommand;
		private BaseCommand _authBySavedGroupCommand;

		public ObservableCollection<AuthGroup> Groups => _model.GetGroups();

		public bool IsAuthWithSavedTokensEnabled => Groups.Count > 0;

		public BaseCommand AuthBySavedGroupCommand => _authBySavedGroupCommand ??=
			new BaseCommand(group => AuthGroup(((AuthGroup) group).Token, false));

		public BaseCommand AuthByNewTokenCommand => _authByNewTokenCommand ??=
			new BaseCommand(token => AuthGroup(token as string, true));

		private async void AuthGroup(string token, bool needValidation)
		{
			var isCompleted =
				await WindowViewModel.Instance.ExecuteAsync(() => _model.AuthorizeAsync(token, needValidation));
			if (isCompleted)
				WindowViewModel.Instance.CurrentScreen = new MessengerView();
		}
	}
}