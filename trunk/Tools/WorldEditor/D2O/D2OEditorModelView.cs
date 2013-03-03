#region License GNU GPL
// D2OEditorModelView.cs
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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using WorldEditor.D2I;
using WorldEditor.Helpers;

namespace WorldEditor.D2O
{
    public class D2OEditorModelView
    {
        private ReadOnlyObservableCollection<object> m_readOnylRows;
        private ObservableCollection<object> m_rows = new ObservableCollection<object>();
        private ObservableCollection<string> m_searchProperties = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readOnlySearchProperties;
        private Dictionary<string, Func<object, object>> m_propertiesGetters = new Dictionary<string, Func<object, object>>();
        private D2OEditor m_editor;
        private readonly string m_filePath;
        private D2OReader m_reader;
        private Type[] m_distinctTypes;
        private Stack<D2OEditedObject> m_editedObjects = new Stack<D2OEditedObject>();

        public D2OEditorModelView(D2OEditor editor, string filePath)
        {
            m_editor = editor;
            m_filePath = filePath;
            m_readOnylRows = new ReadOnlyObservableCollection<object>(m_rows);
            m_readOnlySearchProperties = new ReadOnlyObservableCollection<string>(m_searchProperties);
            NewObjectTypes = new List<Type>();
            Open();
        }

        public ReadOnlyObservableCollection<object> Rows
        {
            get
            {
                return m_readOnylRows;
            }
        }

        private void Open()
        {
            m_reader = new D2OReader(m_filePath);
            foreach (var dataObject in m_reader.EnumerateObjects())
            {
                m_rows.Add(dataObject);
            }

            m_distinctTypes = m_rows.Select(x => x.GetType()).Distinct().ToArray();

            var properties = m_distinctTypes.SelectMany(x => x.GetProperties()).Distinct();

            foreach (var property in properties)
            {
                if (m_searchProperties.Contains(property.Name))
                    continue;

                if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    continue;

                var del = (Func<object, object>)property.GetGetMethod().CreateFuncDelegate(typeof(object));
                m_searchProperties.Add(property.Name);
                m_propertiesGetters.Add(property.Name, del);
            }

            NewObjectTypes.AddRange(m_distinctTypes.Where(x => x.GetConstructor(Type.EmptyTypes) != null));
        }

        public ReadOnlyObservableCollection<string> SearchProperties
        {
            get { return m_readOnlySearchProperties; }
        }

        public List<Type> NewObjectTypes
        {
            get;
            private set;
        }

        #region AddCommand

        private DelegateCommand m_addCommand;

        public DelegateCommand AddCommand
        {
            get { return m_addCommand ?? (m_addCommand = new DelegateCommand(OnAdd, CanAdd)); }
        }

        private bool CanAdd(object parameter)
        {
            return parameter is Type && (parameter as Type).GetConstructor(Type.EmptyTypes) != null;
        }

        private void OnAdd(object parameter)
        {
            if (parameter == null || !CanAdd(parameter))
                return;

            var obj = Activator.CreateInstance((Type)parameter);
            m_rows.Add(obj);

            m_editor.ObjectsGrid.SelectedItem = obj;
            m_editor.ObjectsGrid.ScrollIntoView(obj);
            m_editor.ObjectsGrid.Focus();

            var editedObject = new D2OEditedObject(obj, ObjectState.Added);
            m_editedObjects.Push(editedObject);
        }

        #endregion


        #region RemoveCommand

        private DelegateCommand m_removeCommand;

        public DelegateCommand RemoveCommand
        {
            get { return m_removeCommand ?? (m_removeCommand = new DelegateCommand(OnRemove, CanRemove)); }
        }

        private bool CanRemove(object parameter)
        {
            return parameter is IList && ((IList)parameter).Count > 0;
        }

        private void OnRemove(object parameter)
        {
            if (parameter == null || !CanRemove(parameter))
                return;

            // copy
            var list = (parameter as IList).OfType<object>().ToArray();

            foreach (var item in list)
            {
                m_rows.Remove(item);
                m_editedObjects.Push(new D2OEditedObject(item, ObjectState.Removed));
            }
        }

        #endregion


        #region ConvertCommand

        private DelegateCommand m_convertCommand;

        public DelegateCommand ConvertCommand
        {
            get { return m_convertCommand ?? (m_convertCommand = new DelegateCommand(OnConvert, CanConvert)); }
        }

        private bool CanConvert(object parameter)
        {
            return true;
        }

        private void OnConvert(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(m_reader.FilePath) + ".xml",
                InitialDirectory = Path.GetDirectoryName(m_reader.FilePath),
                Title = "Convert to ...",
                DefaultExt = ".xml",
                Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*",
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dialog.FileName;

            if (File.Exists(filePath))
                File.Delete(filePath);
            var json = JsonConvert.SerializeObject(Rows.ToArray(), new JsonSerializerSettings()
            {
                ContractResolver = SerializePropertyOnlyResolver.Instance,
            });
            var rootJson = "{\"Object\":" + json + "}";
            var document = JsonConvert.DeserializeXmlNode(rootJson, "root");
            document.Save(filePath);

            MessageService.ShowMessage(m_editor, string.Format("File converted to {0}", Path.GetFileName(filePath)));
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
            m_reader.Close();
            var filePath = m_reader.FilePath;
            PerformSave(filePath);
            m_reader = new D2OReader(filePath);
        }

        #endregion


        #region SaveAsCommand

        private DelegateCommand m_saveAsCommand;

        public DelegateCommand SaveAsCommand
        {
            get { return m_saveAsCommand ?? (m_saveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs)); }
        }

        private bool CanSaveAs(object parameter)
        {
            return true;
        }

        private void OnSaveAs(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                FileName = m_reader.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2i",
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dialog.FileName;
            PerformSave(filePath);

        }

        #endregion

        private void PerformSave(string filePath)
        {
            try
            {
                var writer = new D2OWriter(filePath, true);
                writer.StartWriting(false);

                foreach (var row in Rows)
                {
                    if (row is IIndexedData)
                        writer.Write(row, ( (IIndexedData)row ).Id);
                    else
                        writer.Write(row);
                }

                writer.EndWriting();
                MessageService.ShowMessage(m_editor, "File saved successfully");

            }
            catch (IOException ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex);
            }
        }


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
            get { return m_findCommand ?? (m_findCommand = new DelegateCommand(OnFind, CanFind)); }
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
            get { return m_findNextCommand ?? (m_findNextCommand = new DelegateCommand(OnFindNext, CanFindNext)); }
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

        public void Dispose()
        {
            m_reader.Close();
        }
    }

    public class SerializePropertyOnlyResolver : DefaultContractResolver
    {
        public new static readonly SerializePropertyOnlyResolver Instance = new SerializePropertyOnlyResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member is FieldInfo)
            {
                property.Ignored = true;
            }
            return property;
        }
    }
}