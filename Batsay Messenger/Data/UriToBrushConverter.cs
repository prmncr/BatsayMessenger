using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Batsay_Messenger.Data
{
	public class UriToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var random = new Random();
			return value == null
				? new SolidColorBrush(Color.FromRgb((byte) random.Next(256),
					(byte) random.Next(256), (byte) random.Next(256)))
				: new ImageBrush(new BitmapImage((Uri) value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}

		public static Brush Convert(Uri value)
		{
			return (Brush) new UriToBrushConverter().Convert(value, null, null, null);
		}
	}
}