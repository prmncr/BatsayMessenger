using System;

namespace Batsay_Messenger.Data
{
	public abstract class Member
	{
		protected Member(long id, string name, Uri photoUri)
		{
			Id = id;
			PhotoUri = photoUri;
			Name = name;
		}

		public string Name { get; }

		public Uri PhotoUri { get; }

		public long Id { get; }
	}
}