using System;
using System.Collections.ObjectModel;
using Batsay_Messenger.Architecture.Components.Messenger;
using Batsay_Messenger.Architecture.Components.Window;
using Batsay_Messenger.Data;

namespace Batsay_Messenger.Architecture.Components.Authorization
{
	internal class AuthorizationViewModel : BaseViewModel
	{
		private readonly AuthorizationModel _model = new();
		private BaseCommand _authByNewTokenCommand;
		private BaseCommand _authBySavedGroupCommand;

		public ObservableCollection<AuthGroup> Groups => _model.GetGroups();

		public bool IsAuthWithSavedTokensEnabled => Groups.Count > 0;

		public BaseCommand AuthBySavedGroupCommand => _authBySavedGroupCommand ??= new BaseCommand(group =>
		{
			AuthGroup(((AuthGroup) group).Token, false);
			//var isCompleted = await SingleData.ExecuteAsync(_model.AuthorizationAsync, ((AuthGroup) group).Token);
			//if (isCompleted != null) throw new NotImplementedException();
			//SingleData.MainWindowViewModelInstance.CurrentScreen = new MessengerView();
		});

		public BaseCommand AuthByNewTokenCommand => _authByNewTokenCommand ??= new BaseCommand(token =>
		{
			AuthGroup(token as string, true);
			//var isCompleted = await SingleData.ExecuteAsync(_model.AuthorizationValidation, (string) token);
			//if (isCompleted != null) throw new NotImplementedException();
			//SingleData.MainWindowViewModelInstance.CurrentScreen = new MessengerView();
		});

		private async void AuthGroup(string token, bool needValidation)
		{
			var isCompleted = await Singleton.ViewModelInstance.ExecuteAsync(_model.AuthorizeAsyncFormatter,
				new object[] {token, needValidation});
			if (isCompleted)
				Singleton.ViewModelInstance.CurrentScreen = new MessengerView();
		}
	}
}