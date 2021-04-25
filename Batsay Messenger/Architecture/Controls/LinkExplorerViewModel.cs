using System;
using System.Diagnostics;
using System.Windows.Threading;
using Batsay_Messenger.Architecture.Components.Window;
using Batsay_Messenger.Data;
using Settings = Batsay_Messenger.Properties.Settings;

namespace Batsay_Messenger.Architecture.Controls
{
	internal class LinkExplorerViewModel : BaseViewModel
	{
		private readonly DispatcherTimer _timer = new DispatcherTimer();

		public LinkExplorerViewModel(long url)
		{
			Url = $"https://vk.com/{(url > 0 ? "id" + url : "group" + -url)}";
			_timer.Interval = TimeSpan.FromMilliseconds(1);
			_timer.Tick += TimerOnTick;
			_timer.Start();
			DismissButtonProgress = 0;
			DismissButtonProgressText = 15;
		}

		private void TimerOnTick(object sender, EventArgs e)
		{
			DismissButtonProgress += 100 / 1500d;
			_parts++;
			if (_parts % 100 == 0)
				DismissButtonProgressText = 15 - _parts / 100;

			if (Math.Abs(DismissButtonProgress - 100) > 0.1) return;
			Process.Start(Url);
			_timer.Stop();
			Singleton.MainViewInstance.OverlayContent = null;
		}

		private int _parts;
		private double _dismissButtonProgress;

		public double DismissButtonProgress
		{
			get => _dismissButtonProgress;
			set
			{
				_dismissButtonProgress = value;
				OnPropertyChanged(nameof(DismissButtonProgress));
			}
		}

		private int _dismissButtonProgressText;

		public int DismissButtonProgressText
		{
			get => _dismissButtonProgressText;
			set
			{
				_dismissButtonProgressText = value;
				OnPropertyChanged(nameof(DismissButtonProgressText));
			}
		}

		private string _url;

		public string Url
		{
			get => _url;
			set
			{
				_url = value;
				OnPropertyChanged(nameof(Url));
			}
		}

		private BaseCommand _openLink;
		private BaseCommand _openLinkR;
		private BaseCommand _doNotOpenLink;

		public BaseCommand OpenLink => _openLink ??= new BaseCommand(obj =>
		{
			Process.Start(Url);
			_timer.Stop();
			DismissButtonProgress = 0;
			Singleton.MainViewInstance.OverlayContent = null;
		});

		public BaseCommand OpenLinkAndDoNotRemind => _openLinkR ??= new BaseCommand(obj =>
		{
			Process.Start(Url);
			_timer.Stop();
			DismissButtonProgress = 0;
			Settings.Default.OpenLinkExplorer = false;
			Settings.Default.Save();
			Singleton.MainViewInstance.OverlayContent = null;
		});

		public BaseCommand DoNotOpenLink => _doNotOpenLink ??= new BaseCommand(obj =>
		{
			_timer.Stop();
			DismissButtonProgress = 0;
			Singleton.MainViewInstance.OverlayContent = null;
		});
	}
}