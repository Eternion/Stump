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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DBSynchroniser.Records;
using WorldEditor.Database;
using WorldEditor.Editors.Items;

namespace WorldEditor.Search.Items
{
    public class ItemSearchDialogModelView : SearchDialogModelView<ItemWrapper>
    {
        public ItemSearchDialogModelView()
            : base (typeof(ItemRecord))
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

        public override IEnumerable<ItemWrapper> FindMatches()
        {
            var whereStatement = GetSQLWhereStatement();

            var matches = new List<ItemRecord>();

            if (string.IsNullOrEmpty(whereStatement))
            {
                matches = DatabaseManager.Instance.Database.Fetch<ItemRecord>("SELECT * FROM Items");
                matches.AddRange(DatabaseManager.Instance.Database.Query<WeaponRecord>("SELECT * FROM Weapons"));
            }
            else
            {
                matches = DatabaseManager.Instance.Database.Fetch<ItemRecord>("SELECT * FROM Items WHERE " + whereStatement.Replace("'", "\\'"));
                matches.AddRange(DatabaseManager.Instance.Database.Query<WeaponRecord>("SELECT * FROM Weapons WHERE " + whereStatement.Replace("'", "\\'")));
            }

            return matches.Select(x => new ItemWrapper(x));
        }
    }
}