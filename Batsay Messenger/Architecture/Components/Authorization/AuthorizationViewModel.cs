using System;
using System.Collections.ObjectModel;
using Batsay_Messenger.Architecture.Components.Messenger;
using Batsay_Messenger.Data;

namespace Batsay_Messenger.Architecture.Components.Authorization
{
	internal class AuthorizationViewModel : BaseViewModel, IAppScreen
	{
		private readonly AuthorizationModel _model = new();
		private BaseCommand _authBySavedGroupCommand;
		private BaseCommand _authByNewTokenCommand;

		public ObservableCollection<AuthGroup> Groups => _model.GetGroups();

		public bool IsAuthWithSavedTokensEnabled => Groups.Count > 0;

		public BaseCommand AuthBySavedGroupCommand => _authBySavedGroupCommand ??= new BaseCommand(async group =>
		{
			var isCompleted = await Singleton.ExecuteAsync(_model.AuthorizationAsync, ((AuthGroup) group).Token);
			if (isCompleted != null) throw new NotImplementedException();
			Singleton.MainViewInstance.CurrentScreen = new MessengerView();
		});

		public BaseCommand AuthByNewTokenCommand => _authByNewTokenCommand ??= new BaseCommand(async token =>
		{
			var isCompleted = await Singleton.ExecuteAsync(_model.AuthorizationValidation, (string) token);
			if (isCompleted != null) throw new NotImplementedException();
			Singleton.MainViewInstance.CurrentScreen = new MessengerView();
		});
	}
}