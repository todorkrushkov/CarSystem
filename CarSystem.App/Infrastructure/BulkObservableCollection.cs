using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CarSystem.App.Infrastructure
{
	public class BulkObservableCollection<T> : ObservableCollection<T>
	{
		private bool _suppressNotifications;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!_suppressNotifications)
				base.OnCollectionChanged(e);
		}

		public void ReplaceAll(IEnumerable<T> items)
		{
			_suppressNotifications = true;
			Clear();
			foreach (var item in items)
				Add(item);
			_suppressNotifications = false;
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
