using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ObservableCollectionRange<T> : ObservableCollection<T>
    {
        private bool _addingRange;
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChangedRange;
        public ObservableCollectionRange()
        {
        }
        public ObservableCollectionRange(List<T> list)
            : base(list)
        {
        }
        public ObservableCollectionRange(IEnumerable<T> collection)
            : base(collection)
        {
        }
        protected virtual void OnCollectionChangedRange(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChangedRange != null && !_addingRange)
            {
                using (base.BlockReentrancy())
                {
                    CollectionChangedRange(this, e);
                }
            }
        }
        public void AddRange(IEnumerable<T> collection)
        {
            base.CheckReentrancy();
            List<T> newItems = new List<T>();
            if (collection != null && base.Items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        _addingRange = true;
                        base.Add(enumerator.Current);
                        _addingRange = false;
                        newItems.Add(enumerator.Current);
                    }
                }
                OnCollectionChangedRange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems));
            }
        }
        protected override void ClearItems()
        {
            base.CheckReentrancy();
            List<T> oldItems = new List<T>(this);
            base.ClearItems();
            OnCollectionChangedRange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
        }
        protected override void InsertItem(int index, T item)
        {
            base.CheckReentrancy();
            base.InsertItem(index, item);
            OnCollectionChangedRange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.CheckReentrancy();
            T item = base[oldIndex];
            base.MoveItem(oldIndex, newIndex);
            OnCollectionChangedRange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }
        protected override void RemoveItem(int index)
        {
            base.CheckReentrancy();
            T item = base[index];
            base.RemoveItem(index);
            OnCollectionChangedRange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }
        protected override void SetItem(int index, T item)
        {
            base.CheckReentrancy();
            T oldItem = base[index];
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldItem, item, index));
        }
    }
}
