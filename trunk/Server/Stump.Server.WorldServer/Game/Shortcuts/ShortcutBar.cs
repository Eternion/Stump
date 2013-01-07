using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
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

        public ShortcutBar(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            private set;
        }

        internal void Load()
        {
            var database = WorldServer.Instance.DBAccessor.Database;
            m_spellShortcuts = database.Query<SpellShortcut>(string.Format(SpellShortcutRelator.FetchByOwner, Owner.Id)).ToDictionary(x => x.Slot);
            m_itemShortcuts = database.Query<ItemShortcut>(string.Format(ItemShortcutRelator.FetchByOwner, Owner.Id)).ToDictionary(x => x.Slot);
        }

        public void AddShortcut(ShortcutBarEnum barType, DofusProtocol.Types.Shortcut shortcut)
        {
            // do not ask me why i use a sbyte, they are fucking idiots
            if (shortcut is ShortcutSpell && barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                AddSpellShortcut(shortcut.slot, (shortcut as ShortcutSpell).spellId);
            else if (shortcut is ShortcutObjectItem && barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                AddItemShortcut(shortcut.slot, Owner.Inventory.TryGetItem((shortcut as ShortcutObjectItem).itemUID));
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

        public void AddItemShortcut(int slot, PlayerItem item)
        {
            if (!IsSlotFree(slot, ShortcutBarEnum.GENERAL_SHORTCUT_BAR))
                RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, slot);

            var shortcut = new ItemShortcut(Owner.Record, slot, item.Template.Id, item.Guid);

            m_itemShortcuts.Add(slot, shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR, shortcut);
        }

        public void SwapShortcuts(ShortcutBarEnum barType, int slot, int newSlot)
        {
            if (IsSlotFree(slot, barType))
                return;

            Shortcut shortcutToSwitch = GetShortcut(barType, slot);
            Shortcut shortcutDestination = GetShortcut(barType, newSlot);

            if (shortcutDestination != null)
            {
                shortcutDestination.Slot = slot;
                ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, barType, shortcutDestination);
            }
            else
            {
                ShortcutHandler.SendShortcutBarRemovedMessage(Owner.Client, barType, slot);
            }

            shortcutToSwitch.Slot = newSlot;
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, barType, shortcutToSwitch);
        }

        public void RemoveShortcut(ShortcutBarEnum barType, int slot)
        {
            Shortcut shortcut = GetShortcut(barType, slot);

            if (shortcut == null)
                return;

            if (barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                m_spellShortcuts.Remove(slot);
            else if (barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                m_itemShortcuts.Remove(slot);

            m_shortcutsToDelete.Enqueue(shortcut);

            ShortcutHandler.SendShortcutBarRemovedMessage(Owner.Client, barType, slot);
        }

        public int GetNextFreeSlot(ShortcutBarEnum barType)
        {
            for (int i = 0; i < MaxSlot; i++)
            {
                if (IsSlotFree(i, barType))
                    return i;
            }

            return MaxSlot;
        }

        public bool IsSlotFree(int slot, ShortcutBarEnum barType)
        {
            if (barType == ShortcutBarEnum.SPELL_SHORTCUT_BAR)
                return !m_spellShortcuts.ContainsKey(slot);
            else if (barType == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                return !m_itemShortcuts.ContainsKey(slot);
            else
                return true;
        }

        public Shortcut GetShortcut(ShortcutBarEnum barType, int slot)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    return GetSpellShortcut(slot);
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    return GetItemShortcut(slot);
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
                    return m_itemShortcuts.Values;
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

        public void Save()
        {
            lock (m_locker)
            {
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var shortcut in m_itemShortcuts)
                {
                    database.Save(shortcut.Value);
                }

                foreach (var shortcut in m_spellShortcuts)
                {
                    database.Save(shortcut.Value);
                }

                while (m_shortcutsToDelete.Count > 0)
                {
                    Shortcut record = m_shortcutsToDelete.Dequeue();

                    if (record != null)
                        database.Delete(record);
                }
            }
        }
    }
}