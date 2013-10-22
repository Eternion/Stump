using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Stump.Core.Reflection;
using Stump.ORM.SubSonic.Extensions;
using WorldEditor.Helpers;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Search
{
    public class SearchDialogModelView : INotifyPropertyChanged
    {
        private static readonly CriteriaOperator[] PrimitiveOperators = new []
        { CriteriaOperator.EQ, CriteriaOperator.DIFFERENT, CriteriaOperator.GREATER, CriteriaOperator.GREATER_OR_EQ, CriteriaOperator.LESSER, CriteriaOperator.LESSER_OR_EQ};

        private static readonly CriteriaOperator[] StringOperators = new[]
            {CriteriaOperator.EQ, CriteriaOperator.DIFFERENT, CriteriaOperator.CONTAINS};

        private static readonly CriteriaOperator[] ListOperators = new[] {CriteriaOperator.CONTAINS};

        private static readonly Dictionary<string, string> m_I18NPropertiesName = new Dictionary<string, string>()
            {
                /*{"NameId", "Name"},
                {"DescriptionId", "Description"},*/
            };

        private ObservableCollection<object> m_itemsSource;
        private readonly ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private readonly ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private readonly Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private readonly Dictionary<string, Type> m_propertiesType = new Dictionary<string, Type>();
        private readonly Dictionary<string, string> m_i18nProperties = new Dictionary<string, string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchDialogModelView(Type itemType, ObservableCollection<object> source)
        {
            ItemType = itemType;
            ItemsSource = source;
            m_readOnlySearchProperties = new ReadOnlyObservableCollection<string>(m_searchProperties);
            Criterias = new ObservableCollection<SearchCriteria>();
            Criterias.CollectionChanged += OnCriteriasCollectionChanged;
        }

        public ObservableCollection<object> ItemsSource
        {
            get { return m_itemsSource; }
            set
            {
                m_itemsSource = value;
                OnItemsSourceChanged();
            }
        }

        public ReadOnlyObservableCollection<string> SearchProperties
        {
            get { return m_readOnlySearchProperties; }
        }

        public IEnumerable<object> Results
        {
            get;
            set;
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

        public string QuickSearchText
        {
            get { return m_quickSearchText; }
            set
            {
                m_quickSearchText = value;

                var idCriteria = Criterias.FirstOrDefault(x => x.ComparedProperty == "Id");
                var nameCriteria = Criterias.FirstOrDefault(x => x.ComparedProperty == "Name");

                if (string.IsNullOrEmpty(m_quickSearchText))
                {
                    OnRemoveCriteria(nameCriteria);
                    OnRemoveCriteria(idCriteria);
                }

                if (m_quickSearchText.IsNumber() && m_searchProperties.Contains("Id"))
                {
                    if (idCriteria == null)
                        Criterias.Add(idCriteria = CreateCriteria("Id"));

                    idCriteria.ComparedToValue = int.Parse(m_quickSearchText);

                    if (nameCriteria != null)
                        OnRemoveCriteria(nameCriteria);
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
                        OnRemoveCriteria(idCriteria);
                }
            }
        }

        #region UpdateResultsCommand

        private DelegateCommand m_updateResultsCommand;

        public DelegateCommand UpdateResultsCommand
        {
            get { return m_updateResultsCommand ?? (m_updateResultsCommand = new DelegateCommand(OnUpdateResults, CanUpdateResults)); }
        }

        private static bool CanUpdateResults(object parameter)
        {
            return true;
        }

        private void OnUpdateResults(object parameter)
        {
            if (!CanUpdateResults(parameter))
                return;

            Results = FindMatches();
        }

        #endregion


        #region AddCriteriaCommand

        private DelegateCommand m_addCriteriaCommand;

        public DelegateCommand AddCriteriaCommand
        {
            get { return m_addCriteriaCommand ?? (m_addCriteriaCommand = new DelegateCommand(OnAddCriteria, CanAddCriteria)); }
        }

        private bool CanAddCriteria(object parameter)
        {
            return true;
        }

        private void OnAddCriteria(object parameter)
        {
            if (!CanAddCriteria(parameter))
                return;

            Criterias.Add(CreateCriteria(m_searchProperties[0]));
        }

        #endregion

        #region RemoveCriteriaCommand

        private DelegateCommand m_removeCriteriaCommand;
        private string m_quickSearchText;

        public DelegateCommand RemoveCriteriaCommand
        {
            get { return m_removeCriteriaCommand ?? (m_removeCriteriaCommand = new DelegateCommand(OnRemoveCriteria, CanRemoveCriteria)); }
        }

        private bool CanRemoveCriteria(object parameter)
        {
            return parameter is SearchCriteria;
        }

        private void OnRemoveCriteria(object parameter)
        {
            if (parameter == null || !CanRemoveCriteria(parameter))
                return;

            var criteria = (SearchCriteria) parameter;

            Criterias.Remove(criteria);
        }

        #endregion


        #region EditItemCommand

        private DelegateCommand m_editItemCommand;

        public DelegateCommand EditItemCommand
        {
            get { return m_editItemCommand ?? (m_editItemCommand = new DelegateCommand(OnEditItem, CanEditItem)); }
        }

        protected virtual bool CanEditItem(object parameter)
        {
            return true;
        }

        protected virtual void OnEditItem(object parameter)
        {
        }


        #endregion

        protected virtual void OnItemsSourceChanged()
        {
            m_searchProperties.Clear();
            m_propertiesGetters.Clear();
            m_propertiesType.Clear();
            m_i18nProperties.Clear();

            var properties = ItemType.GetProperties();

            foreach (var property in properties.Where(property => !m_searchProperties.Contains(property.Name)))
            {
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof (string))
                {
                    var del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object));
                    m_searchProperties.Add(property.Name);
                    m_propertiesGetters.Add(property.Name, del);
                    m_propertiesType.Add(property.Name, property.PropertyType);

                    
                    string textPropertyName;
                    if (IsI18NProperty(property.Name, out textPropertyName))
                    {
                        m_searchProperties.Add(textPropertyName);
                        m_propertiesGetters.Add(textPropertyName, obj => I18NDataManager.Instance.ReadText((uint)del(obj)));
                        m_propertiesType.Add(textPropertyName, typeof(string));
                        m_i18nProperties.Add(textPropertyName, property.Name);
                    }

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
            if (m_I18NPropertiesName.ContainsKey(propertyName))
            {
                textPropertyName = m_I18NPropertiesName[propertyName];
                return true;
            }

            textPropertyName = null;
            return false;
        }

        public virtual SearchCriteria CreateCriteria(string propertyName)
        {
            var criteria = new SearchCriteria
                {
                    ComparedProperty = propertyName,
                };

            UpdateCriteria(criteria);

            criteria.PropertyChanged += OnCriteriaPropertyChanged;

            return criteria;
        }


        public virtual void UpdateCriteria(SearchCriteria searchCriteria)
        {
            if (!m_searchProperties.Contains(searchCriteria.ComparedProperty))
                throw new Exception(string.Format("Property {0} not handled", searchCriteria.ComparedProperty));

            var isI18N = m_i18nProperties.ContainsKey(searchCriteria.ComparedProperty);
            var type =  isI18N ? typeof(string) : ItemType.GetProperty(searchCriteria.ComparedProperty).PropertyType;
            var operators = GetOperators(type);

            if (operators.Length == 0)
                throw new Exception(string.Format("No operators for type {0}", type));

            searchCriteria.IsI18NProperty = isI18N;
            searchCriteria.IsList = type.HasInterface(typeof(IList));
            searchCriteria.AvailableOperators = operators;
            searchCriteria.Operator = operators[0];
            searchCriteria.ValueType = isI18N ? typeof(string) : m_propertiesType[searchCriteria.ComparedProperty];
            searchCriteria.ComparedToValue = isI18N || type == typeof(string) ? string.Empty : (IComparable)Activator.CreateInstance(m_propertiesType[searchCriteria.ComparedProperty]);
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
                foreach(SearchCriteria criteria in e.OldItems)
                    criteria.PropertyChanged -= OnCriteriaPropertyChanged;
            }
        }

        public virtual IEnumerable<object> FindMatches()
        {
            if (Criterias.Count == 0)
                foreach (var item in ItemsSource)
                {
                    yield return item;
                }

            foreach (var item in ItemsSource)
            {
                var match = false;
                foreach (var criteria in Criterias.TakeWhile(criteria => m_searchProperties.Contains(criteria.ComparedProperty)))
                {
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
                        match = ((string) value).IndexOf((string)criteria.ComparedToValue, StringComparison.InvariantCultureIgnoreCase) != -1;
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