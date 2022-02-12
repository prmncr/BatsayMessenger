using System;
using System.Diagnostics;
using System.Windows.Threading;
using BatsayMessenger.Components.Window;
using BatsayMessenger.Properties;
using Essentials.MVVM;

namespace BatsayMessenger.Components;

internal class LinkExplorerViewModel : BaseViewModel
{
	private readonly DispatcherTimer _timer = new();

	private readonly string _url;
	private double _dismissButtonProgress;

	private int _dismissButtonProgressText;
	private BaseCommand _doNotOpenLink;

	private BaseCommand _openLink;
	private BaseCommand _openLinkR;

	private int _parts;

	public LinkExplorerViewModel(long url)
	{
		Url = $"https://vk.com/{(url > 0 ? "id" + url : "group" + -url)}";
		_timer.Interval = TimeSpan.FromMilliseconds(1);
		_timer.Tick += TimerOnTick;
		_timer.Start();
		DismissButtonProgress = 0;
		DismissButtonProgressText = 15;
	}

	public double DismissButtonProgress
	{
		get => _dismissButtonProgress;
		set
		{
			_dismissButtonProgress = value;
			OnPropertyChanged(nameof(DismissButtonProgress));
		}
	}

	public int DismissButtonProgressText
	{
		get => _dismissButtonProgressText;
		set
		{
			_dismissButtonProgressText = value;
			OnPropertyChanged(nameof(DismissButtonProgressText));
		}
	}

	public string Url
	{
		get => _url;
		init
		{
			_url = value;
			OnPropertyChanged(nameof(Url));
		}
	}

	public BaseCommand OpenLink => _openLink ??= new BaseCommand(_ =>
	{
		Process.Start(Url);
		_timer.Stop();
		DismissButtonProgress = 0;
		WindowViewModel.Instance.OverlayContent = null;
	});

	public BaseCommand OpenLinkAndDoNotRemind => _openLinkR ??= new BaseCommand(_ =>
	{
		Process.Start(Url);
		_timer.Stop();
		DismissButtonProgress = 0;
		Settings.Default.OpenLinkExplorer = false;
		Settings.Default.Save();
		WindowViewModel.Instance.OverlayContent = null;
	});

	public BaseCommand DoNotOpenLink => _doNotOpenLink ??= new BaseCommand(_ =>
	{
		_timer.Stop();
		DismissButtonProgress = 0;
		WindowViewModel.Instance.OverlayContent = null;
	});

	private void TimerOnTick(object sender, EventArgs e)
	{
		DismissButtonProgress += 100 / 1500d;
		_parts++;
		if (_parts % 100 == 0)
			DismissButtonProgressText = 15 - _parts / 100;

		if (Math.Abs(DismissButtonProgress - 100) > 0.1) return;
		Process.Start(Url);
		_timer.Stop();
		WindowViewModel.Instance.OverlayContent = null;
	}
}