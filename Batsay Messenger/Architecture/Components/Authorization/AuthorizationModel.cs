﻿using System;
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
				foreach (var (key, value) in tokens)
					_groups.Add(new AuthGroup( key, value));
			}
			else
			{
				File.Create("config");
				File.WriteAllLines("config", new[] {"{}"});
			}
		}

		public ObservableCollection<AuthGroup> GetGroups()
		{
			return _groups;
		}

		public async Task<bool> AuthorizeAsyncFormatter(object[] args)
		{
			var token = args[0] as string;
			var needValidation = (bool)args[1];
			return await AuthorizeAsync(token, needValidation);
		}
		
		public async Task<bool> AuthorizeAsync(string token, bool needValidation)
		{
			if (needValidation) ValidateToken(token);
			try
			{
				await Singleton.Api.AuthorizeAsync(new ApiAuthParams {AccessToken = token});
				var vkResponse = await Singleton.Api.Groups.GetByIdAsync(null, "0", null);
				var pubInfo = vkResponse.First();
				Singleton.GroupId = pubInfo.Id;
				Singleton.GroupName = pubInfo.Name;
				if (pubInfo.Photo50 != null) Singleton.GroupPhoto50 = pubInfo.Photo50;
				if (pubInfo.Photo100 != null) Singleton.GroupPhoto100 = pubInfo.Photo100;
				return true;
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Invalid token.\n{e.Message}");
			}
		}

		private async void ValidateToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token was empty.");
			if (_groups.Any(i => i.Token == token)) return;
			_groups.Add(new AuthGroup(token, Singleton.GroupName));
			await File.WriteAllTextAsync("config",
				JObject.FromObject(_groups.ToDictionary(group => group.Token, group => group.Name)).ToString());
		}
	}
}