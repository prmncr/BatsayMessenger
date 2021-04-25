using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Batsay_Messenger.Data;
using Newtonsoft.Json.Linq;
using VkNet.Model;

namespace Batsay_Messenger.Architecture.Components.Authorization
{
	internal class AuthorizationModel
	{
		private readonly ObservableCollection<AuthGroup> _groups = new();

		public AuthorizationModel()
		{
			if (File.Exists("config"))
			{
				var tokens = JObject.Parse(File.ReadAllText("config")).ToObject<Dictionary<string, string>>();
				if (tokens == null || tokens.Count == 0) return;
				foreach (var pair in tokens)
					_groups.Add(new AuthGroup
					{
						Token = pair.Key,
						Name = pair.Value
					});
			}
			else
			{
				File.Create("config");
			}
		}

		public ObservableCollection<AuthGroup> GetGroups() => _groups;

		/// <summary>
		/// Method for VK auth.
		/// </summary>
		/// <param name="token">Group token</param>
		/// <returns>If auth completed successfully, returns null, else exception</returns>
		public async Task<Exception> AuthorizationAsync(string token)
		{
			try
			{
				await Singleton.Api.AuthorizeAsync(new ApiAuthParams {AccessToken = token});
				var vkResponse = await Singleton.Api.Groups.GetByIdAsync(null, "0", null);
				var pubInfo = vkResponse.First();
				Singleton.GroupId = pubInfo.Id;
				Singleton.GroupName = pubInfo.Name;
				if (pubInfo.Photo50 != null) Singleton.GroupPhoto50 = pubInfo.Photo50;

				if (pubInfo.Photo100 != null) Singleton.GroupPhoto100 = pubInfo.Photo100;

				return null;
			}
			catch (Exception e)
			{
				return e;
			}
		}

		public async Task<Exception> AuthorizationValidation(string token)
		{
			if (string.IsNullOrWhiteSpace(token)) return new ArgumentException();
			var auth = await AuthorizationAsync(token);

			if (auth != null) return auth;
			if (_groups.Any(i => i.Token == token)) return null;

			_groups.Add(new AuthGroup {Token = token, Name = Singleton.GroupName});

			File.WriteAllText("config",
				JObject.FromObject(_groups.ToDictionary(group => group.Token, group => group.Name)).ToString());
			return null;
		}
	}
}