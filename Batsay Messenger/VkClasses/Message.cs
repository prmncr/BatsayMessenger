using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VkNet.Model.Attachments;

namespace BatsayMessenger.VkClasses;

public class Message
{
	public Message(Member sender, DateTime? date, string text, IEnumerable<Attachment> attaches = null)
	{
		Sender = sender;
		Date = date;
		Text = text;
		Attaches = attaches == null ? null : new ObservableCollection<Attachment>(attaches);
	}

	public Member Sender { get; }

	public DateTime? Date { get; }

	public string Text { get; }

	public ObservableCollection<Attachment> Attaches { get; }
}