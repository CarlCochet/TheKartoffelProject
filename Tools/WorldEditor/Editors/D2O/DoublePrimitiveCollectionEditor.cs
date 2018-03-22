using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace WorldEditor.Editors.Files.D2O
{
    public class DoublePrimitiveCollectionEditor : Control, IPersistableChanged
    {
        private Type m_listType;
        public static readonly DependencyProperty ItemsProperty;
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty SelectedItemProperty;

        public ObservableCollection<IList> Items
        {
            get { return (ObservableCollection<IList>)base.GetValue(DoublePrimitiveCollectionEditor.ItemsProperty); }
            set { base.SetValue(DoublePrimitiveCollectionEditor.ItemsProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)base.GetValue(DoublePrimitiveCollectionEditor.ItemsSourceProperty); }
            set { base.SetValue(DoublePrimitiveCollectionEditor.ItemsSourceProperty, value); }
        }

        public IList SelectedSubList
        {
            get { return (IList)base.GetValue(DoublePrimitiveCollectionEditor.SelectedItemProperty); }
            set { base.SetValue(DoublePrimitiveCollectionEditor.SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DoublePrimitiveCollectionEditor CollectionControl = (DoublePrimitiveCollectionEditor)d;
            if (CollectionControl != null)
            {
                CollectionControl.OnItemSourceChanged((IList)e.OldValue, (IList)e.NewValue);
            }
        }

        public void OnItemSourceChanged(IList oldValue, IList newValue)
        {
            m_listType = null;
            if (newValue != null)
            {
                foreach (IList item in newValue)
                {
                    Items.Add((IList)CreateClone(item));
                }
                Type type = newValue.GetType();
                m_listType = type.GetGenericArguments()[0].GetGenericArguments()[0];
            }
        }

        static DoublePrimitiveCollectionEditor()
        {
            DoublePrimitiveCollectionEditor.ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<IList>), typeof(DoublePrimitiveCollectionEditor), new UIPropertyMetadata(null));
            DoublePrimitiveCollectionEditor.ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(DoublePrimitiveCollectionEditor), new UIPropertyMetadata(null, new PropertyChangedCallback(DoublePrimitiveCollectionEditor.OnItemsSourceChanged)));
            DoublePrimitiveCollectionEditor.SelectedItemProperty = DependencyProperty.Register("SelectedSubList", typeof(IList), typeof(DoublePrimitiveCollectionEditor), new UIPropertyMetadata(null));
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/WorldEditor;component/Editors/D2O/Template.xaml", UriKind.Relative)
            });
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DoublePrimitiveCollectionEditor), new FrameworkPropertyMetadata(typeof(DoublePrimitiveCollectionEditor)));
        }

        public DoublePrimitiveCollectionEditor(Type listType)
        {
            m_listType = listType;
            Items = new ObservableCollection<IList>();
            base.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, new ExecutedRoutedEventHandler(AddNew), new CanExecuteRoutedEventHandler(CanAddNew)));
            base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, new ExecutedRoutedEventHandler(Delete), new CanExecuteRoutedEventHandler(CanDelete)));
            base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, new ExecutedRoutedEventHandler(MoveDown), new CanExecuteRoutedEventHandler(CanMoveDown)));
            base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, new ExecutedRoutedEventHandler(MoveUp), new CanExecuteRoutedEventHandler(CanMoveUp)));
        }

        private void AddNew(object sender, ExecutedRoutedEventArgs e)
        {
            IList newItem = CreateNewItem(m_listType);
            Items.Add(newItem);
            SelectedSubList = newItem;
        }

        private void CanAddNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            Items.Remove((IList)e.Parameter);
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (e.Parameter != null);
        }

        private void MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            IList selectedItem = (IList)e.Parameter;
            int index = Items.IndexOf(selectedItem);
            Items.RemoveAt(index);
            Items.Insert(index + 1, selectedItem);
            SelectedSubList = selectedItem;
        }

        private void CanMoveDown(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null && Items.IndexOf((IList)e.Parameter) < Items.Count - 1)
            {
                e.CanExecute = true;
            }
        }

        private void MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            IList selectedItem = (IList)e.Parameter;
            int index = Items.IndexOf(selectedItem);
            Items.RemoveAt(index);
            Items.Insert(index - 1, selectedItem);
            SelectedSubList = selectedItem;
        }

        private void CanMoveUp(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null && Items.IndexOf((IList)e.Parameter) > 0)
            {
                e.CanExecute = true;
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
            DoublePrimitiveCollectionEditor.CopyValues(source, clone);
            return clone;
        }

        private IList CreateNewItem(Type type)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
			{
				type
			}));
        }

        internal static Type GetListItemType(Type listType)
        {
            Type iListOfT = listType.GetInterfaces().FirstOrDefault((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
            return (iListOfT != null) ? iListOfT.GetGenericArguments()[0] : null;
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

        private IList ComputeItemsSource()
        {
            IList result;
            if (ItemsSource == null)
            {
                result = (ItemsSource = CreateItemsSource());
            }
            else
            {
                result = ItemsSource;
            }
            return result;
        }

        private IList CreateItemsSource()
        {
            IList list = null;
            if (m_listType != null)
            {
                ConstructorInfo constructor = typeof(List<>).MakeGenericType(new Type[]
				{
					typeof(List<>).MakeGenericType(new Type[]
					{
						m_listType
					})
				}).GetConstructor(Type.EmptyTypes);
                list = (IList)constructor.Invoke(null);
            }
            return list;
        }
    }
}