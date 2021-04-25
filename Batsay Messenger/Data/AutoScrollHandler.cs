using System;
using System.Windows;
using System.Windows.Controls;

namespace Batsay_Messenger.Data
{
	class AutoScrollHandler : DependencyObject, IDisposable
	{
		private readonly ScrollViewer _scrollViewer;
		private bool _doScroll;

		public AutoScrollHandler(ScrollViewer scrollViewer)
		{
			_scrollViewer = scrollViewer ?? throw new ArgumentNullException(nameof(scrollViewer));
			_scrollViewer.ScrollToEnd();
			_scrollViewer.ScrollChanged += ScrollChanged;
		}

		private void ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			// User scroll event : set or unset autoscroll mode
			if (e.ExtentHeightChange == 0)
			{ _doScroll = _scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight; }

			// Content scroll event : autoscroll eventually
			if (_doScroll && e.ExtentHeightChange != 0)
			{ _scrollViewer.ScrollToVerticalOffset(_scrollViewer.ExtentHeight); }
		}

		public void Dispose()
		{
			_scrollViewer.ScrollChanged -= ScrollChanged;
		}
	}
}
