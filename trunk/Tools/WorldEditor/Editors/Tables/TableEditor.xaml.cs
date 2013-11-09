using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DBSynchroniser;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Editors.Files.D2O;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using IDataObject = System.Windows.IDataObject;

namespace WorldEditor.Editors.Tables
{
    /// <summary>
    /// Interaction logic for TableEditor.xaml
    /// </summary>
    public partial class TableEditor : Window
    {
        private Dictionary<Type, List<Type>> m_subTypes = new Dictionary<Type, List<Type>>();

        public TableEditor(D2OTable table)
        {
            InitializeComponent();
            DataContext = ModelView = new TableEditorModelView(this, table);
            FindSubClasses();
        }

        public TableEditorModelView ModelView
        {
            get;
            private set;
        }

         private void FindSubClasses()
        {
            foreach (var type in typeof(AbuseReasons).Assembly.GetTypes())
            {
                if (!type.HasInterface(typeof(Stump.DofusProtocol.D2oClasses.IDataObject)))
                    continue;

                if (type.BaseType != typeof(Object))
                {
                    var baseType = type.BaseType;
                    while (baseType.BaseType != typeof(Object))
                        baseType = baseType.BaseType;

                    if (!m_subTypes.ContainsKey(baseType))
                        m_subTypes.Add(baseType, new List<Type>());

                    m_subTypes[baseType].Add(type);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void ObjectsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            
            ModelView.RemoveCommand.RaiseCanExecuteChanged();
            ObjectEditor.EditorDefinitions.Clear();

            if (ObjectsGrid.SelectedItem == null)
                return;

            ModelView.OnObjectEdited(ObjectsGrid.SelectedItem);

            // hack here to handle lists of lists
            var type = ObjectsGrid.SelectedItem.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var listType = property.PropertyType.GetInterfaces().FirstOrDefault(
                    (i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));

                // it's a list
                if (listType != null)
                {
                    var elementType = listType.GetGenericArguments()[0];

                    // it's a list of a list
                    if (elementType.IsGenericType &&
                        elementType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        List<Type> newTypes;
                        m_subTypes.TryGetValue(elementType.GetGenericArguments()[0], out newTypes);

                        // copy to not change it
                        newTypes = newTypes != null ? newTypes.ToList() : new List<Type>();
                        newTypes.Add(elementType.GetGenericArguments()[0]);

                        ObjectEditor.EditorDefinitions.Add(new EditorDefinition()
                        {
                            Editor =
                                new CollectionEditorResolver(this,
                                elementType.GetGenericArguments()[0])
                                {
                                    NewTypes = newTypes,
                                },
                            TargetType = property.PropertyType,
                        });
                    }
                    else if (!elementType.IsPrimitive && elementType != typeof(string))
                    {
                        List<Type> newTypes;
                        m_subTypes.TryGetValue(elementType, out newTypes);

                        // copy to not change it
                        newTypes = newTypes != null ? newTypes.ToList() : new List<Type>();
                        newTypes.Add(elementType);

                        ObjectEditor.EditorDefinitions.Add(new EditorDefinition()
                        {
                            Editor = new CollectionEditor()
                            {
                                NewItemsTypes = newTypes
                            },
                            TargetType = property.PropertyType,
                        });
                    }
                }
            }
 
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

    }
}
