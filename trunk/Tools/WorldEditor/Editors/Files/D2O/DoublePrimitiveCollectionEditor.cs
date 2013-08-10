#region License GNU GPL

// DoublePrimitiveCollectionEditor.cs
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
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorldEditor.Editors.Files.D2O
{
    public class DoublePrimitiveCollectionEditor : Control, IPersistableChanged
    {
        #region Properties

        private Type m_listType;

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<IList>), typeof(DoublePrimitiveCollectionEditor),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(DoublePrimitiveCollectionEditor),
                                        new UIPropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedSubList", typeof(IList), typeof(DoublePrimitiveCollectionEditor),
                                        new UIPropertyMetadata(null));

        public ObservableCollection<IList> Items
        {
            get
            {
                return (ObservableCollection<IList>)GetValue(ItemsProperty);
            }
            set { SetValue(ItemsProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public IList SelectedSubList
        {
            get
            {
                return (IList) GetValue(SelectedItemProperty);
            }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var CollectionControl = (DoublePrimitiveCollectionEditor)d;
            if (CollectionControl != null)
                CollectionControl.OnItemSourceChanged((IList) e.OldValue, (IList) e.NewValue);
        }

        public void OnItemSourceChanged(IList oldValue, IList newValue)
        {
            m_listType = null;

            if (newValue != null)
            {
                foreach (IList item in newValue)
                    Items.Add((IList)CreateClone(item));

                var type = newValue.GetType();
                m_listType = type.GetGenericArguments()[0].GetGenericArguments()[0];
            }
        }

        #endregion //Properties

        #region Constructors

        static DoublePrimitiveCollectionEditor()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("/WorldEditor;component/D2O/Template.xaml", UriKind.Relative)
            });
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoublePrimitiveCollectionEditor),
                                                     new FrameworkPropertyMetadata(typeof(DoublePrimitiveCollectionEditor)));
        }

        public DoublePrimitiveCollectionEditor(Type listType)
        {
            m_listType = listType;
            Items = new ObservableCollection<IList>();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, AddNew, CanAddNew));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete, CanDelete));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, MoveDown, CanMoveDown));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, MoveUp, CanMoveUp));
        }

        #endregion //Constructors

        #region Commands

        private void AddNew(object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = CreateNewItem(m_listType);
            Items.Add(newItem);
            SelectedSubList = newItem;
        }

        private void CanAddNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            Items.Remove((IList)e.Parameter);
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        private void MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItem = (IList)e.Parameter;
            int index = Items.IndexOf(selectedItem);
            Items.RemoveAt(index);
            Items.Insert(++index, selectedItem);
            SelectedSubList = selectedItem;
        }

        private void CanMoveDown(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null && Items.IndexOf((IList)e.Parameter) < (Items.Count - 1))
                e.CanExecute = true;
        }

        private void MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItem = (IList)e.Parameter;
            int index = Items.IndexOf(selectedItem);
            Items.RemoveAt(index);
            Items.Insert(--index, selectedItem);
            SelectedSubList = selectedItem;
        }

        private void CanMoveUp(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null && Items.IndexOf((IList)e.Parameter) > 0)
                e.CanExecute = true;
        }

        #endregion //Commands

        #region Methods

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

        private IList CreateNewItem(Type type)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        }

        internal static Type GetListItemType(Type listType)
        {
            Type iListOfT = listType.GetInterfaces().FirstOrDefault(
                (i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IList<>));

            return (iListOfT != null)
                       ? iListOfT.GetGenericArguments()[0]
                       : null;
        }

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

        private IList ComputeItemsSource()
        {
            if (ItemsSource == null)
                return ItemsSource = CreateItemsSource();

            return ItemsSource;
        }


        private IList CreateItemsSource()
        {
            IList list = null;

            if (m_listType != null)
            {
                ConstructorInfo constructor = typeof(List<>).MakeGenericType(typeof(List<>).MakeGenericType(m_listType)).GetConstructor(Type.EmptyTypes);
                list = (IList)constructor.Invoke(null);
            }

            return list;
        }

        #endregion //Methods
    }
}