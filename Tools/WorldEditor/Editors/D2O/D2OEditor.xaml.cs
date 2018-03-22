using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using WorldEditor.Helpers.Reflection;
using WorldEditor.Loaders.Classes;

namespace WorldEditor.Editors.Files.D2O
{
    public partial class D2OEditor : Window
    {
        private Dictionary<Type, List<Type>> m_subTypes = new Dictionary<Type, List<Type>>();

        public D2OEditorModelView ModelView { get; private set; }

        public D2OEditor(string filename)
        {
            InitializeComponent();
            ModelView = new D2OEditorModelView(this, filename);
            base.DataContext = ModelView;
            FindSubClasses();
        }

        private void FindSubClasses()
        {
            Type[] types = typeof(AbuseReasons).Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.HasInterface(typeof(WorldEditor.Loaders.Classes.IDataObject)))
                {
                    if (type.BaseType != typeof(object))
                    {
                        Type baseType = type.BaseType;
                        while (baseType.BaseType != typeof(object))
                        {
                            baseType = baseType.BaseType;
                        }
                        if (!m_subTypes.ContainsKey(baseType))
                        {
                            m_subTypes.Add(baseType, new List<Type>());
                        }
                        m_subTypes[baseType].Add(type);
                    }
                }
            }
        }

        private void ObjectsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveCommand.RaiseCanExecuteChanged();
            ObjectEditor.EditorDefinitions.Clear();
            if (ObjectsGrid.SelectedItem != null)
            {
                Type type = ObjectsGrid.SelectedItem.GetType();
                PropertyInfo[] properties = type.GetProperties();
                PropertyInfo[] array = properties;
                for (int j = 0; j < array.Length; j++)
                {
                    PropertyInfo property = array[j];
                    Type listType = property.PropertyType.GetInterfaces().FirstOrDefault((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
                    if (listType != null)
                    {
                        Type elementType = listType.GetGenericArguments()[0];
                        if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            List<Type> newTypes;
                            m_subTypes.TryGetValue(elementType.GetGenericArguments()[0], out newTypes);
                            newTypes = ((newTypes != null) ? newTypes.ToList<Type>() : new List<Type>());
                            newTypes.Add(elementType.GetGenericArguments()[0]);
                            ObjectEditor.EditorDefinitions.Add(new EditorDefinition
                            {
                                Editor = new CollectionEditorResolver(this, elementType.GetGenericArguments()[0])
                                {
                                    NewTypes = newTypes
                                },
                                TargetType = property.PropertyType
                            });
                        }
                        else
                        {
                            if (!elementType.IsPrimitive && elementType != typeof(string))
                            {
                                List<Type> newTypes;
                                m_subTypes.TryGetValue(elementType, out newTypes);
                                newTypes = ((newTypes != null) ? newTypes.ToList<Type>() : new List<Type>());
                                newTypes.Add(elementType);
                                ObjectEditor.EditorDefinitions.Add(new EditorDefinition
                                {
                                    Editor = new CollectionEditor
                                    {
                                        NewItemsTypes = newTypes
                                    },
                                    TargetType = property.PropertyType
                                });
                            }
                        }
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModelView.FindCommand.RaiseCanExecuteChanged();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ModelView.Dispose();
        }
    }
}
