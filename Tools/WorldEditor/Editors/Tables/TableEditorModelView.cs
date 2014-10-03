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
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using DBSynchroniser;
using Stump.Core.I18N;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using WorldEditor.Database;
using System.Linq;
using WorldEditor.Editors.Files.D2O;
using WorldEditor.Helpers;
using WorldEditor.Helpers.Converters;
using WorldEditor.Loaders.I18N;
using CheckBox = System.Windows.Controls.CheckBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace WorldEditor.Editors.Tables
{
    public class TableEditorModelView
    {
        private readonly D2OTable m_table;
        private ObservableCollection<object> m_rows;
        private ReadOnlyObservableCollection<object> m_readOnylRows;
        private readonly List<DataGridColumn> m_columns = new List<DataGridColumn>();
        private readonly List<string> m_searchProperties = new List<string>();
        private ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private readonly Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private readonly Dictionary<object, EditedObject> m_editedObjects = new Dictionary<object, EditedObject>(); 
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

        public ReadOnlyCollection<string> SearchProperties
        {
            get
            {
                return m_searchProperties.AsReadOnly();
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
                if (property.PropertyType == typeof (string))
                {
                    property.SetValue(obj, string.Empty);
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

            MessageService.ShowMessage(m_editor, "Table saved");
        }

        private static void SaveEditedObject(EditedObject obj)
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

            obj.State = ObjectState.None;
        }

        #endregion


        #region FindCommand

        private DelegateCommand m_findCommand;

        public int LastFoundIndex
        {
            get;
            set;
        }

        public string SearchText
        {
            get;
            set;
        }

        public string SearchProperty
        {
            get;
            set;
        }

        public DelegateCommand FindCommand
        {
            get
            {
                return m_findCommand ?? ( m_findCommand = new DelegateCommand(OnFind, CanFind) );
            }
        }

        private object FindNext()
        {
            int startIndex = LastFoundIndex == -1 || LastFoundIndex + 1 >= Rows.Count ? 0 : LastFoundIndex + 1;

            object row = null;
            int index = -1;

            if (string.IsNullOrEmpty(SearchProperty))
                return null;

            if (m_rows.Count == 0)
                return null;

            var getter = m_propertiesGetters[SearchProperty];
            var propertyType = getter(m_rows[0]).GetType();
            var isBool = propertyType == typeof(bool);
            var isInteger = propertyType == typeof(int) ||
                propertyType == typeof(uint) ||
                propertyType == typeof(short) ||
                propertyType == typeof(ushort);
            var isLong = propertyType == typeof(long) ||
                propertyType == typeof(ulong);
            var isDouble = propertyType == typeof(double) || propertyType == typeof(float);

            int? searchInteger = null;
            int dummy;
            if (int.TryParse(SearchText, out dummy))
                searchInteger = dummy;

            long? searchLong = null;
            long dummyL;
            if (long.TryParse(SearchText, out dummyL))
                searchLong = dummy;

            double? searchDouble = null;
            double dummyD;
            if (double.TryParse(SearchText, out dummyD))
                searchDouble = dummyD;

            bool? searchBool = SearchText.ToLower() == "true" || SearchText.ToLower() == "false" ?
                SearchText.ToLower() == "true" : (bool?)null;

            for (int i = startIndex; i < m_rows.Count; i++)
            {
                var value = getter(m_rows[i]);
                if (isBool)
                {
                    if (( searchBool.HasValue && searchBool == (bool)value ) ||
                        ( searchInteger.HasValue && (bool)value == ( searchInteger.Value != 0 ) ))
                    {
                        row = m_rows[i];
                        index = i;
                        break;
                    }
                }
                else if (isInteger)
                {
                    if (searchInteger.HasValue && searchInteger == Convert.ToInt32(value))
                    {
                        row = m_rows[i];
                        index = i;
                        break;
                    }
                }
                else if (isLong)
                {
                    if (searchLong.HasValue && searchLong == Convert.ToInt64(value))
                    {
                        row = m_rows[i];
                        index = i;
                        break;
                    }
                }
                else if (isDouble)
                {
                    if (searchDouble.HasValue && searchDouble == Convert.ToDouble(value))
                    {
                        row = m_rows[i];
                        index = i;
                        break;
                    }
                }
                else
                {
                    if (value.ToString().IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        row = m_rows[i];
                        index = i;
                        break;
                    }
                }
            }

            if (row == null)
            {
                LastFoundIndex = -1;
                return null;
            }
            else
            {
                LastFoundIndex = index;
                return row;
            }
        }

        private bool CanFind(object parameter)
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        private void OnFind(object parameter)
        {
            if (!CanFind(parameter))
                return;

            LastFoundIndex = 0;
            var row = FindNext();

            FindNextCommand.RaiseCanExecuteChanged();

            if (row != null)
            {
                m_editor.ObjectsGrid.SelectedItem = row;
                m_editor.ObjectsGrid.ScrollIntoView(row);
                m_editor.ObjectsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        #endregion



        #region FindNextCommand

        private DelegateCommand m_findNextCommand;

        public DelegateCommand FindNextCommand
        {
            get
            {
                return m_findNextCommand ?? ( m_findNextCommand = new DelegateCommand(OnFindNext, CanFindNext) );
            }
        }

        private bool CanFindNext(object parameter)
        {
            return true;
        }

        private void OnFindNext(object parameter)
        {
            var row = FindNext();

            if (row == null)
                row = FindNext();

            if (row != null)
            {
                m_editor.ObjectsGrid.SelectedItem = row;
                m_editor.ObjectsGrid.ScrollIntoView(row);
                m_editor.ObjectsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        #endregion
        

        #region GenerateEnumCommand

        private DelegateCommand m_generateEnumCommand;

        public DelegateCommand GenerateEnumCommand
        {
            get
            {
                return m_generateEnumCommand ?? (m_generateEnumCommand = new DelegateCommand(OnGenerateEnum, CanGenerateEnum));
            }
        }

        private bool CanGenerateEnum(object parameter)
        {
            return m_table.Type.GetProperties().Any(x => x.GetCustomAttribute<I18NFieldAttribute>() != null);
        }

        private void OnGenerateEnum(object parameter)
        {
            if (!CanGenerateEnum(parameter))
                return;

            var builder = new StringBuilder();
            builder.AppendLine("namespace Stump.DofusProtocol.Enums");
            builder.AppendLine("{");
            builder.AppendLine("\t");
            builder.AppendLine(string.Format("\tpublic enum {0}Enum", m_table.ClassName));
            builder.AppendLine("\t{");

            var nameProperty =
                m_table.Type.GetProperties().FirstOrDefault(x => x.GetCustomAttribute<I18NFieldAttribute>() != null);
            var idProperty = 
                m_table.Type.GetProperties().FirstOrDefault(x => x.GetCustomAttribute<PrimaryKeyAttribute>() != null);

            var names = new Dictionary<string, int>();
            foreach (var row in m_rows)
            {
                var nameId = nameProperty.GetValue(row);
                var id = (int)idProperty.GetValue(row);
                var nameRecord = I18NDataManager.Instance.GetText(nameId is uint ? (uint) nameId : (uint) (int) nameId);
                if (nameRecord == null)
                    continue;

                var name = !string.IsNullOrEmpty(nameRecord.English) ? nameRecord.English : nameRecord.French;

                var formattedName = name.Trim().ToUpper().Replace(" ", "_").Replace("\"", "").Replace("'", "_").Replace('-','_').
                    Replace("(", "_").Replace(")", "_").Replace("[", "_").Replace("]", "_");
                if (names.ContainsKey(formattedName))
                {
                    var otherId = names[formattedName];
                    names.Add(formattedName + "_" + otherId, otherId);
                    names.Remove(formattedName);
                    names.Add(formattedName + "_" + id, id);
                }
                else names.Add(formattedName, id);
            }

            foreach(var keyPair in names.OrderBy(x => x.Value))
                builder.AppendLine(string.Format("\t\t{0} = {1},", keyPair.Key, keyPair.Value));

            builder.AppendLine("\t}");
            builder.AppendLine("}");

            File.WriteAllText("./enums/" + m_table.ClassName + "Enum.cs", builder.ToString());
        }

        #endregion

    }
}