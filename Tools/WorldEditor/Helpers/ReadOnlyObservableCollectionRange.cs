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
    public class ReadOnlyObservableCollectionRange<T> : ReadOnlyObservableCollection<T>
    {
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChangedRange;
        public ReadOnlyObservableCollectionRange(ObservableCollectionRange<T> list)
            : base(list)
        {
            list.CollectionChangedRange += new NotifyCollectionChangedEventHandler(HandleCollectionChangedRange);
        }
        private void HandleCollectionChangedRange(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChangedRange(e);
        }
        protected virtual void OnCollectionChangedRange(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChangedRange != null)
            {
                CollectionChangedRange(this, args);
            }
        }
    }
}
