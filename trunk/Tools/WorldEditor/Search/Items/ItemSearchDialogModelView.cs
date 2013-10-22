#region License GNU GPL
// ItemSearchDialogModelView.cs
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
using System.Collections.ObjectModel;
using WorldEditor.Editors.Items;

namespace WorldEditor.Search.Items
{
    public class ItemSearchDialogModelView : SearchDialogModelView
    {
        public ItemSearchDialogModelView(Type itemType, ObservableCollection<object> source)
            : base(itemType, source)
        {
        }

        protected override bool CanEditItem(object parameter)
        {
            return parameter is ItemWrapper;
        }

        protected override void OnEditItem(object parameter)
        {
            if (parameter == null || !CanEditItem(parameter))
                return;

            var editor = new ItemEditor(( (ItemWrapper)parameter ).WrappedItem);
            editor.Show();
        }
    }
}