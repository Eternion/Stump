using System;
using System.Globalization;
using System.Security.Authentication;
using System.Windows.Data;

namespace WorldEditor.Search
{
    [ValueConversion(typeof(CriteriaOperator), typeof(string))]
    public class OperatorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return value.ToString();

            var op = (CriteriaOperator) value;
            switch (op)
            {
                case CriteriaOperator.EQ:
                    return "==";
                case CriteriaOperator.DIFFERENT:
                    return "!=";
                case CriteriaOperator.GREATER:
                    return ">";
                case CriteriaOperator.GREATER_OR_EQ:
                    return ">=";
                case CriteriaOperator.LESSER:
                    return "<";
                case CriteriaOperator.LESSER_OR_EQ:
                    return "<=";
                case CriteriaOperator.CONTAINS:
                    return "contains";
                default:
                    throw new Exception(string.Format("{0} cannot be converted to string", op));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var op = (string) value;
            switch (op)
            {
                case "==":
                    return CriteriaOperator.EQ;
                case "!=":
                    return CriteriaOperator.DIFFERENT;
                case ">":
                    return CriteriaOperator.GREATER;
                case ">=":
                    return CriteriaOperator.GREATER_OR_EQ;
                case "<":
                    return CriteriaOperator.LESSER;
                case "<=":
                    return CriteriaOperator.LESSER_OR_EQ;
                case "contains":
                    return CriteriaOperator.CONTAINS;
                default:
                    throw new Exception(string.Format("{0} is not a valid operator", op));

            }
        }
    }
}