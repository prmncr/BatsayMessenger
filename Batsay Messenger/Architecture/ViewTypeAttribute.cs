using System;

namespace BatsayMessenger.Architecture
{
	public class ViewTypeAttribute : Attribute
	{
		public readonly ViewType ViewType;

		public ViewTypeAttribute(ViewType type)
		{
			ViewType = type;
		}
	}

	public enum ViewType
	{
		MainView,
		OverlayView
	}
}