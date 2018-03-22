using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
namespace WorldEditor.Editors.Files.D2O
{
    internal class CollectionEditorResolver : ITypeEditor
    {
        private readonly Window m_dialogOwner;
        private readonly Type m_listType;

        public List<Type> NewTypes { get; set; }

        public CollectionEditorResolver(Window dialogOwner, Type listType)
        {
            m_dialogOwner = dialogOwner;
            m_listType = listType;
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            Button button = new Button();
            button.Content = "(Collection)";
            button.Margin = new Thickness(2.0);
            button.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (m_listType.IsPrimitive || m_listType == typeof(string))
                {
                    DoublePrimitiveCollectionEditor editor = new DoublePrimitiveCollectionEditor(m_listType);
                    EditorDialog dialog = new EditorDialog(editor);
                    dialog.Width = 600.0;
                    dialog.Height = 400.0;
                    Binding binding = new Binding("Value")
                    {
                        Source = propertyItem,
                        Mode = BindingMode.OneWay
                    };
                    BindingOperations.SetBinding(editor, DoublePrimitiveCollectionEditor.ItemsSourceProperty, binding);
                    dialog.ShowDialog();
                }
                else
                {
                    DoubleCollectionEditor editor2 = new DoubleCollectionEditor(m_listType);
                    editor2.NewItemTypes = NewTypes;
                    EditorDialog dialog = new EditorDialog(editor2);
                    dialog.Width = 800.0;
                    dialog.Height = 400.0;
                    Binding binding = new Binding("Value")
                    {
                        Source = propertyItem,
                        Mode = BindingMode.OneWay
                    };
                    BindingOperations.SetBinding(editor2, DoubleCollectionEditor.ItemsSourceProperty, binding);
                    dialog.ShowDialog();
                }
            };
            return button;
        }
    }
}
