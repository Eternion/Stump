using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AvalonDock.Layout;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;

namespace WorldEditor.Maps
{
    /// <summary>
    /// Interaction logic for MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {
        public MapEditor(DlmReader reader)
        {
            InitializeComponent();
            ModelView = new MapEditorModelView(this, reader);
            DataContext = ModelView;
        }

        public MapEditorModelView ModelView
        {
            get;
            private set;
        }


        public LayoutDocument AddTab(string title, object content)
        {
            var document = new LayoutDocument()
            {
                Content = content,
                Title = title,
            };

            DocumentPane.Children.Add(document);

            return document;
        }
    }
}
