using System;
using Batsay_Messenger.Architecture.Components.Window;
using VkNet;

namespace Batsay_Messenger.Data
{
	public static class Singleton
	{
		public static readonly VkApi Api = new();
		public static long GroupId { get; set; }
		public static string GroupName { get; set; }
		public static Uri GroupPhoto50 { get; set; }
		public static Uri GroupPhoto100 { get; set; }
		public static MainWindowView ViewInstance => _viewInstance;
		public static MainWindowViewModel ViewModelInstance => _viewModelInstance;

		private static MainWindowView _viewInstance;
		private static MainWindowViewModel _viewModelInstance;
	}
}