/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus edition at http://xceed.com/wpf_toolkit

   Visit http://xceed.com and follow @datagrid on Twitter

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    public partial class CollectionEditor : UserControl, ITypeEditor
    {
        private PropertyItem _item;

        public CollectionEditor()
        {
            InitializeComponent();
        }

        public List<Type> NewItemsTypes
        {
            get;
            set;
        }

        #region ITypeEditor Members

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            _item = propertyItem;
            return this;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var editor = new CollectionControlDialog(_item.PropertyType);

            if (NewItemsTypes != null)
                editor.NewItemTypes = NewItemsTypes;

            var binding = new Binding("Value");
            binding.Source = _item;
            binding.Mode = _item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(editor, CollectionControlDialog.ItemsSourceProperty, binding);

            var binding2 = new Binding("NewItemsTypes");
            binding2.Source = this;
            binding2.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(editor, CollectionControlDialog.NewItemTypesProperty, binding2);

            editor.ShowDialog();
        }
    }
}