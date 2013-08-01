using System;
using System.ComponentModel;

namespace WorldEditor.Search
{
    public class SearchCriteria : INotifyPropertyChanged
    {
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

        public IComparable ComparedToValue
        {
            get;
            set;
        }

        public bool IsList
        {
            get;
            set;
        }
    }
}