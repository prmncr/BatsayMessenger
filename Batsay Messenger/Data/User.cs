using System;

namespace Batsay_Messenger.Data
{
	public class User : Member
	{
		public readonly string FirstName;
		public readonly string LastName;

		public User(long id, string firstName, string lastName, Uri photoUri) : base(id, $"{firstName} {lastName}", photoUri)
		{
			FirstName = firstName;
			LastName = lastName;
		}	
	}
}