using System;
using Batsay_Messenger.Architecture.Controls;
using Batsay_Messenger.Data;

namespace Batsay_Messenger.Architecture.Components.Window
{
	public partial class MainWindowView
	{
		private static MainWindowViewModel _viewModel;

		public IAppScreen CurrentScreen
		{
			get => _viewModel.CurrentContent;
			set => _viewModel.CurrentContent = value;
		}

		public MainWindowView()
		{
			Singleton.MainViewInstance = this;
			InitializeComponent();
			_viewModel = (MainWindowViewModel) DataContext;
			Singleton.MainWindowViewModelInstance = (MainWindowViewModel) DataContext;
		}

		public IOverlayControl OverlayContent
		{
			get => _viewModel.OverlayContent;
			set => _viewModel.OverlayContent = value;
		}
	}
}