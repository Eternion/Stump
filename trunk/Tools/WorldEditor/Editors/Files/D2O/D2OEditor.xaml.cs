using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using IDataObject = Stump.DofusProtocol.D2oClasses.IDataObject;

namespace WorldEditor.Editors.Files.D2O
{
    /// <summary>
    /// Interaction logic for D2OEditor.xaml
    /// </summary>
    public partial class D2OEditor : Window
    {
        private List<string> m_columnsName = new List<string>(); 
        private Dictionary<Type, List<Type>> m_subTypes = new Dictionary<Type, List<Type>>();

        public D2OEditor(string filename)
        {
            InitializeComponent();
            ModelView = new D2OEditorModelView(this, filename);
            DataContext = ModelView;
            FindSubClasses();
            CreateColumns();
        }

        public D2OEditorModelView ModelView
        {
            get;
            private set;
        }

        private void FindSubClasses()
        {
            foreach (var type in typeof(AbuseReasons).Assembly.GetTypes())
            {
                if (!type.HasInterface(typeof(IDataObject)))
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

        private void CreateColumns()
        {
            var types = ModelView.Rows.Select(x => x.GetType()).Distinct();

            foreach (var type in types)
            {
                foreach (var property in type.GetProperties())
                {
                    var name = property.Name;

                    if (m_columnsName.Contains(name))
                        continue;

                    var binding = new Binding(name);

                    DataGridColumn column;

                    if (property.PropertyType == typeof(bool))
                        column = new DataGridCheckBoxColumn()
                        {
                            Binding = binding
                        };
                    else
                        column = new DataGridTextColumn()
                        {
                            Binding = binding
                        };

                    column.Header = name;

                    ObjectsGrid.Columns.Add(column);
                    m_columnsName.Add(name);
                }
            }
        }

        private void ObjectsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveCommand.RaiseCanExecuteChanged();
            ObjectEditor.EditorDefinitions.Clear();
            
            if (ObjectsGrid.SelectedItem == null)
                return;

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
                        elementType.GetGenericTypeDefinition() == typeof (List<>))
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
            ModelView.FindCommand.RaiseCanExecuteChanged();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ModelView.Dispose();
        }
    }

    internal class CollectionEditorResolver : ITypeEditor
    {
        private readonly Window m_dialogOwner;
        private readonly Type m_listType;

        public CollectionEditorResolver(Window dialogOwner, Type listType)
        {
            m_dialogOwner = dialogOwner;
            m_listType = listType;
        }

        public List<Type> NewTypes
        {
            get;
            set;
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            var button = new Button();
            button.Content = "(Collection)";
            button.Margin = new Thickness(2);
            button.Click += (sender, e) =>
            {
                if (m_listType.IsPrimitive || m_listType == typeof(string))
                {
                    var editor = new DoublePrimitiveCollectionEditor(m_listType);
                    var dialog = new EditorDialog( editor);
                    dialog.Width = 600;
                    dialog.Height = 400;

                    var binding = new Binding("Value")
                    {
                        Source = propertyItem,
                        Mode = BindingMode.OneWay
                    };

                    BindingOperations.SetBinding(editor, DoublePrimitiveCollectionEditor.ItemsSourceProperty, binding);
                    dialog.ShowDialog();
                }
                else
                {
                    var editor = new DoubleCollectionEditor(m_listType);
                    editor.NewItemTypes = NewTypes;
                    var dialog = new EditorDialog(editor);
                    dialog.Width = 800;
                    dialog.Height = 400;

                    var binding = new Binding("Value")
                    {
                        Source = propertyItem,
                        Mode = BindingMode.OneWay
                    };

                    BindingOperations.SetBinding(editor, DoubleCollectionEditor.ItemsSourceProperty, binding);
                    dialog.ShowDialog();
                }
            };

            return button;
        }
    }
}
