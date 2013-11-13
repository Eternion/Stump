#region License GNU GPL
// ItemEditorModelView.cs
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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DBSynchroniser.Records;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Annotations;
using WorldEditor.Editors.Items.Effects;
using WorldEditor.Helpers;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.Icons;

namespace WorldEditor.Editors.Items
{
    public class ItemEditorModelView : INotifyPropertyChanged
    {
        public event Action<ItemEditorModelView, ItemWrapper> ItemSaved;

        protected virtual void OnItemSaved(ItemWrapper arg2)
        {
            var handler = ItemSaved;
            if (handler != null) handler(this, arg2);
        }

        public ItemEditorModelView(ItemRecord item)
            : this(item is WeaponRecord ? new WeaponWrapper((WeaponRecord)item) : new ItemWrapper(item))
        {
        }

        public ItemEditorModelView(ItemWrapper wrapper)
        {
            Item = wrapper;
            Types = ObjectDataManager.Instance.EnumerateObjects<ItemTypeRecord>().
                Where(x => IsWeapon ? !string.IsNullOrEmpty(x.RawZone) && x.RawZone != "null" : string.IsNullOrEmpty(x.RawZone) || x.RawZone == "null").ToList();
            ItemSets = ObjectDataManager.Instance.EnumerateObjects<ItemSetRecord>().ToList();
        }

        public ItemWrapper Item
        {
            get;
            set;
        }

        public List<ItemTypeRecord> Types
        {
            get;
            set;
        }

        public List<ItemSetRecord> ItemSets
        {
            get;
            set;
        }

        public bool IsWeapon
        {
            get { return Item is WeaponWrapper; }
        }


        #region ResetItemSetCommand

        private DelegateCommand m_resetItemSetCommand;

        public DelegateCommand ResetItemSetCommand
        {
            get { return m_resetItemSetCommand ?? (m_resetItemSetCommand = new DelegateCommand(OnResetItemSet, CanResetItemSet)); }
        }

        private static bool CanResetItemSet(object parameter)
        {
            return true;
        }

        private void OnResetItemSet(object parameter)
        {
            Item.ItemSetId = unchecked ((uint) -1);
        }

        #endregion


        #region ChangeIconCommand

        private DelegateCommand m_changeIconCommand;

        public DelegateCommand ChangeIconCommand
        {
            get { return m_changeIconCommand ?? (m_changeIconCommand = new DelegateCommand(OnChangeIcon, CanChangeIcon)); }
        }

        private static bool CanChangeIcon(object parameter)
        {
            return true;
        }

        private void OnChangeIcon(object parameter)
        {
            var dialog = new IconSelectionDialog
                {
                    IconsSource = IconsManager.Instance.Icons,
                    SelectedIcon = IconsManager.Instance.GetIcon(Item.IconId)
                };

            if (dialog.ShowDialog() == true)
                Item.IconId = dialog.SelectedIcon.Id;
        }

        #endregion


        #region EditEffectCommand

        private DelegateCommand m_editEffectCommand;
        private List<EffectRecord> m_effects;

        public DelegateCommand EditEffectCommand
        {
            get { return m_editEffectCommand ?? (m_editEffectCommand = new DelegateCommand(OnEditEffect, CanEditEffect)); }
        }

        private static bool CanEditEffect(object parameter)
        {
            return parameter is EffectWrapper;
        }

        private void OnEditEffect(object parameter)
        {
            if (parameter == null || !CanEditEffect(parameter))
                return;

            var effect = (EffectWrapper) parameter;

            var dialog = new EffectEditorDialog
                {
                    EffectToEdit = (EffectWrapper) (effect).Clone(),
                    EffectsSource =
                        m_effects ??
                        (m_effects = ObjectDataManager.Instance.EnumerateObjects<EffectRecord>().ToList())
                };

            if (dialog.ShowDialog() != true)
                return;

            var index = Item.WrappedEffects.IndexOf(effect);
            Item.WrappedEffects.Remove(effect);
            Item.WrappedEffects.Insert(index, dialog.EffectToEdit);
        }

        #endregion


        #region RemoveEffectCommand

        private DelegateCommand m_removeEffectCommand;

        public DelegateCommand RemoveEffectCommand
        {
            get { return m_removeEffectCommand ?? (m_removeEffectCommand = new DelegateCommand(OnRemoveEffect, CanRemoveEffect)); }
        }

        private static bool CanRemoveEffect(object parameter)
        {
            return parameter is EffectWrapper || parameter is ICollection;
        }

        private void OnRemoveEffect(object parameter)
        {
            if (parameter == null || !CanRemoveEffect(parameter))
                return;

            var wrapper = parameter as EffectWrapper;
            if (wrapper != null)
            {
                var effect = wrapper;

                Item.WrappedEffects.Remove(effect);
            }
            else
            {
                var collection = parameter as ICollection;
                if (collection != null)
                {
                    Array effects = new object[collection.Count];
                    collection.CopyTo(effects, 0);

                    foreach (EffectWrapper effect in effects)
                    {
                        Item.WrappedEffects.Remove(effect);
                    }
                }
            }
        }

        #endregion


        #region AddEffectCommand

        private DelegateCommand m_addEffectCommand;

        public DelegateCommand AddEffectCommand
        {
            get { return m_addEffectCommand ?? (m_addEffectCommand = new DelegateCommand(OnAddEffect, CanAddEffect)); }
        }

        private static bool CanAddEffect(object parameter)
        {
            return true;
        }

        private void OnAddEffect(object parameter)
        {
            var dialog = new EffectEditorDialog
                {
                    EffectsSource =
                        m_effects ??
                        (m_effects = ObjectDataManager.Instance.EnumerateObjects<EffectRecord>().ToList())
                };

            var effect = new EffectDiceWrapper(new EffectInstanceDice()
                {
                    EffectId = (uint)m_effects.First().Id
                });

            dialog.EffectToEdit = effect;

            if (dialog.ShowDialog() == true)
            {
                Item.WrappedEffects.Add(dialog.EffectToEdit);
            }
        }

        #endregion


        #region SaveCommand

        private DelegateCommand m_saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return m_saveCommand ?? (m_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            try
            {
                Item.Save();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(null, "Cannot save properly : " + ex);
                return;
            }

            OnItemSaved(Item);
            MessageService.ShowMessage(null, "File saved successfully");
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}