using System;
using System.Windows;
using System.Windows.Controls;

namespace BatsayMessenger.Utils
{
	internal class AutoScrollHandler : DependencyObject, IDisposable
	{
		private readonly ScrollViewer _scrollViewer;
		private bool _doScroll;

		public AutoScrollHandler(ScrollViewer scrollViewer)
		{
			_scrollViewer = scrollViewer ?? throw new ArgumentNullException(nameof(scrollViewer));
			_scrollViewer.ScrollToEnd();
			_scrollViewer.ScrollChanged += ScrollChanged;
		}

		public void Dispose()
		{
			_scrollViewer.ScrollChanged -= ScrollChanged;
		}

		private void ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.ExtentHeightChange == 0) _doScroll = _scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight;
			if (_doScroll && e.ExtentHeightChange != 0)
				_scrollViewer.ScrollToVerticalOffset(_scrollViewer.ExtentHeight);
		}
	}
}