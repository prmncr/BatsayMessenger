using System;
using System.Threading.Tasks;
using System.Windows;
using Batsay_Messenger.Architecture.Components.Authorization;
using Batsay_Messenger.Architecture.Controls;
using Batsay_Messenger.Data;
using MaterialDesignThemes.Wpf;

namespace Batsay_Messenger.Architecture.Components.Window
{
	public class MainWindowViewModel : BaseViewModel
	{
		#region Private variables

		private IAppScreen _currentContent;
		private IOverlayControl _overlayContent;
		private bool _overlayVisibility;
		private bool _isLoadingVisible;
		private BaseCommand _minimizeCommand;
		private BaseCommand _maximizeCommand;
		private BaseCommand _closeCommand;
		private PackIconKind _maximizeButtonIcon = PackIconKind.WindowMaximize;
		private Thickness _borderPadding = new Thickness(0);

		#endregion

		public string Title => "Ba" + (new Random().NextDouble() < 0.01 ? "c" : "ts") + "ay Messenger";

		/// <summary>
		/// Uses for fixing WindowChrome paddings
		/// </summary>
		public Thickness BorderPadding
		{
			get => _borderPadding;
			set
			{
				_borderPadding = value;
				OnPropertyChanged(nameof(BorderPadding));
			}
		}
		
		public IAppScreen CurrentContent
		{
			get => _currentContent;
			set
			{
				_currentContent = value;
				OnPropertyChanged(nameof(CurrentContent));
			}
		}

		public bool IsOverlayVisible
		{
			get => _overlayVisibility;
			private set
			{
				_overlayVisibility = value;
				OnPropertyChanged(nameof(IsOverlayVisible));
			}
		}

		public IOverlayControl OverlayContent
		{
			get => _overlayContent;
			set
			{
				_overlayContent = value;
				IsOverlayVisible = _overlayContent != null;
				OnPropertyChanged(nameof(OverlayContent));
			}
		}

		public bool IsLoadingVisible
		{
			get => _isLoadingVisible;
			set
			{
				_isLoadingVisible = value;
				OnPropertyChanged(nameof(IsLoadingVisible));
			}
		}

		public PackIconKind MaximizeButtonIcon
		{
			get => _maximizeButtonIcon;
			set
			{
				_maximizeButtonIcon = value;
				OnPropertyChanged(nameof(MaximizeButtonIcon));
			}
		}

		public BaseCommand MinimizeCommand => _minimizeCommand ??= new BaseCommand(obj =>
		{
			if (obj == null) return;
			var window = (System.Windows.Window) obj;
			SystemCommands.MinimizeWindow(window);
		});

		public BaseCommand MaximizeCommand => _maximizeCommand ??= new BaseCommand(obj =>
		{
			if (obj == null) return;
			var window = (System.Windows.Window) obj;
			if (window.WindowState == WindowState.Normal)
				SystemCommands.MaximizeWindow(window);
			else
				SystemCommands.RestoreWindow(window);
		});

		public BaseCommand CloseCommand => _closeCommand ??= new BaseCommand(obj =>
		{
			if (obj == null) return;
			var window = (System.Windows.Window) obj;
			if (Singleton.Api.IsAuthorized)
				Singleton.Api.LogOut();
			SystemCommands.CloseWindow(window);
		});

		public MainWindowViewModel()
		{
			CurrentContent = new AuthorizationView();
			Singleton.MainViewInstance.StateChanged += (sender, args) =>
			{
				MaximizeButtonIcon = Singleton.MainViewInstance.WindowState == WindowState.Normal
					? PackIconKind.WindowMaximize
					: PackIconKind.WindowRestore;
				BorderPadding = new Thickness(Singleton.MainViewInstance.WindowState == WindowState.Maximized ? 5 : 0);
			};
		}

		public async Task<TOut> ExecuteAsync<T, TOut>(Func<T, Task<TOut>> func, T args)
		{
			IsLoadingVisible = true;
			var answer = await func(args);
			IsLoadingVisible = false;
			return answer;
		}

		public void ExecuteAsync(Action action)
		{
			IsLoadingVisible = true;
			Task.Run(action).ContinueWith(new Action<Task>(_ => IsLoadingVisible = false));
		}
	}
}