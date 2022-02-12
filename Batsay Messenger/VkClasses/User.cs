using System;

namespace BatsayMessenger.VkClasses;

public class User : Member
{
	public User(long id, string firstName, string lastName, Uri photoUri) : base(id, $"{firstName} {lastName}",
		photoUri)
	{
		FirstName = firstName;
		LastName = lastName;
	}

	public string FirstName { get; }
	public string LastName { get; }
}