using WorldEditor.Loaders.Data;
using Stump.ORM.SubSonic.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WorldEditor.Helpers;
using WorldEditor.Helpers.Reflection;

namespace WorldEditor.Search
{
    public abstract class SearchDialogModelView<T> : INotifyPropertyChanged
    {
        private readonly Type m_searchType;
        private readonly ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private readonly ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private readonly Dictionary<string, Type> m_propertiesType = new Dictionary<string, Type>();
        private readonly Dictionary<string, string> m_i18nProperties = new Dictionary<string, string>();
        private string m_quickSearchText;
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<T> _results;
        private ObservableCollection<SearchCriteria> _criteras;

        public ReadOnlyObservableCollection<string> SearchProperties
        {
            get { return m_readOnlySearchProperties; }
        }

        public IEnumerable<T> Results
        {
            get { return _results; }
            set
            {
                if (_results == value)
                {
                    return;
                }
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        public ObservableCollection<SearchCriteria> Criterias
        {
            get { return _criteras; }
            set
            {
                if (_criteras == value)
                {
                    return;
                }
                _criteras = value;
                OnPropertyChanged("Criterias");
            }
        }

        public string QuickSearchText
        {
            get { return m_quickSearchText; }
            set
            {
                if (string.Equals(QuickSearchText, value, StringComparison.Ordinal))
                {
                    return;
                }
                m_quickSearchText = value;
                var idCriteria = Criterias.FirstOrDefault(entry => entry.ComparedProperty == "Id");
                var nameCriteria = Criterias.FirstOrDefault(entry => entry.ComparedProperty == "Name");
                if (string.IsNullOrEmpty(m_quickSearchText))
                {
                    OnRemoveCriteria(nameCriteria);
                    OnRemoveCriteria(idCriteria);
                }
                if (m_quickSearchText.IsNumber() && m_searchProperties.Contains("Id"))
                {
                    if (idCriteria == null)
                    {
                        Criterias.Add(idCriteria = CreateCriteria("Id"));
                    }
                    idCriteria.ComparedToValue = int.Parse(m_quickSearchText);
                    if (nameCriteria != null)
                    {
                        OnRemoveCriteria(nameCriteria);
                    }
                }
                else if (m_searchProperties.Contains("Name"))
                {
                    if (nameCriteria == null)
                    {
                        nameCriteria = CreateCriteria("Name");
                        nameCriteria.Operator = CriteriaOperator.CONTAINS;
                        Criterias.Add(nameCriteria);
                    }
                    nameCriteria.ComparedToValue = m_quickSearchText;
                    if (idCriteria != null)
                    {
                        OnRemoveCriteria(idCriteria);
                    }
                }
                OnPropertyChanged("QuickSearchText");
            }
        }

        public DelegateCommand UpdateResultsCommand { get; private set; }

        public DelegateCommand AddCriteriaCommand { get; private set; }

        public DelegateCommand RemoveCriteriaCommand { get; private set; }

        public DelegateCommand EditItemCommand { get; private set; }

        public SearchDialogModelView(Type searchType)
        {
            UpdateResultsCommand = new DelegateCommand(OnUpdateResults, CanUpdateResults);
            AddCriteriaCommand = new DelegateCommand(OnAddCriteria, CanAddCriteria);
            RemoveCriteriaCommand = new DelegateCommand(OnRemoveCriteria, CanRemoveCriteria);
            EditItemCommand = new DelegateCommand(OnEditItem, CanEditItem);

            m_searchType = searchType;
            m_readOnlySearchProperties = new ReadOnlyObservableCollection<string>(m_searchProperties);
            Criterias = new ObservableCollection<SearchCriteria>();
            Criterias.CollectionChanged += OnCriteriasCollectionChanged;
            LoadAvailableCriterias();
        }

        private static bool CanUpdateResults(object parameter)
        {
            return true;
        }

        private void OnUpdateResults(object parameter)
        {
            if (CanUpdateResults(parameter))
            {
                Results = FindMatches();
            }
        }

        private bool CanAddCriteria(object parameter)
        {
            return true;
        }

        private void OnAddCriteria(object parameter)
        {
            if (CanAddCriteria(parameter))
            {
                Criterias.Add(CreateCriteria(m_searchProperties[0]));
            }
        }

        private bool CanRemoveCriteria(object parameter)
        {
            return parameter is SearchCriteria;
        }

        private void OnRemoveCriteria(object parameter)
        {
            if (parameter != null && CanRemoveCriteria(parameter))
            {
                var criteria = (SearchCriteria)parameter;
                Criterias.Remove(criteria);
            }
        }

        protected virtual bool CanEditItem(object parameter)
        {
            return true;
        }

        protected virtual void OnEditItem(object parameter)
        {
        }

        protected void LoadAvailableCriterias()
        {
            m_searchProperties.Clear();
            m_propertiesType.Clear();
            m_i18nProperties.Clear();

            foreach (var property in m_searchType.GetProperties().Where(entry => !m_searchProperties.Contains(entry.Name)))
            {
                if (property.PropertyType != typeof(byte[]))
                {
                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                    {
                        var del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object));
                        m_searchProperties.Add(property.Name);
                        m_propertiesType.Add(property.Name, property.PropertyType);
                        if (property.GetCustomAttribute<I18NFieldAttribute>() != null)
                        {
                            string textPropertyName = property.Name.Replace("Id", "");
                            m_searchProperties.Add(textPropertyName);
                            m_propertiesType.Add(textPropertyName, typeof(string));
                            m_i18nProperties.Add(textPropertyName, property.Name);
                        }
                    }
                }
            }
        }

        public virtual SearchCriteria CreateCriteria(string propertyName)
        {
            var criteria = new SearchCriteria
            {
                ComparedProperty = propertyName
            };
            UpdateCriteria(criteria);
            criteria.PropertyChanged += OnCriteriaPropertyChanged;
            return criteria;
        }

        public virtual void UpdateCriteria(SearchCriteria searchCriteria)
        {
            if (!m_searchProperties.Contains(searchCriteria.ComparedProperty))
            {
                throw new Exception(string.Format("Property {0} not handled", searchCriteria.ComparedProperty));
            }
            PropertyInfo property = m_searchType.GetProperty(searchCriteria.ComparedProperty);
            bool isI18N = m_i18nProperties.ContainsKey(searchCriteria.ComparedProperty);
            Type type = isI18N ? typeof(string) : property.PropertyType;
            CriteriaOperator[] operators = SearchDialogModelView<T>.GetOperators(type);
            if (operators.Length == 0)
            {
                throw new Exception(string.Format("No operators for type {0}", type));
            }
            searchCriteria.IsI18NProperty = isI18N;
            if (isI18N)
            {
                searchCriteria.I18NColumn = m_i18nProperties[searchCriteria.ComparedProperty];
            }
            searchCriteria.AvailableOperators = operators;
            searchCriteria.Operator = operators[0];
            searchCriteria.ValueType = (isI18N ? typeof(string) : m_propertiesType[searchCriteria.ComparedProperty]);
            if (searchCriteria.ComparedToValue != null && searchCriteria.ValueType.IsInstanceOfType(searchCriteria.ComparedToValue))
            {
                searchCriteria.ComparedToValueString = searchCriteria.ComparedToValue.ToString();
            }
            else
            {
                searchCriteria.ComparedToValue = ((isI18N || type == typeof(string)) ? string.Empty : ((IComparable)Activator.CreateInstance(m_propertiesType[searchCriteria.ComparedProperty])));
            }
        }

        private void OnCriteriaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ComparedProperty")
            {
                UpdateCriteria((SearchCriteria)sender);
            }
        }

        private void OnCriteriasCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var criteria in e.OldItems.Cast<SearchCriteria>())
                {
                    criteria.PropertyChanged -= OnCriteriaPropertyChanged;
                }
            }
        }

        protected string GetSQLWhereStatement()
        {
            return string.Join(" AND ", Criterias.Select(entry => entry.GetSQLWhereStatement()));
        }

        public abstract IEnumerable<T> FindMatches();

        public static CriteriaOperator[] GetOperators(Type type)
        {
            CriteriaOperator[] result;
            if (type.IsPrimitive)
            {
                result = PrimitiveOperators;
            }
            else
            {
                if (type == typeof(string))
                {
                    result = StringOperators;
                }
                else
                {
                    if (type.HasInterface(typeof(IList)) && type.IsGenericType)
                    {
                        Type[] args = type.GetGenericArguments();
                        if ((args.Length == 1 && args[0].IsPrimitive) || args[0] == typeof(string))
                        {
                            result = ListOperators;
                            return result;
                        }
                    }
                    result = new CriteriaOperator[0];
                }
            }
            return result;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Shitty big static array
        private static readonly CriteriaOperator[] PrimitiveOperators = new CriteriaOperator[]
		{
			CriteriaOperator.EQ,
			CriteriaOperator.DIFFERENT,
			CriteriaOperator.GREATER,
			CriteriaOperator.GREATER_OR_EQ,
			CriteriaOperator.LESSER,
			CriteriaOperator.LESSER_OR_EQ
		};

        private static readonly CriteriaOperator[] StringOperators = new CriteriaOperator[]
		{
			CriteriaOperator.EQ,
			CriteriaOperator.DIFFERENT,
			CriteriaOperator.CONTAINS
		};

        private static readonly CriteriaOperator[] ListOperators = new CriteriaOperator[]
		{
			CriteriaOperator.CONTAINS
		};

        #endregion
    }
}