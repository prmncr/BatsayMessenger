using System;
using System.Threading.Tasks;
using System.Windows;
using BatsayMessenger.Architecture.Components.Auth;
using BatsayMessenger.Architecture.Controls;
using MaterialDesignThemes.Wpf;
using MVVMBase;

namespace BatsayMessenger.Architecture.Components.Window
{
	public class WindowViewModel : BaseViewModel
	{
		private static WindowViewModel _instance;
		private Thickness _borderPadding = new(0);
		private BaseCommand _closeCommand;
		private BaseCommand _closeOverlay;
		private IAppScreen _currentScreen;
		private bool _isLoadingVisible;
		private PackIconKind _maximizeButtonIcon = PackIconKind.WindowMaximize;
		private BaseCommand _maximizeCommand;
		private BaseCommand _minimizeCommand;
		private IOverlayControl _overlayContent;
		private bool _overlayVisibility;

		private WindowViewModel()
		{
			CurrentScreen = new AuthView();
			WindowView.Instance.StateChanged += (_, _) =>
			{
				MaximizeButtonIcon = WindowView.Instance.WindowState == WindowState.Normal
					? PackIconKind.WindowMaximize
					: PackIconKind.WindowRestore;
				BorderPadding = new Thickness(WindowView.Instance.WindowState == WindowState.Maximized ? 7 : 0);
			};
			_instance = this;
		}

		public static WindowViewModel Instance => _instance ??= new WindowViewModel();

		public static string Title => "Ba" + (new Random().NextDouble() < 0.01 ? "c" : "ts") + "ay Messenger";

		/// <summary>
		///     Uses for fixing WindowChrome's fullscreen paddings
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

		public IAppScreen CurrentScreen
		{
			get => _currentScreen;
			set
			{
				_currentScreen = value;
				OnPropertyChanged(nameof(CurrentScreen));
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
			if (VkClasses.Data.Api.IsAuthorized)
				VkClasses.Data.Api.LogOut();
			SystemCommands.CloseWindow(window);
		});

		public BaseCommand CloseOverlay => _closeOverlay ??= new BaseCommand(_ => OverlayContent = null);

		public async Task<TOut> ExecuteAsync<TOut>(Func<Task<TOut>> func)
		{
			IsLoadingVisible = true;
			var task = await func();
			IsLoadingVisible = false;
			return task;
		}

		public async Task ExecuteAsync(Func<Task> func)
		{
			IsLoadingVisible = true;
			await func();
			IsLoadingVisible = false;
		}
	}
}