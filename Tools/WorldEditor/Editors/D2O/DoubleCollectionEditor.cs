using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace WorldEditor.Editors.Files.D2O
{
    public class DoubleCollectionEditor : Control, IPersistableChanged
    {
        public static readonly DependencyProperty ItemsProperty;
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty SelectedSubListProperty;
        public static readonly DependencyProperty SelectedSubListSourceProperty;
        public static readonly DependencyProperty NewItemTypesProperty;
        public static readonly DependencyProperty SelectedItemProperty;
        private Type m_listType;

        public IList<Type> NewItemTypes
        {
            get { return (IList<Type>)base.GetValue(DoubleCollectionEditor.NewItemTypesProperty); }
            set { base.SetValue(DoubleCollectionEditor.NewItemTypesProperty, value); }
        }

        public Type SubListType
        {
            get { return (m_listType != null) ? typeof(List<>).MakeGenericType(m_listType) : null; }
        }

        public ObservableCollection<IList> Items
        {
            get { return (ObservableCollection<IList>)base.GetValue(DoubleCollectionEditor.ItemsProperty); }
            set { base.SetValue(DoubleCollectionEditor.ItemsProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)base.GetValue(DoubleCollectionEditor.ItemsSourceProperty); }
            set { base.SetValue(DoubleCollectionEditor.ItemsSourceProperty, value); }
        }

        public ObservableCollection<object> SelectedSubList
        {
            get { return (ObservableCollection<object>)base.GetValue(DoubleCollectionEditor.SelectedSubListProperty); }
            set { base.SetValue(DoubleCollectionEditor.SelectedSubListProperty, value); }
        }

        public IList SelectedSubListSource
        {
            get { return (IList)base.GetValue(DoubleCollectionEditor.SelectedSubListSourceProperty); }
            set { base.SetValue(DoubleCollectionEditor.SelectedSubListSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return base.GetValue(DoubleCollectionEditor.SelectedItemProperty); }
            set { base.SetValue(DoubleCollectionEditor.SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DoubleCollectionEditor CollectionControl = (DoubleCollectionEditor)d;
            if (CollectionControl != null)
            {
                CollectionControl.OnItemSourceChanged((IList)e.OldValue, (IList)e.NewValue);
            }
        }

        public void OnItemSourceChanged(IList oldValue, IList newValue)
        {
            m_listType = null;
            Type oldType = (oldValue != null) ? oldValue.GetType() : null;
            Type oldListType = (oldType != null) ? oldType.GetGenericArguments()[0].GetGenericArguments()[0] : null;
            if (newValue != null)
            {
                Items.Clear();
                foreach (IList item in newValue)
                {
                    Items.Add((IList)CreateClone(item));
                }
                Type type = newValue.GetType();
                m_listType = type.GetGenericArguments()[0].GetGenericArguments()[0];
                if (!NewItemTypes.Contains(m_listType))
                {
                    NewItemTypes.Insert(0, m_listType);
                }
                if (oldListType != null && m_listType != oldListType && !m_listType.IsSubclassOf(oldListType))
                {
                    NewItemTypes.Remove(oldListType);
                }
            }
        }

        private static void OnSubListSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DoubleCollectionEditor CollectionControl = (DoubleCollectionEditor)d;
            if (CollectionControl != null)
            {
                CollectionControl.OnSubListSourceChanged((IList)e.OldValue, (IList)e.NewValue);
            }
        }

        private void OnSubListSourceChanged(IList oldValue, IList newValue)
        {
            SelectedSubList.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnSelectedSubListChanged);
            SelectedSubList.Clear();
            foreach (object obj in newValue)
            {
                SelectedSubList.Add(obj);
            }
            SelectedSubList.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSelectedSubListChanged);
        }

        private void OnSelectedSubListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    object item = e.NewItems[i];
                    SelectedSubListSource.Insert(e.NewStartingIndex + i, item);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    SelectedSubListSource.Remove(item);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                SelectedSubListSource.Clear();
            }
        }

        static DoubleCollectionEditor()
        {
            DoubleCollectionEditor.ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<IList>), typeof(DoubleCollectionEditor), new UIPropertyMetadata(null));
            DoubleCollectionEditor.ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(DoubleCollectionEditor), new UIPropertyMetadata(null, new PropertyChangedCallback(DoubleCollectionEditor.OnItemsSourceChanged)));
            DoubleCollectionEditor.SelectedSubListProperty = DependencyProperty.Register("SelectedSubList", typeof(ObservableCollection<object>), typeof(DoubleCollectionEditor), new UIPropertyMetadata(null));
            DoubleCollectionEditor.SelectedSubListSourceProperty = DependencyProperty.Register("SelectedSubListSource", typeof(IList), typeof(DoubleCollectionEditor), new UIPropertyMetadata(null, new PropertyChangedCallback(DoubleCollectionEditor.OnSubListSourceChanged)));
            DoubleCollectionEditor.NewItemTypesProperty = DependencyProperty.Register("NewItemTypes", typeof(IList), typeof(DoubleCollectionEditor), new UIPropertyMetadata(null));
            DoubleCollectionEditor.SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DoubleCollectionEditor), new PropertyMetadata(null));
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/WorldEditor;component/Editors/D2O/Template.xaml", UriKind.Relative)
            });
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleCollectionEditor), new FrameworkPropertyMetadata(typeof(DoubleCollectionEditor)));
        }

        public DoubleCollectionEditor(Type listType)
        {
            m_listType = listType;
            Items = new ObservableCollection<IList>();
            SelectedSubList = new ObservableCollection<object>();
            base.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, new ExecutedRoutedEventHandler(AddNew), new CanExecuteRoutedEventHandler(CanAddNew)));
            base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, new ExecutedRoutedEventHandler(Delete), new CanExecuteRoutedEventHandler(CanDelete)));
            base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, new ExecutedRoutedEventHandler(MoveDown), new CanExecuteRoutedEventHandler(CanMoveDown)));
            base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, new ExecutedRoutedEventHandler(MoveUp), new CanExecuteRoutedEventHandler(CanMoveUp)));
        }

        private void AddNew(object sender, ExecutedRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            object newItem = (list == Items) ? CreateNewItem((Type)e.Parameter) : CreateNewItem((Type)e.Parameter);
            list.Add(newItem);
            if (list == Items)
            {
                SelectedSubListSource = (IList)newItem;
            }
            else
            {
                SelectedItem = newItem;
            }
        }

        private void CanAddNew(object sender, CanExecuteRoutedEventArgs e)
        {
            Type t = e.Parameter as Type;
            if (t != null && t.GetConstructor(Type.EmptyTypes) != null)
            {
                e.CanExecute = true;
            }
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            list.Remove(e.Parameter);
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (e.Parameter != null);
        }

        private void MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            object selectedItem = e.Parameter;
            int index = list.IndexOf(selectedItem);
            list.RemoveAt(index);
            list.Insert(index + 1, selectedItem);
            if (list == Items)
            {
                SelectedSubListSource = (IList)selectedItem;
            }
            else
            {
                SelectedItem = selectedItem;
            }
        }

        private void CanMoveDown(object sender, CanExecuteRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            if (e.Parameter != null && list.IndexOf(e.Parameter) < Items.Count - 1)
            {
                e.CanExecute = true;
            }
        }

        private void MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            object selectedItem = e.Parameter;
            int index = list.IndexOf(selectedItem);
            list.RemoveAt(index);
            list.Insert(index - 1, selectedItem);
            if (list == Items)
            {
                SelectedSubListSource = (IList)selectedItem;
            }
            else
            {
                SelectedItem = selectedItem;
            }
        }

        private void CanMoveUp(object sender, CanExecuteRoutedEventArgs e)
        {
            IList list = (IList)((FrameworkElement)e.OriginalSource).Tag;
            if (e.Parameter != null && list.IndexOf(e.Parameter) > 0)
            {
                e.CanExecute = true;
            }
        }

        public void PersistChanges()
        {
            IList list = ComputeItemsSource();
            if (list != null)
            {
                list.Clear();
                foreach (object item in Items)
                {
                    list.Add(item);
                }
            }
        }

        private static void CopyValues(object source, object destination)
        {
            FieldInfo[] myObjectFields = source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] array = myObjectFields;
            for (int i = 0; i < array.Length; i++)
            {
                FieldInfo fi = array[i];
                fi.SetValue(destination, fi.GetValue(source));
            }
        }

        private object CreateClone(object source)
        {
            Type type = source.GetType();
            object clone = Activator.CreateInstance(type);
            DoubleCollectionEditor.CopyValues(source, clone);
            return clone;
        }

        private object CreateNewItem(Type type)
        {
            return Activator.CreateInstance(type);
        }

        internal static Type GetListItemType(Type listType)
        {
            Type iListOfT = listType.GetInterfaces().FirstOrDefault((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
            return (iListOfT != null) ? iListOfT.GetGenericArguments()[0] : null;
        }

        private IList ComputeItemsSource()
        {
            return ItemsSource;
        }
    }
}