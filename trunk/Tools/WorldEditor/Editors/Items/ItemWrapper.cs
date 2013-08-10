#region License GNU GPL

// ItemWrapper.cs
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Annotations;
using WorldEditor.Loaders.D2O;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Items
{
    public class ItemWrapper : INotifyPropertyChanged
    {
        private string m_name;
        private bool m_nameChanged;
        private string m_description;
        private bool m_descriptionChanged;
        protected ObservableCollection<EffectWrapper> m_effects;

        protected ItemWrapper()
        {
        }

        public ItemWrapper(Item wrappedItem)
        {
            WrappedItem = wrappedItem;
            m_effects = new ObservableCollection<EffectWrapper>(PossibleEffects.Select(EffectWrapper.Create));
            m_effects.CollectionChanged += OnEffectsChanged;
        }

        protected void OnEffectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (EffectWrapper effect in e.NewItems)
                {
                    WrappedItem.PossibleEffects.Add(effect.WrappedEffect);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (EffectWrapper effect in e.OldItems)
                {
                    WrappedItem.PossibleEffects.Remove(effect.WrappedEffect);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                WrappedItem.PossibleEffects.Clear();
            }
        }

        public Item WrappedItem
        {
            get;
            protected set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = I18NDataManager.Instance.ReadText(NameId)); }
            set { m_name = value;
                m_nameChanged = true;
            }
        }

        public int Id
        {
            get { return WrappedItem.id; }
            set { WrappedItem.id = value; }
        }

        public uint NameId
        {
            get { return WrappedItem.nameId; }
            set { WrappedItem.nameId = value; }
        }

        public int TypeId
        {
            get { return (int)WrappedItem.typeId; }
            set
            {
                WrappedItem.typeId = (uint)value;
                WrappedItem.type = ObjectDataManager.Instance.Get<ItemType>((uint)value);
                OnPropertyChanged("Type");
            }
        }

        public uint DescriptionId
        {
            get { return WrappedItem.descriptionId; }
            set { WrappedItem.descriptionId = value; }
        }

        public string Description
        {
            get
            {
                return m_description ?? ( m_description = I18NDataManager.Instance.ReadText(DescriptionId) );
            }
            set { m_description = value;
                m_descriptionChanged = true;
            }
        }

        public int IconId
        {
            get { return WrappedItem.iconId; }
            set { WrappedItem.iconId = value; }
        }

        public uint Level
        {
            get { return WrappedItem.level; }
            set { WrappedItem.level = value; }
        }

        public uint RealWeight
        {
            get { return WrappedItem.realWeight; }
            set { WrappedItem.realWeight = value;
                WrappedItem.weight = value;
            }
        }

        public Boolean Cursed
        {
            get { return WrappedItem.cursed; }
            set { WrappedItem.cursed = value; }
        }

        public int UseAnimationId
        {
            get { return WrappedItem.useAnimationId; }
            set { WrappedItem.useAnimationId = value; }
        }

        public Boolean Usable
        {
            get { return WrappedItem.usable; }
            set { WrappedItem.usable = value; }
        }

        public Boolean Targetable
        {
            get { return WrappedItem.targetable; }
            set { WrappedItem.targetable = value; }
        }

        public float Price
        {
            get { return WrappedItem.price; }
            set { WrappedItem.price = value; }
        }

        public Boolean TwoHanded
        {
            get { return WrappedItem.twoHanded; }
            set { WrappedItem.twoHanded = value; }
        }

        public Boolean Etheral
        {
            get { return WrappedItem.etheral; }
            set { WrappedItem.etheral = value; }
        }

        public uint ItemSetId
        {
            get { return (uint)WrappedItem.itemSetId; }
            set { WrappedItem.itemSetId = (int)value; }
        }

        public String Criteria
        {
            get { return WrappedItem.criteria; }
            set { WrappedItem.criteria = value; }
        }

        public String CriteriaTarget
        {
            get { return WrappedItem.criteriaTarget; }
            set { WrappedItem.criteriaTarget = value; }
        }

        public Boolean HideEffects
        {
            get { return WrappedItem.hideEffects; }
            set { WrappedItem.hideEffects = value; }
        }

        public Boolean Enhanceable
        {
            get { return WrappedItem.enhanceable; }
            set { WrappedItem.enhanceable = value; }
        }

        public Boolean NonUsableOnAnother
        {
            get { return WrappedItem.nonUsableOnAnother; }
            set { WrappedItem.nonUsableOnAnother = value; }
        }

        public uint AppearanceId
        {
            get { return WrappedItem.appearanceId; }
            set { WrappedItem.appearanceId = value; }
        }

        public Boolean SecretRecipe
        {
            get { return WrappedItem.secretRecipe; }
            set { WrappedItem.secretRecipe = value; }
        }

        public List<uint> RecipeIds
        {
            get { return WrappedItem.recipeIds; }
            set { WrappedItem.recipeIds = value; }
        }

        public Boolean BonusIsSecret
        {
            get { return WrappedItem.bonusIsSecret; }
            set { WrappedItem.bonusIsSecret = value; }
        }

        public List<EffectInstance> PossibleEffects
        {
            get { return WrappedItem.possibleEffects; }
            set { WrappedItem.possibleEffects = value; }
        }

        public ObservableCollection<EffectWrapper> WrappedEffects
        {
            get { return m_effects; }
        }

        public List<uint> FavoriteSubAreas
        {
            get { return WrappedItem.favoriteSubAreas; }
            set { WrappedItem.favoriteSubAreas = value; }
        }

        public uint FavoriteSubAreasBonus
        {
            get { return WrappedItem.favoriteSubAreasBonus; }
            set { WrappedItem.favoriteSubAreasBonus = value; }
        }

        public ItemType Type
        {
            get { return WrappedItem.type; }
            set
            {
                WrappedItem.type = value;
                WrappedItem.typeId = (uint)value.id;
                OnPropertyChanged("TypeId");
            }
        }

        public uint Weight
        {
            get { return WrappedItem.weight; }
            set { WrappedItem.weight = value;
                WrappedItem.realWeight = value;
            }
        }

        public virtual void Save()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}