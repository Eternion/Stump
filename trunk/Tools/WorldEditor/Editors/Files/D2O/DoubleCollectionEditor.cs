#region License GNU GPL

// DoubleCollectionEditor.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorldEditor.Editors.Files.D2O
{
    public class DoubleCollectionEditor : Control, IPersistableChanged
    {
        #region Properties

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof (ObservableCollection<IList>), typeof (DoubleCollectionEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof (IList), typeof (DoubleCollectionEditor),
                                        new UIPropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedSubListProperty =
            DependencyProperty.Register("SelectedSubList", typeof(ObservableCollection<object>), typeof(DoubleCollectionEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectedSubListSourceProperty =
            DependencyProperty.Register("SelectedSubListSource", typeof (IList), typeof (DoubleCollectionEditor),
                                        new UIPropertyMetadata(null, OnSubListSourceChanged));

        public static readonly DependencyProperty NewItemTypesProperty =
            DependencyProperty.Register("NewItemTypes", typeof (IList), typeof (DoubleCollectionEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (object), typeof (DoubleCollectionEditor),
                                        new PropertyMetadata(default(object)));

        private Type m_listType;


        public IList<Type> NewItemTypes
        {
            get { return (IList<Type>) GetValue(NewItemTypesProperty); }
            set { SetValue(NewItemTypesProperty, value); }
        }

        public Type SubListType
        {
            get { return m_listType != null ? typeof (List<>).MakeGenericType(m_listType) : null; }
        }

        public ObservableCollection<IList> Items
        {
            get { return (ObservableCollection<IList>) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ObservableCollection<object> SelectedSubList
        {
            get
            {
                return (ObservableCollection<object>)GetValue(SelectedSubListProperty);
            }
            set
            {
                SetValue(SelectedSubListProperty, value);
            }
        }

        public IList SelectedSubListSource
        {
            get
            {
                return (IList)GetValue(SelectedSubListSourceProperty);
            }
            set
            {
                SetValue(SelectedSubListSourceProperty, value);
            }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var CollectionControl = (DoubleCollectionEditor) d;
            if (CollectionControl != null)
                CollectionControl.OnItemSourceChanged((IList) e.OldValue, (IList) e.NewValue);
        }

        public void OnItemSourceChanged(IList oldValue, IList newValue)
        {
            m_listType = null;


            Type oldType = oldValue != null ? oldValue.GetType() : null;
            var oldListType = oldType != null ? oldType.GetGenericArguments()[0].GetGenericArguments()[0] : null;

            if (newValue != null)
            {
                Items.Clear();
                foreach (IList item in newValue)
                    Items.Add((IList) CreateClone(item));

                Type type = newValue.GetType();
                m_listType = type.GetGenericArguments()[0].GetGenericArguments()[0];

                if (!NewItemTypes.Contains(m_listType))
                    NewItemTypes.Insert(0, m_listType);

                if (oldListType != null && m_listType != oldListType && !m_listType.IsSubclassOf(oldListType))
                    NewItemTypes.Remove(oldListType);
            }
        }


        private static void OnSubListSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var CollectionControl = (DoubleCollectionEditor)d;
            if (CollectionControl != null)
                CollectionControl.OnSubListSourceChanged((IList)e.OldValue, (IList)e.NewValue);
        }

        private void OnSubListSourceChanged(IList oldValue, IList newValue)
        {
            SelectedSubList.CollectionChanged -= OnSelectedSubListChanged;
            SelectedSubList.Clear();
            foreach (var obj in newValue)
            {
                SelectedSubList.Add(obj);
            }
            SelectedSubList.CollectionChanged += OnSelectedSubListChanged;
        }

        private void OnSelectedSubListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    var item = e.NewItems[i];
                    SelectedSubListSource.Insert(e.NewStartingIndex + i, item);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    SelectedSubListSource.Remove(item);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
                SelectedSubListSource.Clear();
        }


        #endregion //Properties

        #region Constructors

        static DoubleCollectionEditor()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("/WorldEditor;component/D2O/Template.xaml", UriKind.Relative)
                });
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DoubleCollectionEditor),
                                                     new FrameworkPropertyMetadata(typeof (DoubleCollectionEditor)));
        }

        public DoubleCollectionEditor(Type listType)
        {
            m_listType = listType;
            Items = new ObservableCollection<IList>();
            SelectedSubList = new ObservableCollection<object>();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, AddNew, CanAddNew));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete, CanDelete));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, MoveDown, CanMoveDown));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, MoveUp, CanMoveUp));
        }

        #endregion //Constructors

        #region Commands

        private void AddNew(object sender, ExecutedRoutedEventArgs e)
        {
            var list = (IList) ((FrameworkElement)e.OriginalSource).Tag;

            var newItem = list == Items ? CreateNewItem(typeof(List<>).MakeGenericType((Type)e.Parameter)) : CreateNewItem((Type)e.Parameter);
            list.Add(newItem);

            if (list == Items)
                SelectedSubListSource = (IList)newItem;
            else
            {
                SelectedItem = newItem;
            }
        }

        private void CanAddNew(object sender, CanExecuteRoutedEventArgs e)
        {
            var t = e.Parameter as Type;
            if (t != null && t.GetConstructor(Type.EmptyTypes) != null)
                e.CanExecute = true;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var list = (IList)( (FrameworkElement)e.OriginalSource ).Tag;

            list.Remove(e.Parameter);
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        private void MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            var list = (IList)( (FrameworkElement)e.OriginalSource ).Tag;

            var selectedItem = e.Parameter;
            int index = list.IndexOf(selectedItem);
            list.RemoveAt(index);
            list.Insert(++index, selectedItem);

            if (list == Items)
                SelectedSubListSource = (IList)selectedItem;
            else
                SelectedItem = selectedItem;
        }

        private void CanMoveDown(object sender, CanExecuteRoutedEventArgs e)
        {
            var list = (IList)( (FrameworkElement)e.OriginalSource ).Tag;

            if (e.Parameter != null && list.IndexOf(e.Parameter) < ( Items.Count - 1 ))
                e.CanExecute = true;
        }

        private void MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            var list = (IList)( (FrameworkElement)e.OriginalSource ).Tag;

            var selectedItem = e.Parameter;
            int index = list.IndexOf(selectedItem);
            list.RemoveAt(index);
            list.Insert(--index, selectedItem);
            if (list == Items)
                SelectedSubListSource = (IList)selectedItem;
            else
                SelectedItem = selectedItem;
        }

        private void CanMoveUp(object sender, CanExecuteRoutedEventArgs e)
        {
            var list = (IList)( (FrameworkElement)e.OriginalSource ).Tag;

            if (e.Parameter != null && list.IndexOf(e.Parameter) > 0)
                e.CanExecute = true;
        }

        #endregion //Commands

        #region Methods

        public void PersistChanges()
        {
            IList list = ComputeItemsSource();
            if (list == null)
                return;

            //the easiest way to persist changes to the source is to just clear the source list and then add all items to it.
            list.Clear();

            foreach (object item in Items)
            {
                list.Add(item);
            }
        }

        private static void CopyValues(object source, object destination)
        {
            FieldInfo[] myObjectFields =
                source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo fi in myObjectFields)
            {
                fi.SetValue(destination, fi.GetValue(source));
            }
        }

        private object CreateClone(object source)
        {
            object clone = null;

            Type type = source.GetType();
            clone = Activator.CreateInstance(type);
            CopyValues(source, clone);

            return clone;
        }

        private object CreateNewItem(Type type)
        {
            return Activator.CreateInstance(type);
        }

        internal static Type GetListItemType(Type listType)
        {
            Type iListOfT = listType.GetInterfaces().FirstOrDefault(
                (i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IList<>));

            return (iListOfT != null)
                       ? iListOfT.GetGenericArguments()[0]
                       : null;
        }

        private IList ComputeItemsSource()
        {
            return ItemsSource;
        }

        #endregion //Methods 
    }
}