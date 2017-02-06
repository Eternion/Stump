using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Shortcuts;
using Shortcut = Stump.Server.WorldServer.Database.Shortcuts.Shortcut;

namespace Stump.Server.WorldServer.Game.Shortcuts
{
    public class ShortcutBar
    {
        public const int MaxSlot = 40;

        private readonly object m_locker = new object();
        private readonly Queue<Shortcut> m_shortcutsToDelete = new Queue<Shortcut>();
        private Dictionary<int, SpellShortcut> m_spellShortcuts = new Dictionary<int, SpellShortcut>();
        private Dictionary<int, ItemShortcut> m_itemShortcuts = new Dictionary<int, ItemShortcut>();
        private Dictionary<int, PresetShortcut> m_presetShortcuts = new Dictionary<int, PresetShortcut>(); 

        public ShortcutBar(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<int, SpellShortcut> SpellsShortcuts
        {
            get { return new ReadOnlyDictionary<int, SpellShortcut>(m_spellShortcuts); }
        }
        
        public IReadOnlyDictionary<int, ItemShortcut> ItemsShortcuts
        {
            get { return new ReadOnlyDictionary<int, ItemShortcut>(m_itemShortcuts); }
        }

        public IReadOnlyDictionary<int, PresetShortcut> PresetShortcuts
        {
            get { return new ReadOnlyDictionary<int, PresetShortcut>(m_presetShortcuts); }
        }

        internal void Load()
        {
            var database = WorldServer.Instance.DBAccessor.Database;

            m_spellShortcuts = database.Query<SpellShortcut>(string.Format(SpellShortcutRelator.FetchByOwner, Owner.Id)).DistinctBy(x => x.Slot).ToDictionary(x => x.Slot);
            m_itemShortcuts = database.Query<ItemShortcut>(string.Format(ItemShortcutRelator.FetchByOwner, Owner.Id)).DistinctBy(x => x.Slot).ToDictionary(x => x.Slot);
            m_presetShortcuts = database.Query<PresetShortcut>(string.Format(PresetShortcutRelator.FetchByOwner, Owner.Id)).DistinctBy(x => x.Slot).ToDictionary(x => x.Slot);
        }

        public void AddShortcut(ShortcutBarEnum barType, DofusProtocol.Types.Shortcut shortcut)
        {
            // do not ask me why i use a sbyte, they are fucking idiots
            if (shortcut is ShortcutSpell && barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                AddSpellShortcut(shortcut.slot, ((ShortcutSpell) shortcut).spellId);
            else if (shortcut is ShortcutObjectItem && barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
            {
                var item = Owner.Inventory.TryGetItem(((ShortcutObjectItem) shortcut).itemUID);

                if (item != null)
                    AddItemShortcut(shortcut.slot, item);
                else
                    ShortcutHandler.SendShortcutBarAddErrorMessage(Owner.Client);
            }
            else if (shortcut is ShortcutObjectPreset && barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                AddPresetShortcut(shortcut.slot, ((ShortcutObjectPreset) shortcut).presetId);
            else
            {
                ShortcutHandler.SendShortcutBarAddErrorMessage(Owner.Client);
            }
        }

        public void AddSpellShortcut(int slot, short spellId)
        {
            if (!IsSlotFree(slot, ShortcutBarEnum.SPELL_SHORTCUT_BAR))
                RemoveShortcut(ShortcutBarEnum.SPELL_SHORTCUT_BAR, slot);

            var shortcut = new SpellShortcut(Owner.Record, slot, spellId);

            m_spellShortcuts.Add(slot, shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.SPELL_SHORTCUT_BAR, shortcut);
        }

        public void AddItemShortcut(int slot, BasePlayerItem item)
        {
            if (!IsSlotFree(slot, ShortcutBarEnum.GENERAL_SHORTCUT_BAR))
                RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, slot);

            var shortcut = new ItemShortcut(Owner.Record, slot, item.Template.Id, item.Guid);

            m_itemShortcuts.Add(slot, shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR, shortcut);
        }

        public void AddPresetShortcut(int slot, int presetId)
        {
            if (!IsSlotFree(slot, ShortcutBarEnum.GENERAL_SHORTCUT_BAR))
                RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, slot);

            var shortcut = new PresetShortcut(Owner.Record, slot, presetId);

            m_presetShortcuts.Add(slot, shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR, shortcut);
        }

        public void SwapShortcuts(ShortcutBarEnum barType, int slot, int newSlot)
        {
            if (IsSlotFree(slot, barType))
                return;

            var shortcutToSwitch = GetShortcut(barType, slot);
            var shortcutDestination = GetShortcut(barType, newSlot);

            RemoveInternal(shortcutToSwitch);
            RemoveInternal(shortcutDestination);

            if (shortcutDestination != null)
            {
                shortcutDestination.Slot = slot;
                AddInternal(shortcutDestination);
                ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, barType, shortcutDestination);
            }
            else
            {
                ShortcutHandler.SendShortcutBarRemovedMessage(Owner.Client, barType, slot);
            }

            shortcutToSwitch.Slot = newSlot;
            AddInternal(shortcutToSwitch);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, barType, shortcutToSwitch);
        }

        public void RemoveShortcut(ShortcutBarEnum barType, int slot)
        {
            var shortcut = GetShortcut(barType, slot);

            if (shortcut == null)
                return;

            switch (barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    m_spellShortcuts.Remove(slot);
                    break;
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    {
                        if (shortcut is ItemShortcut)
                            m_itemShortcuts.Remove(slot);
                        else if (shortcut is PresetShortcut)
                            m_presetShortcuts.Remove(slot);
                    }
                    
                    break;
            }
            m_shortcutsToDelete.Enqueue(shortcut);

            ShortcutHandler.SendShortcutBarRemovedMessage(Owner.Client, barType, slot);
        }

        private void AddInternal(Shortcut shortcut)
        {
            if (shortcut is SpellShortcut && !m_spellShortcuts.ContainsKey(shortcut.Slot))
                m_spellShortcuts.Add(shortcut.Slot, (SpellShortcut)shortcut);
            else if (shortcut is ItemShortcut && !m_itemShortcuts.ContainsKey(shortcut.Slot))
                m_itemShortcuts.Add(shortcut.Slot, (ItemShortcut)shortcut);
            else if (shortcut is PresetShortcut && !m_presetShortcuts.ContainsKey(shortcut.Slot))
                m_presetShortcuts.Add(shortcut.Slot, (PresetShortcut)shortcut);
        }

        private void RemoveInternal(Shortcut shortcut)
        {
            if (shortcut is SpellShortcut)
            {
                m_spellShortcuts.Remove(shortcut.Slot);
                return;
            }

            if (shortcut is ItemShortcut)
            {
                m_itemShortcuts.Remove(shortcut.Slot);
                return;
            }

            if (shortcut is PresetShortcut)
            {
                m_presetShortcuts.Remove(shortcut.Slot);
                return;
            }
        }

        public int GetNextFreeSlot(ShortcutBarEnum barType)
        {
            for (var i = 0; i < MaxSlot; i++)
            {
                if (IsSlotFree(i, barType))
                    return i;
            }

            return MaxSlot;
        }

        public bool IsSlotFree(int slot, ShortcutBarEnum barType)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    return !m_spellShortcuts.ContainsKey(slot);
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    return !m_itemShortcuts.ContainsKey(slot) && !m_presetShortcuts.ContainsKey(slot);
            }

            return true;
        }

        public Shortcut GetShortcut(ShortcutBarEnum barType, int slot)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    return GetSpellShortcut(slot);
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    {
                        var shortcut = GetItemShortcut(slot);
                        if (shortcut == null)
                            return GetPresetShortcut(slot);

                        return shortcut;
                    }
                default:
                    return null;
            }
        }

        public IEnumerable<Shortcut> GetShortcuts(ShortcutBarEnum barType)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    return m_spellShortcuts.Values;
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    return m_itemShortcuts.Values.Concat<Shortcut>(m_presetShortcuts.Values);
                default:
                    return new Shortcut[0];
            }
        }

        public SpellShortcut GetSpellShortcut(int slot)
        {
            SpellShortcut shortcut;
            return m_spellShortcuts.TryGetValue(slot, out shortcut) ? shortcut : null;
        }

        public ItemShortcut GetItemShortcut(int slot)
        {
            ItemShortcut shortcut;
            return m_itemShortcuts.TryGetValue(slot, out shortcut) ? shortcut : null;
        }

        public PresetShortcut GetPresetShortcut(int slot)
        {
            PresetShortcut shortcut;
            return m_presetShortcuts.TryGetValue(slot, out shortcut) ? shortcut : null;
        }

        public void Save()
        {
            lock (m_locker)
            {
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var shortcut in m_itemShortcuts.Where(shortcut => shortcut.Value.IsDirty || shortcut.Value.IsNew).ToArray())
                {
                    database.Save(shortcut.Value);
                }

                foreach (var shortcut in m_spellShortcuts.Where(shortcut => shortcut.Value.IsDirty || shortcut.Value.IsNew).ToArray())
                {
                    database.Save(shortcut.Value);
                }

                foreach (var shortcut in m_presetShortcuts.Where(shortcut => shortcut.Value.IsDirty || shortcut.Value.IsNew).ToArray())
                {
                    database.Save(shortcut.Value);
                }

                while (m_shortcutsToDelete.Count > 0)
                {
                    var record = m_shortcutsToDelete.Dequeue();

                    if (record != null)
                        database.Delete(record);
                }
            }
        }
    }
}