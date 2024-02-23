using System;
using System.Collections.Generic;

namespace App.Core
{
    public class ListSynced<T> : List<T>
    {
        public event EventHandler<ListChangedEventArgs<T>> ListChanged;

        public new void Add(T item)
        {
            base.Add(item);
            OnListChanged(new ListChangedEventArgs<T>(ListSyncedChangedType.ItemAdded, item));
        }

        public new bool Remove(T item)
        {
            var result = base.Remove(item);
            if (result)
            {
                OnListChanged(new ListChangedEventArgs<T>(ListSyncedChangedType.ItemDeleted, item));
            }
            return result;
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnListChanged(new ListChangedEventArgs<T>(ListSyncedChangedType.ItemInserted, item, index));
        }

        public new void Clear()
        {
            base.Clear();
            OnListChanged(new ListChangedEventArgs<T>(ListSyncedChangedType.Reset));
        }

        protected virtual void OnListChanged(ListChangedEventArgs<T> e)
        {
            if (ListChanged == null)
                return;
            ListChanged.Invoke(this, e);
        }
    }

    public enum ListSyncedChangedType
    {
        Reset = 0,
        ItemAdded = 1,
        ItemDeleted = 2,
        ItemInserted = 3
    }

    public class ListChangedEventArgs<T> : EventArgs
    {
        public ListSyncedChangedType ListChangedType { get; set; }
        public T Item { get; set; }
        public int Index { get; set; }

        public ListChangedEventArgs(ListSyncedChangedType listChangedType, T item = default(T), int index = -1)
        {
            ListChangedType = listChangedType;
            Item = item;
            Index = index;
        }
    }
}