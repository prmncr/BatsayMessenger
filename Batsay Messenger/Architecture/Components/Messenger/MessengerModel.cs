using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Batsay_Messenger.Data;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model.RequestParams;

namespace Batsay_Messenger.Architecture.Components.Messenger
{
	internal class MessengerModel
	{
		private readonly Random _random = new();

		public ObservableCollection<Conversation> Conversations { get; set; } = new();

		private readonly Group _sender = new(Singleton.GroupId, Singleton.GroupName, Singleton.GroupPhoto50);

		public MessengerModel()
		{
			LongPollSetup();
		}

		public ObservableCollection<Message> GetMessages(long? selectedConversationId)
		{
			return selectedConversationId == null
				? null
				: Conversations.First(x => x.Id == selectedConversationId).Messages;
		}

		public async void SearchConversation(object obj)
		{
			if (!long.TryParse(obj.ToString(), out var id)) return;
			if (id < 2_000_000_000) return;
			try
			{
				var response = await Singleton.Api.Messages.GetConversationsByIdAsync(new[] {id});
				if (response.Items.Any())
				{
					var conversation = response.Items.First();
					if (Conversations.Count(x => x.Id == id) == 0)
						Conversations.Add(new Conversation(conversation.Peer.Id, conversation));
				}
				else
				{
					if (Conversations.Count(x => x.Id == id) == 0)
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
			var response = await Singleton.Api.Messages.GetConversationsByIdAsync(new[] {id});
			var conversationData = response.Items.FirstOrDefault();
			Conversation conversation;
			if (conversationData != null)
			{
				var membersResponse = await Singleton.Api.Messages.GetConversationMembersAsync(id);
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

			 Conversations.Add(conversation);
			return conversation;
		}

		public void DeleteConversation(long id)
		{
			Conversations.Remove(x => x.Id == id);
		}

		public async void SendMessage(string text, long id)
		{
			await Singleton.Api.Messages.SendAsync(new MessagesSendParams
			{
				PeerId = id,
				Message = text,
				RandomId = _random.Next()
			});
			Conversations.First(conversation => conversation.Id == id).Messages.Add(new Message
			{
				Text = text,
				Date = DateTime.Now,
				Sender = _sender
			});
		}

		[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
		private void ReceiveMessage(VkNet.Model.Message message)
		{
			var conversationId = message.PeerId.Value;
			var senderId = message.FromId.Value;
			var current = Conversations.FirstOrDefault(conversation => conversation.Id == conversationId);
			if (current == null)
				current = LoadConversationInfo(conversationId).Result;

			Member sender;
			if (senderId > 0)
				sender = new User(senderId,
					current.MembersHash.Contains(senderId)
						? ((User) current.Members[senderId]).FirstName
						: $"@id{senderId}",
					current.MembersHash.Contains(senderId)
						? ((User) current.Members[senderId]).LastName
						: string.Empty,
					current.Members?[senderId].PhotoUri);
			else
				sender = new Group(senderId,
					current.MembersHash.Contains(senderId)
						? current.Members?[senderId].Name
						: $"@group{-senderId}",
					current.Members?[senderId].PhotoUri);

			current.Messages.Add(new Message
			{
				Sender = sender,
				Text = message.Text,
				Date = message.Date,
				Attaches = message.Attachments
			});
		}

		private VkNet.Model.Message LongPoll(IProgress<VkNet.Model.Message> progress)
		{
			var response = Singleton.Api.Groups.GetLongPollServer((ulong) Singleton.GroupId);
			var ts = response.Ts;
			while (true)
			{
				var poll = Singleton.Api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
					{Server = response.Server, Ts = ts, Key = response.Key, Wait = 25});
				if (poll?.Updates == null) continue;
				foreach (var groupUpdate in poll.Updates)
					if (groupUpdate.Type == GroupUpdateType.MessageNew ||
					    groupUpdate.Type == GroupUpdateType.MessageReply)
						progress.Report(groupUpdate.MessageNew.Message);

				ts = poll.Ts;
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private async void LongPollSetup()
		{
			await Task.Factory.StartNew(() => LongPoll(new Progress<VkNet.Model.Message>(ReceiveMessage)));
		}
	}
}