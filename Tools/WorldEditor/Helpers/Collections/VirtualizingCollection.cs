using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
namespace WorldEditor.Helpers.Collections
{
    public class VirtualizingCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        private readonly IItemsProvider<T> _itemsProvider;
        private readonly int _pageSize = 100;
        private readonly long _pageTimeout = 10000L;
        private readonly bool m_alwaysFetchCount;
        private int _count = -1;
        private readonly Dictionary<int, IList<T>> _pages = new Dictionary<int, IList<T>>();
        private readonly Dictionary<int, DateTime> _pageTouchTimes = new Dictionary<int, DateTime>();
        public IItemsProvider<T> ItemsProvider
        {
            get
            {
                return _itemsProvider;
            }
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
        }
        public long PageTimeout
        {
            get
            {
                return _pageTimeout;
            }
        }
        public virtual int Count
        {
            get
            {
                if (_count == -1 || m_alwaysFetchCount)
                {
                    LoadCount();
                }
                return _count;
            }
            protected set
            {
                _count = value;
            }
        }
        public T this[int index]
        {
            get
            {
                int pageIndex = index / PageSize;
                int pageOffset = index % PageSize;
                RequestPage(pageIndex);
                if (pageOffset > PageSize / 2 && pageIndex < Count / PageSize)
                {
                    RequestPage(pageIndex + 1);
                }
                if (pageOffset < PageSize / 2 && pageIndex > 0)
                {
                    RequestPage(pageIndex - 1);
                }
                CleanUpPages();
                T result;
                if (_pages[pageIndex] == null)
                {
                    result = default(T);
                }
                else
                {
                    result = _pages[pageIndex][pageOffset];
                }
                return result;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }
        public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout, bool alwaysFetchCount)
        {
            _itemsProvider = itemsProvider;
            _pageSize = pageSize;
            _pageTimeout = (long)pageTimeout;
            m_alwaysFetchCount = alwaysFetchCount;
        }
        public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
        {
            _itemsProvider = itemsProvider;
            _pageSize = pageSize;
            _pageTimeout = (long)pageTimeout;
        }
        public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize)
        {
            _itemsProvider = itemsProvider;
            _pageSize = pageSize;
        }
        public VirtualizingCollection(IItemsProvider<T> itemsProvider)
        {
            _itemsProvider = itemsProvider;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
            yield break;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            throw new NotSupportedException();
        }
        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }
        bool IList.Contains(object value)
        {
            return Contains((T)((object)value));
        }
        public bool Contains(T item)
        {
            return false;
        }
        public void Clear()
        {
            throw new NotSupportedException();
        }
        int IList.IndexOf(object value)
        {
            return IndexOf((T)((object)value));
        }
        public int IndexOf(T item)
        {
            return -1;
        }
        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }
        void IList.Insert(int index, object value)
        {
            Insert(index, (T)((object)value));
        }
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }
        public void CleanUpPages()
        {
            if (PageTimeout >= 0L)
            {
                List<int> keys = new List<int>(_pageTouchTimes.Keys);
                foreach (int key in keys)
                {
                    if (key != 0 && (DateTime.Now - _pageTouchTimes[key]).TotalMilliseconds > (double)PageTimeout)
                    {
                        _pages.Remove(key);
                        _pageTouchTimes.Remove(key);
                        Trace.WriteLine("Removed Page: " + key);
                    }
                }
            }
        }
        protected virtual void PopulatePage(int pageIndex, IList<T> page)
        {
            Trace.WriteLine("Page populated: " + pageIndex);
            if (_pages.ContainsKey(pageIndex))
            {
                _pages[pageIndex] = page;
            }
        }
        protected virtual void RequestPage(int pageIndex)
        {
            if (!_pages.ContainsKey(pageIndex))
            {
                _pages.Add(pageIndex, null);
                _pageTouchTimes.Add(pageIndex, DateTime.Now);
                Trace.WriteLine("Added page: " + pageIndex);
                LoadPage(pageIndex);
            }
            else
            {
                _pageTouchTimes[pageIndex] = DateTime.Now;
            }
        }
        protected virtual void LoadCount()
        {
            Count = FetchCount();
        }
        protected virtual void LoadPage(int pageIndex)
        {
            PopulatePage(pageIndex, FetchPage(pageIndex));
        }
        protected IList<T> FetchPage(int pageIndex)
        {
            return ItemsProvider.FetchRange(pageIndex * PageSize, PageSize);
        }
        protected int FetchCount()
        {
            return ItemsProvider.FetchCount();
        }
    }
}
