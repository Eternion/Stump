using System;
using System.ComponentModel;

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
            }
        }

        public bool IsList
        {
            get;
            set;
        }

        public bool IsI18NProperty
        {
            get;
            set;
        }
        }
}