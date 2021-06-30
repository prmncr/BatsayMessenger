namespace BatsayMessenger.VkClasses
{
	public class AuthGroup
	{
		public AuthGroup(string token, string name)
		{
			Token = token;
			Name = name;
		}

		public string Name { get; }
		public string Token { get; }
	}
}