using System.Windows.Media;

namespace BatsayMessenger.Utils
{
	public static class ObjectToBrushConverter
	{
		public static Color ConvertToRgb(this object obj)
		{
			var i = obj.GetHashCode();
			return Color.FromRgb((byte) ((i >> 16) & 0xFF), (byte) ((i >> 8) & 0xFF), (byte) (i & 0xFF));
		}
	}
}