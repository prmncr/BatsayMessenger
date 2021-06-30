namespace BatsayMessenger.Architecture.Components.Window
{
	public partial class WindowView
	{
		private static WindowView _instance;

		public WindowView()
		{
			_instance = this;
			var viewModel = WindowViewModel.Instance;
			DataContext = viewModel;
		}

		public static WindowView Instance => _instance ??= new WindowView();
	}
}