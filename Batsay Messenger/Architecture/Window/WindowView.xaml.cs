using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BatsayMessenger.Architecture.Window
{
	public partial class WindowView
	{
		private static WindowView _instance;

		public WindowView()
		{
			_instance = this;
		}

		private void OpenFirstScreen(object sender, RoutedEventArgs e)
		{
			((WindowViewModel)DataContext).OpenFirstScreen();
		}

		public static WindowView Instance => _instance ??= new WindowView();

		public static void FadeOut()
		{
			((Control)Instance?.MainPresenter?.Content)?.BeginAnimation(OpacityProperty,
				new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(500))));
		}

		public static void FadeIn()
		{
			((Control)Instance?.MainPresenter?.Content)?.BeginAnimation(OpacityProperty,
				new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500))));
		}
	}
}