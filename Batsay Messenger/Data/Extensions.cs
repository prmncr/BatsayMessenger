using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Batsay_Messenger.Data
{
	public static class ExtensionMethods
	{
		public static int Remove<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
		{
			var itemsToRemove = coll.Where(condition).ToList();

			foreach (var itemToRemove in itemsToRemove)
			{
				coll.Remove(itemToRemove);
			}

			return itemsToRemove.Count;
		}
	}
}
