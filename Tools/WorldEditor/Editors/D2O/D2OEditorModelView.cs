using Newtonsoft.Json;
using WorldEditor.Loaders.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Xml;
using WorldEditor.Helpers;
using WorldEditor.Helpers.Converters;
using WorldEditor.Loaders.Data;
using WorldEditor.Helpers.Reflection;

namespace WorldEditor.Editors.Files.D2O
{
    public class D2OEditorModelView
    {
        private ReadOnlyObservableCollection<object> m_readOnylRows;
        private ObservableCollection<object> m_rows = new ObservableCollection<object>();
        private ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private D2OEditor m_editor;
        private readonly string m_filePath;
        private D2OReader m_reader;
        private Type[] m_distinctTypes;
        private Stack<EditedObject> m_editedObjects = new Stack<EditedObject>();
        private List<DataGridColumn> m_columns = new List<DataGridColumn>();
        private DelegateCommand m_addCommand;
        private DelegateCommand m_removeCommand;
        private DelegateCommand m_convertCommand;
        private DelegateCommand m_saveCommand;
        private DelegateCommand m_saveAsCommand;
        private DelegateCommand m_findCommand;
        private DelegateCommand m_findNextCommand;

        public ReadOnlyObservableCollection<object> Rows
        {
            get { return m_readOnylRows; }
        }

        public ReadOnlyCollection<DataGridColumn> Columns
        {
            get { return m_columns.AsReadOnly(); }
        }

        public ReadOnlyObservableCollection<string> SearchProperties
        {
            get { return m_readOnlySearchProperties; }
        }

        public List<Type> NewObjectTypes { get; private set; }

        public DelegateCommand AddCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_addCommand) == null)
                {
                    arg_31_0 = (m_addCommand = new DelegateCommand(new Action<object>(OnAdd), (CanAdd)));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand RemoveCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_removeCommand) == null)
                {
                    arg_31_0 = (m_removeCommand = new DelegateCommand(new Action<object>(OnRemove), (CanRemove)));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand ConvertCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_convertCommand) == null)
                {
                    arg_31_0 = (m_convertCommand = new DelegateCommand(new Action<object>(OnConvert), (CanConvert)));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand SaveCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_saveCommand) == null)
                {
                    arg_31_0 = (m_saveCommand = new DelegateCommand(new Action<object>(OnSave), (D2OEditorModelView.CanSave)));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand SaveAsCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_saveAsCommand) == null)
                {
                    arg_31_0 = (m_saveAsCommand = new DelegateCommand(new Action<object>(OnSaveAs), (CanSaveAs)));
                }
                return arg_31_0;
            }
        }

        public int LastFoundIndex { get; set; }

        public string SearchText { get; set; }

        public string SearchProperty { get; set; }

        public DelegateCommand FindCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_findCommand) == null)
                {
                    arg_31_0 = (m_findCommand = new DelegateCommand(new Action<object>(OnFind), (CanFind)));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand FindNextCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_findNextCommand) == null)
                {
                    arg_31_0 = (m_findNextCommand = new DelegateCommand(new Action<object>(OnFindNext), (CanFindNext)));
                }
                return arg_31_0;
            }
        }

        public D2OEditorModelView(D2OEditor editor, string filePath)
        {
            m_editor = editor;
            m_filePath = filePath;
            m_readOnylRows = new ReadOnlyObservableCollection<object>(m_rows);
            m_readOnlySearchProperties = new ReadOnlyObservableCollection<string>(m_searchProperties);
            NewObjectTypes = new List<Type>();
            Open();
        }
        private void Open()
        {
            m_reader = new D2OReader(m_filePath);
            foreach (object dataObject in m_reader.EnumerateObjects(false))
            {
                m_rows.Add(dataObject);
            }
            m_distinctTypes = (
                from x in m_rows
                select x.GetType()).Distinct<Type>().ToArray<Type>();
            IEnumerable<PropertyInfo> properties = m_distinctTypes.SelectMany((Type x) => x.GetProperties()).Distinct<PropertyInfo>();
            IEnumerable<D2OFieldDefinition> d2oProperties = m_reader.Classes.SelectMany((KeyValuePair<int, D2OClassDefinition> x) => x.Value.Fields.Values).Distinct<D2OFieldDefinition>();
            using (IEnumerator<PropertyInfo> enumerator2 = properties.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    PropertyInfo property = enumerator2.Current;
                    D2OFieldDefinition d2oProperty = d2oProperties.FirstOrDefault((D2OFieldDefinition x) => x.Name.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (!m_searchProperties.Contains(property.Name))
                    {
                        if (property.PropertyType.IsPrimitive || !(property.PropertyType != typeof(string)))
                        {
                            Func<object, object> del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object), new Type[0]);
                            m_searchProperties.Add(property.Name);
                            m_propertiesGetters.Add(property.Name, del);
                            System.Windows.Data.Binding binding = new System.Windows.Data.Binding(property.Name);
                            DataGridTemplateColumn column = new DataGridTemplateColumn();
                            FrameworkElementFactory element;
                            if (property.PropertyType == typeof(bool))
                            {
                                element = new FrameworkElementFactory(typeof(System.Windows.Controls.CheckBox));
                                element.SetBinding(ToggleButton.IsCheckedProperty, binding);
                                element.SetValue(FrameworkElement.MarginProperty, new Thickness(1.0));
                                element.SetValue(FrameworkElement.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Center);
                                element.SetValue(UIElement.IsEnabledProperty, false);
                            }
                            else
                            {
                                element = new FrameworkElementFactory(typeof(TextBlock));
                                if (d2oProperty != null && d2oProperty.TypeId == D2OFieldType.I18N)
                                {
                                    binding.Converter = new IdToI18NTextConverter();
                                    column.Width = 120.0;
                                }
                                element.SetBinding(TextBlock.TextProperty, binding);
                                element.SetValue(FrameworkElement.MarginProperty, new Thickness(1.0));
                            }
                            column.CellTemplateSelector = new CellTemplateSelector
                            {
                                Template = new DataTemplate(property.PropertyType)
                                {
                                    VisualTree = element
                                },
                                DefaultTemplate = new DataTemplate(),
                                ExpectedType = property.ReflectedType
                            };
                            column.Header = property.Name;
                            m_columns.Add(column);
                            m_editor.ObjectsGrid.Columns.Add(column);
                        }
                    }
                }
            }
            NewObjectTypes.AddRange(
                from x in m_distinctTypes
                where x.GetConstructor(Type.EmptyTypes) != null
                select x);
        }

        private bool CanAdd(object parameter)
        {
            return parameter is Type && (parameter as Type).GetConstructor(Type.EmptyTypes) != null;
        }

        private void OnAdd(object parameter)
        {
            if (parameter != null && CanAdd(parameter))
            {
                object obj = Activator.CreateInstance((Type)parameter);
                PropertyInfo[] properties = obj.GetType().GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    }
                }
                m_rows.Add(obj);
                m_editor.ObjectsGrid.SelectedItem = obj;
                m_editor.ObjectsGrid.ScrollIntoView(obj);
                m_editor.ObjectsGrid.Focus();
                EditedObject editedObject = new EditedObject(obj, ObjectState.Added);
                m_editedObjects.Push(editedObject);
            }
        }

        private bool CanRemove(object parameter)
        {
            return parameter is IList && ((IList)parameter).Count > 0;
        }

        private void OnRemove(object parameter)
        {
            if (parameter != null && CanRemove(parameter))
            {
                object[] list = (parameter as IList).OfType<object>().ToArray<object>();
                object[] array = list;
                for (int i = 0; i < array.Length; i++)
                {
                    object item = array[i];
                    m_rows.Remove(item);
                    m_editedObjects.Push(new EditedObject(item, ObjectState.Removed));
                }
            }
        }

        private bool CanConvert(object parameter)
        {
            return true;
        }

        private void OnConvert(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(m_reader.FilePath) + ".xml",
                InitialDirectory = Path.GetDirectoryName(m_reader.FilePath),
                Title = "Convert to ...",
                DefaultExt = ".xml",
                Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                string json = JsonConvert.SerializeObject(Rows.ToArray<object>(), new JsonSerializerSettings
                {
                    ContractResolver = SerializePropertyOnlyResolver.Instance
                });
                string rootJson = "{\"Object\":" + json + "}";
                XmlDocument document = JsonConvert.DeserializeXmlNode(rootJson, "root");
                document.Save(filePath);
                MessageService.ShowMessage(m_editor, string.Format("File converted to {0}", Path.GetFileName(filePath)));
            }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            m_reader.Close();
            string filePath = m_reader.FilePath;
            PerformSave(filePath);
            m_reader = new D2OReader(filePath);
        }

        private bool CanSaveAs(object parameter)
        {
            return true;
        }

        private void OnSaveAs(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = m_reader.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2i",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                PerformSave(filePath);
            }
        }

        private void PerformSave(string filePath)
        {
            D2OWriter writer = null;
            try
            {
                writer = new D2OWriter(filePath, true);
                writer.StartWriting(true);
                foreach (object row in Rows)
                {
                    if (row is IIndexedData)
                    {
                        writer.Write(row, ((IIndexedData)row).Id);
                    }
                    else
                    {
                        writer.Write(row);
                    }
                }
                writer.EndWriting();
                MessageService.ShowMessage(m_editor, "File saved successfully");
            }
            catch (IOException ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex.Message);
            }
            catch (Exception ex2)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex2);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        private object FindNext()
        {
            int startIndex = (LastFoundIndex == -1 || LastFoundIndex + 1 >= Rows.Count) ? 0 : (LastFoundIndex + 1);
            object row = null;
            int index = -1;
            object result;
            if (string.IsNullOrEmpty(SearchProperty))
            {
                result = null;
            }
            else
            {
                if (m_rows.Count == 0)
                {
                    result = null;
                }
                else
                {
                    Func<object, object> getter = m_propertiesGetters[SearchProperty];
                    Type propertyType = getter(m_rows[0]).GetType();
                    bool isBool = propertyType == typeof(bool);
                    bool isInteger = propertyType == typeof(int) || propertyType == typeof(uint) || propertyType == typeof(short) || propertyType == typeof(ushort);
                    bool isLong = propertyType == typeof(long) || propertyType == typeof(ulong);
                    bool isDouble = propertyType == typeof(double) || propertyType == typeof(float);
                    int? searchInteger = null;
                    int dummy;
                    if (int.TryParse(SearchText, out dummy))
                    {
                        searchInteger = new int?(dummy);
                    }
                    long? searchLong = null;
                    long dummyL;
                    if (long.TryParse(SearchText, out dummyL))
                    {
                        searchLong = new long?((long)dummy);
                    }
                    double? searchDouble = null;
                    double dummyD;
                    if (double.TryParse(SearchText, out dummyD))
                    {
                        searchDouble = new double?(dummyD);
                    }
                    bool? searchBool = (SearchText.ToLower() == "true" || SearchText.ToLower() == "false") ? new bool?(SearchText.ToLower() == "true") : null;
                    for (int i = startIndex; i < m_rows.Count; i++)
                    {
                        object value = getter(m_rows[i]);
                        if (isBool)
                        {
                            if ((searchBool.HasValue && searchBool == (bool)value) || (searchInteger.HasValue && (bool)value == (searchInteger.Value != 0)))
                            {
                                row = m_rows[i];
                                index = i;
                                break;
                            }
                        }
                        else
                        {
                            if (isInteger)
                            {
                                if (searchInteger.HasValue && searchInteger == Convert.ToInt32(value))
                                {
                                    row = m_rows[i];
                                    index = i;
                                    break;
                                }
                            }
                            else
                            {
                                if (isLong)
                                {
                                    if (searchLong.HasValue && searchLong == Convert.ToInt64(value))
                                    {
                                        row = m_rows[i];
                                        index = i;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (isDouble)
                                    {
                                        bool arg_3C6_0;
                                        if (searchDouble.HasValue)
                                        {
                                            double? num = searchDouble;
                                            double num2 = Convert.ToDouble(value);
                                            arg_3C6_0 = (num.GetValueOrDefault() != num2 || !num.HasValue);
                                        }
                                        else
                                        {
                                            arg_3C6_0 = true;
                                        }
                                        if (!arg_3C6_0)
                                        {
                                            row = m_rows[i];
                                            index = i;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (value.ToString().IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                                        {
                                            row = m_rows[i];
                                            index = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (row == null)
                    {
                        LastFoundIndex = -1;
                        result = null;
                    }
                    else
                    {
                        LastFoundIndex = index;
                        result = row;
                    }
                }
            }
            return result;
        }

        private bool CanFind(object parameter)
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        private void OnFind(object parameter)
        {
            if (CanFind(parameter))
            {
                LastFoundIndex = 0;
                object row = FindNext();
                FindNextCommand.RaiseCanExecuteChanged();
                if (row != null)
                {
                    m_editor.ObjectsGrid.SelectedItem = row;
                    m_editor.ObjectsGrid.ScrollIntoView(row);
                    m_editor.ObjectsGrid.Focus();
                }
                else
                {
                    MessageService.ShowMessage(m_editor, "Not found");
                }
            }
        }

        private bool CanFindNext(object parameter)
        {
            return true;
        }

        private void OnFindNext(object parameter)
        {
            object row = FindNext();
            if (row == null)
            {
                row = FindNext();
            }
            if (row != null)
            {
                m_editor.ObjectsGrid.SelectedItem = row;
                m_editor.ObjectsGrid.ScrollIntoView(row);
                m_editor.ObjectsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        public void Dispose()
        {
            m_reader.Close();
        }
    }
}
