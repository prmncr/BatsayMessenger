using System;
using VkNet;

namespace BatsayMessenger.VkClasses;

public static class Data
{
	public static readonly VkApi Api = new();
	public static long GroupId { get; set; }
	public static string GroupName { get; set; }
	public static Uri GroupPhoto50 { get; set; }
	public static Uri GroupPhoto100 { get; set; }
}