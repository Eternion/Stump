#region License GNU GPL
// TableEditor.cs
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
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DBSynchroniser;
using Stump.Core.Reflection;
using Stump.ORM;
using WorldEditor.Database;
using System.Linq;
using WorldEditor.Editors.Files.D2O;
using WorldEditor.Helpers;
using WorldEditor.Helpers.Converters;

namespace WorldEditor.Editors.Tables
{
    public class TableEditorModelView
    {
        private readonly D2OTable m_table;
        private ObservableCollection<object> m_rows;
        private ReadOnlyObservableCollection<object> m_readOnylRows;
        private readonly List<DataGridColumn> m_columns = new List<DataGridColumn>();
        private readonly ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private readonly Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private Dictionary<object, EditedObject> m_editedObjects = new Dictionary<object, EditedObject>(); 
        private readonly TableEditor m_editor;

        public TableEditorModelView(TableEditor editor, D2OTable table)
        {
            m_table = table;
            m_editor = editor;
            GenerateColumns();
            LoadTable();
        }


        public ReadOnlyObservableCollection<object> Rows
        {
            get
            {
                return m_readOnylRows;
            }
        }

        public ReadOnlyCollection<DataGridColumn> Columns
        {
            get
            {
                return m_columns.AsReadOnly();
            }
        }

        private void LoadTable()
        {
            var method = typeof (Stump.ORM.Database).GetMethodExt("Fetch", 1, new[] {typeof (Sql)});
            var generic = method.MakeGenericMethod(m_table.Type);
            var rows = ((IList)generic.Invoke(DatabaseManager.Instance.Database,
                                              new object[] {new Sql("SELECT * FROM `" + m_table.TableName + "`")}));

            m_rows = new ObservableCollection<object>(rows.OfType<object>());
            m_readOnylRows = new ReadOnlyObservableCollection<object>(m_rows);
        }


        private void GenerateColumns()
        {
            var properties = m_table.Type.GetProperties().Where(x => x.GetCustomAttribute<BinaryFieldAttribute>() == null);

            foreach (var property in properties)
            {
                if (m_searchProperties.Contains(property.Name))
                    continue;

                if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    continue;

                var del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object));
                m_searchProperties.Add(property.Name);
                m_propertiesGetters.Add(property.Name, del);

                var binding = new System.Windows.Data.Binding(property.Name);

                var column = new DataGridTemplateColumn();
                FrameworkElementFactory element;
                if (property.PropertyType == typeof(bool))
                {
                    element = new FrameworkElementFactory(typeof(CheckBox));
                    element.SetBinding(ToggleButton.IsCheckedProperty, binding);
                    element.SetValue(FrameworkElement.MarginProperty, new Thickness(1));
                    element.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    element.SetValue(UIElement.IsEnabledProperty, false);
                }
                else
                {
                    element = new FrameworkElementFactory(typeof(TextBlock));

                    if (property.GetCustomAttribute<I18NFieldAttribute>() != null)
                    {
                        binding.Converter = new IdToI18NTextConverter();
                        column.Width = 120;
                    }

                    element.SetBinding(TextBlock.TextProperty, binding);
                    element.SetValue(FrameworkElement.MarginProperty, new Thickness(1));
                }
                column.CellTemplateSelector = new CellTemplateSelector
                    {
                    Template = new DataTemplate(property.PropertyType)
                    {
                        VisualTree = element
                    },
                    DefaultTemplate = new DataTemplate(),
                    ExpectedType = property.ReflectedType,
                };

                column.Header = property.Name;

                m_columns.Add(column);
                m_editor.ObjectsGrid.Columns.Add(column);
            }
        }

        public void OnObjectEdited(object item)
        {
            if (!m_editedObjects.ContainsKey(item))
                m_editedObjects.Add(item, new EditedObject(item, ObjectState.Dirty));
        }

        public void OnObjectRemoved(object item)
        {
            if (m_editedObjects.ContainsKey(item))
                m_editedObjects[item].State = ObjectState.Removed;
            else
                m_editedObjects.Add(item, new EditedObject(item, ObjectState.Removed));
        }

        public void OnObjectAdded(object item)
        {
            if (m_editedObjects.ContainsKey(item))
                m_editedObjects[item].State = ObjectState.Added;
            else
                m_editedObjects.Add(item, new EditedObject(item, ObjectState.Added));
        }

        #region RemoveCommand

        private DelegateCommand m_removeCommand;

        public DelegateCommand RemoveCommand
        {
            get { return m_removeCommand ?? (m_removeCommand = new DelegateCommand(OnRemove, CanRemove)); }
        }

        private bool CanRemove(object parameter)
        {
            return parameter is IList && ( (IList)parameter ).Count > 0;
        }

        private void OnRemove(object parameter)
        {
            if (parameter == null || !CanRemove(parameter))
                return;   
            
            // copy
            var list = ( parameter as IList ).OfType<object>().ToArray();

            foreach (var item in list)
            {
                m_rows.Remove(item);
                OnObjectRemoved(item);
            }
        }

        #endregion


        #region AddCommand

        private DelegateCommand m_addCommand;

        public DelegateCommand AddCommand
        {
            get { return m_addCommand ?? (m_addCommand = new DelegateCommand(OnAdd, CanAdd)); }
        }

        private bool CanAdd(object parameter)
        {
            return true;
        }

        private void OnAdd(object parameter)
        {
            var obj = Activator.CreateInstance(m_table.Type);

            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                }
            }

            m_rows.Add(obj);

            m_editor.ObjectsGrid.SelectedItem = obj;
            m_editor.ObjectsGrid.ScrollIntoView(obj);
            m_editor.ObjectsGrid.Focus();

            OnObjectAdded(obj);
        }

        #endregion


        #region SaveCommand

        private DelegateCommand m_saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return m_saveCommand ?? (m_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        private bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            foreach (var obj in m_editedObjects.Values.ToArray())
            {
                m_editedObjects.Remove(obj.Object);
                SaveEditedObject(obj);
            }
        }

        private void SaveEditedObject(EditedObject obj)
        {
            switch (obj.State)
            {
                case ObjectState.Added:
                    DatabaseManager.Instance.Database.Insert(obj.Object);
                    break;
                case ObjectState.Removed:
                    DatabaseManager.Instance.Database.Delete(obj.Object);
                    break;
                case ObjectState.Dirty:
                    DatabaseManager.Instance.Database.Update(obj.Object);
                    break;
            }
        }

        #endregion
    }
}