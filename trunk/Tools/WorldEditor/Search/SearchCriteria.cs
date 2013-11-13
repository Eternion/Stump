using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Stump.Core.Sql;
using WorldEditor.Annotations;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Search
{
    public class SearchCriteria : INotifyPropertyChanged
    {
        private IComparable m_comparedToValue;
        private string m_comparedToValueString;
        public event PropertyChangedEventHandler PropertyChanged;


        public CriteriaOperator Operator
        {
            get;
            set;
        }

        public CriteriaOperator[] AvailableOperators
        {
            get;
            set;
        }

        public string ComparedProperty
        {
            get;
            set;
        }

        public Type ValueType
        {
            get;
            set;
        }

        public string ComparedToValueString
        {
            get { return m_comparedToValueString; }
            set
            {
                m_comparedToValue = (IComparable) Convert.ChangeType(value, ValueType);
                m_comparedToValueString = value;
            }
        }

        public IComparable ComparedToValue
        {
            get { return m_comparedToValue; }
            set
            {
                m_comparedToValue = value;
                m_comparedToValueString = value.ToString();
                OnPropertyChanged("ComparedToValueString");
            }
        }

        public bool IsI18NProperty
        {
            get;
            set;
        }

        public string I18NColumn
        {
            get;
            set;
        }

        public string GetSQLWhereStatement()
        {
            var builder = new StringBuilder();

            if (IsI18NProperty)
            {
                builder.AppendFormat("`{0}` ", I18NColumn);
                builder.AppendFormat("IN (SELECT Id FROM langs WHERE {0} COLLATE UTF8_GENERAL_CI LIKE '%{1}%')",
                    I18NDataManager.Instance.DefaultLanguage, SqlBuilder.EscapeValue(ComparedToValueString));
            }
            else
            {
                builder.AppendFormat("`{0}` ", ComparedProperty);

                if (Operator == CriteriaOperator.CONTAINS)
                {
                    builder.AppendFormat("Contain({0}, {1})", ComparedProperty, GetSQLOperand(ValueType, ComparedToValueString));
                }
                else
                {
                    builder.AppendFormat("{0} {1}", GetSQLOperator(Operator), GetSQLOperand(ValueType, ComparedToValueString));
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
                case CriteriaOperator.GREATER_OR_EQ:
                    return ">=";
                case CriteriaOperator.LESSER:
                    return "<";
                case CriteriaOperator.LESSER_OR_EQ:
                    return "<=";
                default:
                    throw new Exception(string.Format("{0} cannot be converted to string", op));
            }
        }

        private static string GetSQLOperand(Type type, string value)
        {
            if (type == typeof (string))
                return "\"" + value + "\"";
            if (type == typeof (int) || type == typeof (uint) ||
                type == typeof (short) || type == typeof (ushort) ||
                type == typeof (long) || type == typeof (ulong) ||
                type == typeof (decimal) ||
                type == typeof (byte) || type == typeof (sbyte) ||
                type == typeof (float) || type == typeof (double))
                return value;

            if (type == typeof (bool))
                return bool.Parse(value) ? "1" : "0";

            return "\"" + value + "\"";
        }

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}