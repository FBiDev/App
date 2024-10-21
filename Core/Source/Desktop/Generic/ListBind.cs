using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace App.Core.Desktop
{
    public class ListBind<T> : BindingList<T>
    {
        private bool isSortedValue;

        private ListSortDirection sortDirectionValue;

        private PropertyDescriptor sortPropertyValue;

        public ListBind()
        {
        }

        public ListBind(IList<T> list)
        {
            foreach (object item in list)
            {
                Add((T)item);
            }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return isSortedValue; }
        }

        public void SyncList(ListSynced<T> otherList)
        {
            otherList.ListChanged += (sender, e) =>
            {
                switch (e.ListChangedType)
                {
                    case ListSyncedChangedType.Reset: Clear(); break;
                    case ListSyncedChangedType.ItemAdded: Add(e.Item); break;
                    case ListSyncedChangedType.ItemDeleted: Remove(e.Item); break;
                    case ListSyncedChangedType.ItemInserted: Insert(e.Index, e.Item); break;
                }
            };
        }

        public void AddRange(IEnumerable<T> itemsToAdd)
        {
            foreach (T item in itemsToAdd)
            {
                Add(item);
            }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType == null && prop.PropertyType.IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (underlyingType != null)
                {
                    interfaceType = underlyingType.GetInterface("IComparable");
                }
            }

            if (interfaceType != null)
            {
                sortPropertyValue = prop;
                sortDirectionValue = direction;

                IEnumerable<T> query = Items;

                if (direction == ListSortDirection.Ascending)
                {
                    query = query.OrderBy(i => prop.GetValue(i));
                }
                else
                {
                    query = query.OrderByDescending(i => prop.GetValue(i));
                }

                int newIndex = 0;
                foreach (object item in query)
                {
                    Items[newIndex] = (T)item;
                    newIndex++;
                }

                isSortedValue = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                throw new NotSupportedException("Cannot sort by " + prop.Name +
                    ". This" + prop.PropertyType +
                    " does not implement IComparable");
            }
        }
    }
}