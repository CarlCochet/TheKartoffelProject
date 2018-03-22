using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
namespace WorldEditor.Helpers.Collections
{
    public class AsyncVirtualizingCollection<T> : VirtualizingCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly SynchronizationContext _synchronizationContext;
        private bool _isLoading;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected SynchronizationContext SynchronizationContext
        {
            get
            {
                return _synchronizationContext;
            }
        }
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (_isLoading == value)
                {
                    return;
                }
                if (value != _isLoading)
                {
                    _isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
                FirePropertyChanged("IsLoading");
            }
        }
        public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider)
            : base(itemsProvider)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }
        public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize)
            : base(itemsProvider, pageSize)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }
        public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
            : base(itemsProvider, pageSize, pageTimeout)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler h = CollectionChanged;
            if (h != null)
            {
                h(this, e);
            }
        }
        private void FireCollectionReset()
        {
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null)
            {
                h(this, e);
            }
        }
        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(e);
        }
        protected override void LoadCount()
        {
            Count = 0;
            IsLoading = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCountWork));
        }
        private void LoadCountWork(object args)
        {
            int count = base.FetchCount();
            SynchronizationContext.Send(new SendOrPostCallback(LoadCountCompleted), count);
        }
        private void LoadCountCompleted(object args)
        {
            Count = (int)args;
            IsLoading = false;
            FireCollectionReset();
        }
        protected override void LoadPage(int index)
        {
            IsLoading = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadPageWork), index);
        }
        private void LoadPageWork(object args)
        {
            int pageIndex = (int)args;
            IList<T> page = base.FetchPage(pageIndex);
            SynchronizationContext.Send(new SendOrPostCallback(LoadPageCompleted), new object[]
			{
				pageIndex,
				page
			});
        }
        private void LoadPageCompleted(object args)
        {
            int pageIndex = (int)((object[])args)[0];
            IList<T> page = (IList<T>)((object[])args)[1];
            PopulatePage(pageIndex, page);
            IsLoading = false;
            FireCollectionReset();
        }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
