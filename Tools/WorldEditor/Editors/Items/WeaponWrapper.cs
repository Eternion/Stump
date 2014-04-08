#region License GNU GPL

// WeaponWrapper.cs
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
using DBSynchroniser.Records.Langs;
using Stump.Core.I18N;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Items
{
    public class WeaponWrapper : ItemWrapper
    {
        public WeaponWrapper()
        {
            WrappedItem = new WeaponRecord();
            m_name = new LangText();
            m_name.SetText(Languages.All, "New weapon");
            m_description = new LangText();
            m_description.SetText(Languages.All, "Item description");
            m_effects = new ObservableCollection<EffectWrapper>();
            WrappedItem.recipeIds = new List<uint>();
            WrappedItem.favoriteSubAreas = new List<uint>();
            WrappedItem.possibleEffects = new List<EffectInstance>();
            WrappedItem.criteria = "";
            WrappedItem.criteriaTarget = "";
            WrappedItem.itemSetId = -1;
            New = true;
        }

        public WeaponWrapper(WeaponRecord wrappedWeapon)
            : base(wrappedWeapon)
        {
            WrappedItem = wrappedWeapon;
        }

        public WeaponRecord WrappedWeapon
        {
            get
            {
                return (WeaponRecord) WrappedItem;
            }
        }

        public int ApCost
        {
            get { return WrappedWeapon.apCost; }
            set { WrappedWeapon.apCost = value; }
        }

        public int MinRange
        {
            get { return WrappedWeapon.minRange; }
            set { WrappedWeapon.minRange = value; }
        }

        public int Range
        {
            get { return WrappedWeapon.range; }
            set { WrappedWeapon.range = value; }
        }

        public Boolean CastInLine
        {
            get { return WrappedWeapon.castInLine; }
            set { WrappedWeapon.castInLine = value; }
        }

        public Boolean CastInDiagonal
        {
            get { return WrappedWeapon.castInDiagonal; }
            set { WrappedWeapon.castInDiagonal = value; }
        }

        public Boolean CastTestLos
        {
            get { return WrappedWeapon.castTestLos; }
            set { WrappedWeapon.castTestLos = value; }
        }

        public int CriticalHitProbability
        {
            get { return WrappedWeapon.criticalHitProbability; }
            set { WrappedWeapon.criticalHitProbability = value; }
        }

        public int CriticalHitBonus
        {
            get { return WrappedWeapon.criticalHitBonus; }
            set { WrappedWeapon.criticalHitBonus = value; }
        }

        public int CriticalFailureProbability
        {
            get { return WrappedWeapon.criticalFailureProbability; }
            set { WrappedWeapon.criticalFailureProbability = value; }
        }

        public override void Save()
        {
            if (New)
            {
                Id = Math.Max(ObjectDataManager.Instance.FindFreeId<ItemRecord>(), ObjectDataManager.Instance.FindFreeId<WeaponRecord>());
                NameId = I18NDataManager.Instance.FindFreeId();
                DescriptionId = NameId + 1;
            }

            WrappedItem.PossibleEffects = WrappedEffects.Select(x => x.WrappedEffect).ToList();

            if (New)
                ObjectDataManager.Instance.Insert(WrappedWeapon);
            else
                ObjectDataManager.Instance.Update(WrappedWeapon);

            if (New)
            {
                m_name.Id = NameId;
                m_description.Id = DescriptionId;
                I18NDataManager.Instance.CreateText(m_name);
                I18NDataManager.Instance.CreateText(m_description);
            }
            else
            {
                I18NDataManager.Instance.SaveText(m_name);
                I18NDataManager.Instance.SaveText(m_description);
            }

            New = false;
        }
    }
}