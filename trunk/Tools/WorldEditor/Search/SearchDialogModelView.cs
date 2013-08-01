using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Stump.Core.Reflection;

namespace WorldEditor.Search
{
    public class SearchDialogModelView : INotifyPropertyChanged
    {
        private static readonly CriteriaOperator[] PrimitiveOperators = new []
        { CriteriaOperator.EQ, CriteriaOperator.DIFFERENT, CriteriaOperator.GREATER, CriteriaOperator.GREATER_OR_EQ, CriteriaOperator.LESSER, CriteriaOperator.LESSER_OR_EQ};

        private static readonly CriteriaOperator[] StringOperators = new[]
            {CriteriaOperator.EQ, CriteriaOperator.DIFFERENT, CriteriaOperator.CONTAINS};

        private static readonly CriteriaOperator[] ListOperators = new[] {CriteriaOperator.CONTAINS};

        private ObservableCollection<object> m_itemsSource;
        private ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private Dictionary<string, Type> m_propertiesType = new Dictionary<string, Type>();
        private Dictionary<string, string> m_i18nProperties = new Dictionary<string, string>(); 

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchDialogModelView(Type itemType, ObservableCollection<object> source)
        {
            ItemType = itemType;
            ItemsSource = source;
            m_readOnlySearchProperties = new ReadOnlyObservableCollection<string>(m_searchProperties);
            Criterias = new ObservableCollection<SearchCriteria>();
        }

        public ObservableCollection<object> ItemsSource
        {
            get { return m_itemsSource; }
            set { m_itemsSource = value;
                OnItemsSourceChanged();
            }
        }

        public ReadOnlyObservableCollection<string> SearchProperties
        {
            get { return m_readOnlySearchProperties; }
        }
        

        public Type ItemType
        {
            get;
            private set;
        }

        public ObservableCollection<SearchCriteria> Criterias
        {
            get;
            set;
        }

        protected virtual void OnItemsSourceChanged()
        {
            m_searchProperties.Clear();
            m_propertiesGetters.Clear();
            m_propertiesType.Clear();

            var properties = ItemType.GetProperties();

            foreach (var property in properties)
            {
                if (m_searchProperties.Contains(property.Name))
                    continue;

                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof (string))
                {
                    string textPropertyName;
                    if (IsI18NProperty(property.Name, out textPropertyName))
                    {
                        m_searchProperties.Add(textPropertyName);
                        m_propertiesGetters.Add(textPropertyName, (obj) => "...");
                        m_propertiesType.Add(textPropertyName, typeof(string));
                    }

                    var del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object));
                    m_searchProperties.Add(property.Name);
                    m_propertiesGetters.Add(property.Name, del);
                    m_propertiesType.Add(property.Name, property.PropertyType);
                }
                else if (property.PropertyType.HasInterface(typeof (IList)) && property.PropertyType.IsGenericType)
                {
                    var args = property.PropertyType.GetGenericArguments();
                    if (args.Length == 1 && args[0].IsPrimitive || args[0] == typeof (string))
                    {
                        var del = (Func<object, object>) property.GetGetMethod().CreateFuncDelegate(typeof (object));
                        m_searchProperties.Add(property.Name);
                        m_propertiesGetters.Add(property.Name, del);
                        m_propertiesType.Add(property.Name, args[0]);

                    }
                }
            }
        }

        protected virtual bool IsI18NProperty(string propertyName, out string textPropertyName)
        {
            if (propertyName == "NameId")
            {
                textPropertyName = "Name";
                return true;
            }
            else if (propertyName == "DescriptionId")
            {
                textPropertyName = "Description";
                return true;
            }

            textPropertyName = null;
            return false;
        }

        public virtual SearchCriteria CreateCriteria(string propertyName)
        {
            if (!m_searchProperties.Contains(propertyName))
                throw new Exception(string.Format("Property {0} not handled", propertyName));

            var type = ItemType.GetProperty(propertyName).PropertyType;
            var operators = GetOperators(type);

            if (operators.Length == 0)
                throw new Exception(string.Format("No operators for type {0}", type));

            var criteria = new SearchCriteria()
                {
                    ComparedProperty = propertyName,
                    IsList = type.HasInterface(typeof(IList)),
                    AvailableOperators = operators,
                    Operator = operators[0],
                    ValueType = m_propertiesType[propertyName],
                    ComparedToValue = (IComparable)Activator.CreateInstance(m_propertiesType[propertyName])
                };

            return criteria;
        }

        public virtual IEnumerable<object> FindMatches()
        {
            if (Criterias.Count == 0)
                yield return ItemsSource;

            foreach (var item in ItemsSource)
            {
                bool match = false;
                foreach (var criteria in Criterias)
                {
                    if (!m_searchProperties.Contains(criteria.ComparedProperty))
                        break;

                    Func<object, object> propGetter;
                    if (!m_propertiesGetters.TryGetValue(criteria.ComparedProperty, out propGetter))
                        break;

                    
                    var value = propGetter(item);
                    if (criteria.Operator != CriteriaOperator.CONTAINS)
                    {
                        match = Match(((IComparable)value).CompareTo(criteria.ComparedToValue), criteria.Operator);
                    }
                    else if (criteria.Operator == CriteriaOperator.CONTAINS && !criteria.IsList) // is string
                    {
                        match = ((string) value).Contains((string)criteria.ComparedToValue);
                    }
                    else if (criteria.Operator == CriteriaOperator.CONTAINS && criteria.IsList)
                    {
                        match = ((IList) value).Contains(criteria.ComparedToValue);
                    }

                    if (!match)
                        break;
                }

                if (match)
                    yield return item;
            }
        }

        protected bool Match(int comparaison, CriteriaOperator criteriaOperator)
        {
            switch (criteriaOperator)
            {
                case CriteriaOperator.EQ:
                    return comparaison == 0;
                case CriteriaOperator.DIFFERENT:
                    return comparaison != 0;
                case CriteriaOperator.GREATER:
                    return comparaison > 0;
                case CriteriaOperator.GREATER_OR_EQ:
                    return comparaison >= 0;
                case CriteriaOperator.LESSER:
                    return comparaison < 0;
                case CriteriaOperator.LESSER_OR_EQ:
                    return comparaison <= 0;
                default:
                    return false;
            }
        }

        public static CriteriaOperator[] GetOperators(Type type)
        {
            if (type.IsPrimitive)
                return PrimitiveOperators;
            if (type == typeof (string))
                return StringOperators;

            if (type.HasInterface(typeof (IList)) && type.IsGenericType)
            {
                var args = type.GetGenericArguments();
                if (args.Length == 1 && args[0].IsPrimitive || args[0] == typeof (string))
                    return ListOperators;
            }

            return new CriteriaOperator[0];
        }
    }
}