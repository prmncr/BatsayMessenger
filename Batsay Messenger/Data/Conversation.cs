using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Batsay_Messenger.Data
{
	public class Conversation
	{
		public readonly VkNet.Model.Conversation Data;
		public readonly HashSet<long> MembersHash = new();

		public string Title => _title; 
		public long Id => _id;
		public Brush Photo => _photo;
		public ObservableCollection<Message> Messages => _messages;
		public Dictionary<long, Member> Members => _members;

		private readonly string _title;
		private readonly long _id;
		private readonly Brush _photo;
		private readonly ObservableCollection<Message> _messages = new();
		private readonly Dictionary<long, Member> _members;

		public Conversation(long id, VkNet.Model.Conversation conversationData = null,
			Dictionary<long, Member> members = null)
		{
			Data = conversationData;
			_id = id;
			_title = Data != null ? Data.ChatSettings.Title : _id.ToString();
			_photo = Data != null
				? new ImageBrush(new BitmapImage(Data.ChatSettings.Photo.Photo100))
				: new SolidColorBrush(_id.ConvertToRgb());
			_members = members;
			if (members == null) return;
			foreach (var member in  members)
				MembersHash.Add(member.Key);
		}
	}
}