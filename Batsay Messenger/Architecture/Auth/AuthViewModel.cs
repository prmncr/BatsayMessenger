using System;
using System.Collections.ObjectModel;
using BatsayMessenger.Architecture.Messenger;
using BatsayMessenger.Architecture.Window;
using BatsayMessenger.VkClasses;
using Essentials.MVVM;
using VkNet.Exception;

namespace BatsayMessenger.Architecture.Auth
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
			new BaseCommand(obj =>
			{
				var token = obj.ToString();
				if (string.IsNullOrWhiteSpace(token)) return;
				AuthGroup(token, true);
			});

		private async void AuthGroup(string token, bool needValidation)
		{
			if (!await WindowViewModel.Instance.ExecuteAsync(() => _model.AuthorizeAsync(token, needValidation)))
				throw new VkAuthorizationException();
			WindowViewModel.Instance.CurrentScreen = new MessengerView();
		}
	}
}