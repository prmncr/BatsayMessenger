using System;
using System.Collections.Generic;
using VkNet.Model.Attachments;

namespace Batsay_Messenger.Data
{
	public class Message
	{
		public Member Sender { get; set; }

		public DateTime? Date { get; set; }

		public string Text { get; set; }

		public IEnumerable<Attachment> Attaches { get; set; }
	}
}