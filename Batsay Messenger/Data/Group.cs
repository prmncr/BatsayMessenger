using System;

namespace Batsay_Messenger.Data
{
	public class Group : Member
	{
		public long AbsId => -Id;

		public Group(long id, string name, Uri photoUri) : base(id, name, photoUri) { }
	}
}