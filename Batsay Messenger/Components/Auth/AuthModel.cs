using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatsayMessenger.VkClasses;
using Newtonsoft.Json.Linq;
using VkNet.Model;

namespace BatsayMessenger.Components.Auth;

internal class AuthModel
{
	private readonly ObservableCollection<AuthGroup> _groups = new();

	public AuthModel()
	{
		if (File.Exists("config"))
		{
			var tokens = JObject.Parse(File.ReadAllText("config")).ToObject<Dictionary<string, string>>();
			if (tokens == null || tokens.Count == 0) return;
			foreach (var (key, value) in tokens)
				_groups.Add(new AuthGroup(key, value));
		}
		else
		{
			File.Create("config").Write(Encoding.Default.GetBytes("{}"));
		}
	}

	public ObservableCollection<AuthGroup> GetGroups()
	{
		return _groups;
	}

	public async Task<bool> AuthorizeAsync(string token, bool needValidation)
	{
		if (string.IsNullOrWhiteSpace(token))
			throw new ArgumentException("Token was empty.");
		try
		{
			await Data.Api.AuthorizeAsync(new ApiAuthParams { AccessToken = token });
			var vkResponse = await Data.Api.Groups.GetByIdAsync(null, "0", null);
			var group = vkResponse.First();
			Data.GroupId = group.Id;
			Data.GroupName = group.Name;
			if (group.Photo50 != null) Data.GroupPhoto50 = group.Photo50;
			if (group.Photo100 != null) Data.GroupPhoto100 = group.Photo100;
			if (needValidation) AddNewToken(token);
			return true;
		}
		catch (Exception e)
		{
			throw new ArgumentException($"Invalid token.\n{e.Message}");
		}
	}

	private async void AddNewToken(string token)
	{
		_groups.Add(new AuthGroup(token, Data.GroupName));
		await File.WriteAllTextAsync("config",
			JObject.FromObject(_groups.ToDictionary(group => group.Token, group => group.Name)).ToString());
	}
}