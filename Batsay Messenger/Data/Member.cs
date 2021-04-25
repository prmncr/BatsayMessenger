using System;

namespace Batsay_Messenger.Data
{
	public abstract class Member
	{
		public string Name => _name;
		public Uri PhotoUri => _photoUri;
		public long Id => _id;

		private readonly string _name;
		private readonly Uri _photoUri;
		private readonly long _id;

		protected Member(long id, string name, Uri photoUri)
		{
			_id = id;
			_photoUri = photoUri;
			_name = name;
		}
	}
}