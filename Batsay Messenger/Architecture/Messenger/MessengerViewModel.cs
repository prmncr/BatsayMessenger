﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BatsayMessenger.Architecture.SettingsViewer;
using BatsayMessenger.Architecture.Window;
using BatsayMessenger.Properties;
using BatsayMessenger.VkClasses;
using Essentials.MVVM;

namespace BatsayMessenger.Architecture.Messenger
{
	public class MessengerViewModel : BaseViewModel
	{
		private readonly MessengerModel _model;
		private Conversation _currentConversation;
		private string _currentConversationId;
		private string _currentConversationName;
		private BaseCommand _deleteConversationCommand;
		private BaseCommand _groupInfoCommand;
		private string _messageText;
		private bool _messengerVisibility;
		private BaseCommand _openLinkOverlayCommand;
		private BaseCommand _openSettingsCommand;
		private BaseCommand _searchConversationCommand;
		private BaseCommand _sendMessageCommand;

		public MessengerViewModel()
		{
			_model = new MessengerModel();
		}

		public static string GroupName => Data.GroupName;
		public static Uri GroupPhoto => Data.GroupPhoto50;

		public ObservableCollection<Conversation> Conversations => this._model.Conversations;

		public ObservableCollection<Message> Messages => _model.GetMessages(CurrentConversation?.Id);

		public string CurrentConversationName
		{
			get => _currentConversationName;
			set
			{
				_currentConversationName = value;
				OnPropertyChanged(nameof(CurrentConversationName));
			}
		}

		public string CurrentConversationId
		{
			get => _currentConversationId;
			set
			{
				_currentConversationId = value;
				OnPropertyChanged(nameof(CurrentConversationId));
			}
		}

		public Conversation CurrentConversation
		{
			get => _currentConversation;
			set
			{
				CurrentConversationName = value.Title;
				CurrentConversationId = value.Id.ToString();
				_currentConversation = value;
				MessengerVisibility = true;
				OnPropertyChanged(nameof(Messages));
				OnPropertyChanged(nameof(CurrentConversation));
			}
		}

		public string MessageText
		{
			get => _messageText;
			set
			{
				_messageText = value;
				OnPropertyChanged(nameof(MessageText));
			}
		}

		public bool MessengerVisibility
		{
			get => _messengerVisibility;
			set
			{
				_messengerVisibility = value;
				OnPropertyChanged(nameof(MessengerVisibility));
			}
		}

		public BaseCommand OpenLinkOverlayCommand => _openLinkOverlayCommand ??= new BaseCommand(obj =>
		{
			var url = (long) obj;
			if (Settings.Default.OpenLinkExplorer)
				WindowViewModel.Instance.OverlayContent = new LinkViewer(url);
			else
				Process.Start($"https://vk.com/{(url > 0 ? "id" + url : "group" + -url)}");
		});

		public BaseCommand OpenSettingsCommand => _openSettingsCommand ??=
			new BaseCommand(_ => WindowViewModel.Instance.OverlayContent = new SettingsControl());

		public BaseCommand DeleteConversationCommand => _deleteConversationCommand ??=
			new BaseCommand(obj => _model.DeleteConversation((long) obj));

		public BaseCommand SearchConversationCommand => _searchConversationCommand ??= new BaseCommand(async obj =>
			await WindowViewModel.Instance.ExecuteAsync(() => _model.SearchConversation(obj)));

		public BaseCommand SendMessageCommand => _sendMessageCommand ??=
			new BaseCommand(_ =>
				{
					_model.SendMessage(MessageText, CurrentConversation.Id);
					MessageText = string.Empty;
				},
				_ => MessageText?.Length > 0);

		public BaseCommand GroupInfoCommand => _groupInfoCommand ??= new BaseCommand(_ =>
			WindowViewModel.Instance.OverlayContent = new GroupViewer());
	}
}