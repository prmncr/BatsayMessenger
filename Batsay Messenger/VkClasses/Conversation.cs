using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BatsayMessenger.Utils;

namespace BatsayMessenger.VkClasses;

public class Conversation
{
	public Conversation(long id, VkNet.Model.Conversation conversationData = null,
		Dictionary<long, Member> members = null)
	{
		Data = conversationData;
		Id = id;
		Title = Data != null ? Data.ChatSettings.Title : Id.ToString();
		Photo = Data != null
			? new ImageBrush(new BitmapImage(Data.ChatSettings.Photo.Photo100))
			: new SolidColorBrush(Id.ConvertToRgb());
		Members = members ?? new Dictionary<long, Member>();
	}

	public VkNet.Model.Conversation Data { get; }
	public string Title { get; }
	public long Id { get; }
	public Brush Photo { get; }
	public ObservableCollection<Message> Messages { get; } = new();
	public Dictionary<long, Member> Members { get; }
}