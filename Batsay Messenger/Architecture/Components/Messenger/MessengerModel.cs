#nullable enable
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BatsayMessenger.VkClasses;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model.RequestParams;

namespace BatsayMessenger.Architecture.Components.Messenger
{
	internal class MessengerModel
	{
		private readonly Random _random = new();
		private readonly Group _sender = new(Data.GroupId, Data.GroupName, Data.GroupPhoto50);
		private readonly CancellationToken _token;
		private readonly CancellationTokenSource _tokenSource = new();

		public MessengerModel()
		{
			_token = _tokenSource.Token;
			LongPollInit();
		}

		public ObservableCollection<Conversation> Conversations { get; private set; } = new();

		~MessengerModel()
		{
			_tokenSource.Cancel();
		}

		public ObservableCollection<Message>? GetMessages(long? selectedConversationId)
		{
			return selectedConversationId == null
				? null
				: Conversations.First(x => x.Id == selectedConversationId).Messages;
		}

		public async Task SearchConversation(object obj)
		{
			if (!long.TryParse(obj.ToString(), out var id)) return;
			if (id < 2_000_000_000) return;
			try
			{
				var response = await Data.Api.Messages.GetConversationsByIdAsync(new[] {id});
				if (response.Items.Any())
				{
					var conversation = response.Items.First();
					if (Conversations.All(x => x.Id != id))
						Conversations.Add(new Conversation(conversation.Peer.Id, conversation));
				}
				else
				{
					if (Conversations.All(x => x.Id != id))
						Conversations.Add(new Conversation(id));
				}
			}
			catch (VkApiMethodInvokeException e)
			{
				Console.WriteLine(e);
			}
		}

		private async Task<Conversation> LoadConversationInfo(long id)
		{
			var response = await Data.Api.Messages.GetConversationsByIdAsync(new[] {id});
			var conversationData = response.Items.FirstOrDefault();
			Conversation conversation;
			if (conversationData != null)
			{
				var membersResponse = await Data.Api.Messages.GetConversationMembersAsync(id);
				var members = membersResponse.Profiles.ToDictionary(user => user.Id,
					user => new User(user.Id, user.FirstName, user.LastName, user.Photo50) as Member);

				foreach (var group in membersResponse.Groups)
					members.Add(-group.Id, new Group(group.Id, group.Name, group.Photo50));

				conversation = new Conversation(id, conversationData, members);
			}
			else
			{
				conversation = new Conversation(id);
			}

			return conversation;
		}

		public void DeleteConversation(long id)
		{
			Conversations = new ObservableCollection<Conversation>(Conversations.Where(x => x.Id != id));
		}

		public async void SendMessage(string text, long id)
		{
			await Data.Api.Messages.SendAsync(new MessagesSendParams
			{
				PeerId = id,
				Message = text,
				RandomId = _random.Next()
			});
			Conversations.First(conversation => conversation.Id == id).Messages
				.Add(new Message(_sender, DateTime.Now, text));
		}

		#region LONGPOLL

		private async void LongPollInit()
		{
			var progress = new Progress<VkNet.Model.Message>(ReceiveMessage);
			await Task.Factory.StartNew(() => LongPoll(progress), _token);
		}

		private void LongPoll(IProgress<VkNet.Model.Message> progress)
		{
			var response = Data.Api.Groups.GetLongPollServer((ulong) Data.GroupId);
			var ts = response.Ts;
			while (!_token.IsCancellationRequested)
			{
				var poll = Data.Api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
					{Server = response.Server, Ts = ts, Key = response.Key, Wait = 25});
				if (poll?.Updates == null) continue;
				foreach (var update in poll.Updates)
					if (update.Type == GroupUpdateType.MessageNew || update.Type == GroupUpdateType.MessageReply)
						progress.Report(update.MessageNew.Message);
				ts = poll.Ts;
			}
		}

		private async void ReceiveMessage(VkNet.Model.Message message)
		{
			var conversationId = message.PeerId!.Value;
			var senderId = message.FromId!.Value;

			var current = Conversations.FirstOrDefault(conversation => conversation.Id == conversationId);
			if (current == null)
			{
				current = await LoadConversationInfo(conversationId);
				Conversations.Add(current);
			}

			var isVisible = current.Members.ContainsKey(senderId);
			Member sender;
			if (senderId > 0)
				sender = new User(senderId,
					isVisible ? ((User) current.Members[senderId]).FirstName : $"@id{senderId}",
					isVisible ? ((User) current.Members[senderId]).LastName : string.Empty,
					isVisible ? current.Members?[senderId]?.PhotoUri : null);
			else
				sender = new Group(senderId,
					isVisible ? current.Members?[senderId].Name : $"@group{-senderId}",
					isVisible ? current.Members?[senderId]?.PhotoUri : null);

			current.Messages.Add(new Message(sender, message.Date, message.Text, message.Attachments));
		}

		#endregion
	}
}