using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
namespace WorldEditor.Search
{
    [ValueConversion(typeof(CriteriaOperator), typeof(string))]
    public class OperatorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return value.ToString();
            }
            else
            {
                CriteriaOperator op = (CriteriaOperator)value;
                switch (op)
                {
                    case CriteriaOperator.EQ:
                        return  "==";

                    case CriteriaOperator.DIFFERENT:
                        return  "!=";

                    case CriteriaOperator.GREATER:
                        return  ">";

                    case CriteriaOperator.LESSER:
                        return  "<";

                    case CriteriaOperator.GREATER_OR_EQ:
                        return  ">=";

                    case CriteriaOperator.LESSER_OR_EQ:
                        return  "<=";

                    case CriteriaOperator.CONTAINS:
                        return  "contains";

                    default:
                        throw new Exception(string.Format("{0} cannot be converted to string", op));
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string op = (string)value;
            if (op == null)
            {
                return null;
            }

            var test = new Dictionary<string, int>(7)
					{
						{ "==", 0 },
						{ "!=", 1 },
						{ ">", 2 },
						{ ">=", 3 },
						{ "<", 4 },
						{ "<=", 5 },                  
                        { "contains", 6 }
					};

            int num;
            if (test.TryGetValue(op, out num))
            {
                switch (num)
                {
                    case 0:
                        return CriteriaOperator.EQ;

                    case 1:
                        return CriteriaOperator.DIFFERENT;

                    case 2:
                        return CriteriaOperator.GREATER;

                    case 3:
                        return CriteriaOperator.GREATER_OR_EQ;

                    case 4:
                        return CriteriaOperator.LESSER;

                    case 5:
                        return CriteriaOperator.LESSER_OR_EQ;

                    case 6:
                        return CriteriaOperator.CONTAINS;
                }
            }
            throw new Exception(string.Format("{0} is not a valid operator", op));
        }
    }
}
