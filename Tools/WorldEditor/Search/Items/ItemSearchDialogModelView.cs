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
using WorldEditor.Loaders.I18N;

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

        protected override bool CanCopyItem(object parameter)
        {
            return parameter is ItemWrapper;
        }

        protected override void OnCopyItem(object parameter)
        {            
            if (parameter == null || !CanEditItem(parameter))
                return;

            var item = ((ItemWrapper) parameter).WrappedItem;
            ItemWrapper copy;

            if (item is WeaponRecord)
                copy = new WeaponWrapper();
            else copy = new ItemWrapper();

            copy.Name = I18NDataManager.Instance.ReadText(item.NameId);
            copy.TypeId = (int) item.TypeId;
            copy.Description = I18NDataManager.Instance.ReadText(item.DescriptionId);
            copy.IconId = item.IconId;
            copy.Level = item.Level;
            copy.RealWeight = item.RealWeight;
            copy.Cursed = item.Cursed;
            copy.UseAnimationId = item.UseAnimationId;
            copy.Usable = item.Usable;
            copy.Targetable = item.Targetable;
            copy.Price = item.Price;
            copy.TwoHanded = item.TwoHanded;
            copy.Etheral = item.Etheral;
            copy.ItemSetId = (uint) item.ItemSetId;
            copy.Criteria = item.Criteria;
            copy.CriteriaTarget = item.CriteriaTarget;
            copy.HideEffects = item.HideEffects;
            copy.Enhanceable = item.Enhanceable;
            copy.NonUsableOnAnother = item.NonUsableOnAnother;
            copy.AppearanceId = item.AppearanceId;
            copy.SecretRecipe = item.SecretRecipe;
            copy.RecipeIds = item.RecipeIds;
            copy.BonusIsSecret = item.BonusIsSecret;
            copy.WrappedEffects = new ObservableCollection<EffectWrapper>(item.PossibleEffects.Select(EffectWrapper.Create));
            copy.FavoriteSubAreas = item.FavoriteSubAreas;
            copy.FavoriteSubAreasBonus = item.FavoriteSubAreasBonus;
            copy.Weight = item.Weight;

            if (item is WeaponRecord)
            {
                var weapon = item as WeaponRecord;
                var copy_weapon = copy as WeaponWrapper;

                copy_weapon.ApCost = weapon.ApCost;
                copy_weapon.MinRange = weapon.MinRange;
                copy_weapon.Range = weapon.Range;
                copy_weapon.CastInLine = weapon.CastInLine;
                copy_weapon.CastInDiagonal = weapon.CastInDiagonal;
                copy_weapon.CastTestLos = weapon.CastTestLos;
                copy_weapon.CriticalHitProbability = weapon.CriticalHitProbability;
                copy_weapon.CriticalHitBonus = weapon.CriticalHitBonus;
                copy_weapon.CriticalFailureProbability = weapon.CriticalFailureProbability;

            }

            var editor = new ItemEditor(copy);
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
                matches = DatabaseManager.Instance.Database.Fetch<ItemRecord>("SELECT * FROM Items WHERE " + whereStatement);
                matches.AddRange(DatabaseManager.Instance.Database.Query<WeaponRecord>("SELECT * FROM Weapons WHERE " + whereStatement));
            }

            return matches.Select(x => new ItemWrapper(x));
        }
    }
}