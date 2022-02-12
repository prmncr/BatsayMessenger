using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Essentials.MVVM;

namespace BatsayMessenger.Components;

public partial class ConversationControl : INotifyPropertyChanged
{
	private long _shortId;

	public ConversationControl()
	{
		InitializeComponent();
		ActionButton.Opacity = 0;
		RegisterName(ActionButton.Name, ActionButton);

		CreateAnimations(MouseEnterEvent, 1);
		CreateAnimations(MouseLeaveEvent, 0);
	}

	public string ConversationName
	{
		get => (string)GetValue(ConversationNameProperty);
		set => SetValue(ConversationNameProperty, value);
	}

	public long ConversationId
	{
		get
		{
			var r = (long)GetValue(ConversationIdProperty);
			ConversationIdShort = r;
			return r;
		}

		set => SetValue(ConversationIdProperty, value);
	}

	public long ConversationIdShort
	{
		get => _shortId;
		set
		{
			_shortId = value - (value > 2_000_000_000 ? 2_000_000_000 : 0);
			OnPropertyChanged(nameof(ConversationIdShort));
		}
	}

	public Brush ConversationPhoto
	{
		get => (Brush)GetValue(ConversationPhotoProperty);
		set
		{
			SetValue(ConversationPhotoProperty, value);
			ShortIdTextBlock.Visibility = Visibility.Collapsed;
		}
	}

	public BaseCommand Command
	{
		get => (BaseCommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	private void CreateAnimations(RoutedEvent e, double toValue)
	{
		var animation = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(200)));
		var storyboard = new Storyboard { Children = { animation } };
		Storyboard.SetTargetName(animation, nameof(ActionButton));
		Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));

		ConversationViewer.Triggers.Add(new EventTrigger
		{
			RoutedEvent = e,
			Actions = { new BeginStoryboard { Storyboard = storyboard } }
		});
	}

	#region PropertyChanged

	public event PropertyChangedEventHandler PropertyChanged;

	private void OnPropertyChanged(string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	#endregion

	#region DepencyProperties

	public static readonly DependencyProperty ConversationNameProperty = DependencyProperty.Register(
		"ConversationName",
		typeof(string), typeof(ConversationControl), new PropertyMetadata(default(string)));

	public static readonly DependencyProperty ConversationIdProperty = DependencyProperty.Register("ConversationId",
		typeof(long), typeof(ConversationControl), new PropertyMetadata(default(long)));

	public static readonly DependencyProperty ConversationPhotoProperty = DependencyProperty.Register(
		"ConversationPhoto", typeof(Brush), typeof(ConversationControl), new PropertyMetadata(default(Brush)));

	public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
		typeof(BaseCommand), typeof(ConversationControl), new PropertyMetadata(default(BaseCommand)));

	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
		"CommandParameter",
		typeof(object), typeof(ConversationControl), new PropertyMetadata(default(object)));

	#endregion
}