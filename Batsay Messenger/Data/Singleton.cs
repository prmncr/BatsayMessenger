using System;
using System.Net;
using System.Threading.Tasks;
using Batsay_Messenger.Architecture.Components.Window;
using VkNet;

namespace Batsay_Messenger.Data
{
	public static class Singleton
	{
		public static VkApi Api = new();

		public static long GroupId;

		public static string GroupName;

		public static Uri GroupPhoto50;

		public static Uri GroupPhoto100;

		public static MainWindowView MainViewInstance;

		public static MainWindowViewModel MainWindowViewModelInstance;

		public static Task<TOut> ExecuteAsync<T, TOut>(Func<T, Task<TOut>> func, T args) => 
			MainWindowViewModelInstance.ExecuteAsync(func, args);

		public static void ExecuteAsync(Action action) => 
			MainWindowViewModelInstance.ExecuteAsync(action);
	}
}
