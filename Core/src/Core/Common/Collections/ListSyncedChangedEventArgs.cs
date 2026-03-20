using System;

namespace App.Core
{
    public class ListSyncedChangedEventArgs<T> : EventArgs
    {
        public ListSyncedChangedEventArgs(ListSyncedChangedType listChangedType, T item = default(T), int index = -1)
        {
            ListChangedType = listChangedType;
            Item = item;
            Index = index;
        }

        public ListSyncedChangedType ListChangedType { get; set; }

        public T Item { get; set; }

        public int Index { get; set; }
    }
}