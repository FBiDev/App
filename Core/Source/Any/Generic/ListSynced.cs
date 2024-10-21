using System;
using System.Collections.Generic;

namespace App.Core
{
    public enum ListSyncedChangedType
    {
        Reset = 0,
        ItemAdded = 1,
        ItemDeleted = 2,
        ItemInserted = 3
    }

    public class ListSynced<T> : List<T>
    {
        public event EventHandler<ListSyncedChangedEventArgs<T>> ListChanged;

        public new void Add(T item)
        {
            base.Add(item);
            OnListChanged(new ListSyncedChangedEventArgs<T>(ListSyncedChangedType.ItemAdded, item));
        }

        public new bool Remove(T item)
        {
            var result = base.Remove(item);
            if (result)
            {
                OnListChanged(new ListSyncedChangedEventArgs<T>(ListSyncedChangedType.ItemDeleted, item));
            }

            return result;
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnListChanged(new ListSyncedChangedEventArgs<T>(ListSyncedChangedType.ItemInserted, item, index));
        }

        public new void Clear()
        {
            base.Clear();
            OnListChanged(new ListSyncedChangedEventArgs<T>(ListSyncedChangedType.Reset));
        }

        protected virtual void OnListChanged(ListSyncedChangedEventArgs<T> e)
        {
            if (ListChanged == null)
            {
                return;
            }

            ListChanged.Invoke(this, e);
        }
    }
}