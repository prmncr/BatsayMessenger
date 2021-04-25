using System.Windows;
using System.Windows.Controls;

namespace Batsay_Messenger.Data
{
	internal class AutoScroll
	{
		public static readonly DependencyProperty AutoScrollProperty =
			DependencyProperty.RegisterAttached("AutoScrollToEnd",
				typeof(bool), typeof(AutoScroll),
				new PropertyMetadata(false, HookupAutoScrollToEnd));

		public static readonly DependencyProperty AutoScrollHandlerProperty =
			DependencyProperty.RegisterAttached("AutoScrollToEndHandler",
				typeof(AutoScrollHandler), typeof(AutoScroll));

		private static void HookupAutoScrollToEnd(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			if (!(d is ScrollViewer scrollViewer)) return;

			SetAutoScrollToEnd(scrollViewer, (bool) e.NewValue);
		}

		public static bool GetAutoScrollToEnd(ScrollViewer instance)
		{
			return (bool) instance.GetValue(AutoScrollProperty);
		}

		public static void SetAutoScrollToEnd(ScrollViewer instance, bool value)
		{
			var oldHandler = (AutoScrollHandler) instance.GetValue(AutoScrollHandlerProperty);
			if (oldHandler != null)
			{
				oldHandler.Dispose();
				instance.SetValue(AutoScrollHandlerProperty, null);
			}

			instance.SetValue(AutoScrollProperty, value);
			if (value)
				instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));
		}
	}
}