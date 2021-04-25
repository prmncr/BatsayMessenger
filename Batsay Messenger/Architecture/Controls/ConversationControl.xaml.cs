using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Batsay_Messenger.Data;
using MaterialDesignThemes.Wpf;

namespace Batsay_Messenger.Architecture.Controls
{
	public partial class ConversationControl : UserControl
	{
		public static readonly DependencyProperty ConversationNameProperty = DependencyProperty.Register("ConversationName",
			typeof(string), typeof(ConversationControl), new PropertyMetadata(default(string)));

		public string ConversationName
		{
			get => (string) GetValue(ConversationNameProperty);
			set => SetValue(ConversationNameProperty, value);
		}

		public static readonly DependencyProperty ConversationIdProperty = DependencyProperty.Register("ConversationId",
				typeof(long), typeof(ConversationControl), new PropertyMetadata(default(long)));

		public long ConversationId
		{
			get => (long)GetValue(ConversationIdProperty);
			set => SetValue(ConversationIdProperty, value);
		}

		public static readonly DependencyProperty ConversationPhotoProperty = DependencyProperty.Register(
			"ConversationPhoto", typeof(Brush), typeof(ConversationControl), new PropertyMetadata(default(Brush)));

		public Brush ConversationPhoto
		{
			get => (Brush) GetValue(ConversationPhotoProperty);
			set => SetValue(ConversationPhotoProperty, value);
		}
		
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
			typeof(BaseCommand), typeof(ConversationControl), new PropertyMetadata(default(BaseCommand)));

		public BaseCommand Command
		{
			get => (BaseCommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
			typeof(object), typeof(ConversationControl), new PropertyMetadata(default(object)));

		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		public ConversationControl()
		{
			InitializeComponent();
			ActionButton.Opacity = 0;
			RegisterName(ActionButton.Name, ActionButton);

			CreateAnimations(MouseEnterEvent, 1);
			CreateAnimations(MouseLeaveEvent, 0);
		}

		private void CreateAnimations(RoutedEvent e, double toValue)
		{
			var animation = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(200)));
			var storyboard = new Storyboard { Children = { animation } };
			Storyboard.SetTargetName(animation, nameof(ActionButton));
			Storyboard.SetTargetProperty(animation, new PropertyPath(Button.OpacityProperty));

			ConversationViewer.Triggers.Add(new EventTrigger
			{
				RoutedEvent = e,
				Actions = { new BeginStoryboard { Storyboard = storyboard } }
			});
		}

		private void ConversationIdTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			ConversationIdTextBlock.Text = ConversationPhoto == null ? (ConversationId - 2000000000).ToString() : null;
		}
	}
}
