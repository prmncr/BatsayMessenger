namespace Batsay_Messenger.Data
{
	public class AuthGroup
	{
		public string Name { get; }
		public string Token { get; }

		public AuthGroup(string token, string name)
		{
			Token = token;
			Name = name;
		}
	}
}