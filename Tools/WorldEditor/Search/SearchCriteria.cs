using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WorldEditor.Helpers.Sql;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Search
{
    public class SearchCriteria : INotifyPropertyChanged
    {
        private IComparable m_comparedToValue;
        private string m_comparedToValueString;
        public event PropertyChangedEventHandler PropertyChanged;
        private CriteriaOperator _operator;
        private CriteriaOperator[] _avaibleOperators;
        private string _comparedProperty;
        private Type _valueType;
        private bool _isI18NProperty;
        private string _I18NColumn;

        public CriteriaOperator Operator
        {
            get { return _operator; }
            set
            {
                if (_operator == value)
                {
                    return;
                }
                _operator = value;
                OnPropertyChanged("Operator");
            }
        }

        public CriteriaOperator[] AvailableOperators
        {
            get { return _avaibleOperators; }
            set
            {
                _avaibleOperators = value;
                OnPropertyChanged("AvailableOperators");
            }
        }

        public string ComparedProperty
        {
            get { return _comparedProperty; }
            set
            {
                if (string.Equals(_comparedProperty, value, StringComparison.Ordinal))
                {
                    return;
                }
                _comparedProperty = value;
                OnPropertyChanged("ComparedProperty");
            }
        }

        public Type ValueType
        {
            get { return _valueType; }
            set
            {
                if (_valueType == value)
                {
                    return;
                }
                _valueType = value;
                OnPropertyChanged("ValueType");
            }
        }

        public string ComparedToValueString
        {
            get { return m_comparedToValueString; }
            set
            {
                if (string.Equals(m_comparedToValueString, value, StringComparison.Ordinal))
                {
                    return;
                }
                m_comparedToValue = (IComparable)Convert.ChangeType(value, ValueType);
                m_comparedToValueString = value;
                OnPropertyChanged("ComparedToValueString");
            }
        }

        public IComparable ComparedToValue
        {
            get { return m_comparedToValue; }
            set
            {
                if (m_comparedToValue == value)
                {
                    return;
                }
                m_comparedToValue = value;
                OnPropertyChanged("ComparedToValue");
                m_comparedToValueString = value.ToString();
                OnPropertyChanged("ComparedToValueString");
            }
        }

        public bool IsI18NProperty
        {
            get { return _isI18NProperty; }
            set
            {
                if (_isI18NProperty == value)
                {
                    return;
                }
                _isI18NProperty = value;
                OnPropertyChanged("IsI18NProperty");
            }
        }

        public string I18NColumn
        {
            get { return _I18NColumn; }
            set
            {
                if (string.Equals(_I18NColumn, value, StringComparison.Ordinal))
                {
                    return;
                }
                _I18NColumn = value;
                OnPropertyChanged("I18NColumn");
            }
        }

        public string GetSQLWhereStatement()
        {
            StringBuilder builder = new StringBuilder();
            if (IsI18NProperty)
            {
                builder.AppendFormat("`{0}` ", I18NColumn);
                builder.AppendFormat("IN (SELECT Id FROM langs WHERE {0} COLLATE UTF8_GENERAL_CI LIKE '%{1}%')", I18NDataManager.Instance.DefaultLanguage, SqlBuilder.EscapeValue(ComparedToValueString));
            }
            else
            {
                builder.AppendFormat("`{0}` ", ComparedProperty);
                if (Operator == CriteriaOperator.CONTAINS)
                {
                    builder.AppendFormat("Contain({0}, {1})", ComparedProperty, SearchCriteria.GetSQLOperand(ValueType, ComparedToValueString));
                }
                else
                {
                    builder.AppendFormat("{0} {1}", SearchCriteria.GetSQLOperator(Operator), SearchCriteria.GetSQLOperand(ValueType, ComparedToValueString));
                }
            }
            return builder.ToString();
        }

        private static string GetSQLOperator(CriteriaOperator op)
        {
            switch (op)
            {
                case CriteriaOperator.EQ:
                    return "=";

                case CriteriaOperator.DIFFERENT:
                    return "!=";

                case CriteriaOperator.GREATER:
                    return ">";

                case CriteriaOperator.LESSER:
                    return "<";

                case CriteriaOperator.GREATER_OR_EQ:
                    return ">=";

                case CriteriaOperator.LESSER_OR_EQ:
                    return "<=";

                default:
                    throw new Exception(string.Format("{0} cannot be converted to string", op));
            }
        }

        private static string GetSQLOperand(Type type, string value)
        {
            if (type == typeof(string))
            {
                return "\"" + value + "\"";
            }
            else if (type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort) || type == typeof(long) || type == typeof(ulong) || type == typeof(decimal) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(float) || type == typeof(double))
            {
                return value;
            }
            else if (type == typeof(bool))
            {
                return (bool.Parse(value) ? "1" : "0");
            }
            else
            {
                return "\"" + value + "\"";
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}