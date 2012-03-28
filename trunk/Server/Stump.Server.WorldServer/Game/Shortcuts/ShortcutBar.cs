using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Shortcuts;
using Item = Stump.Server.WorldServer.Game.Items.Item;
using Shortcut = Stump.Server.WorldServer.Database.Shortcuts.Shortcut;

namespace Stump.Server.WorldServer.Game.Shortcuts
{
    public class ShortcutBar
    {
        private readonly object m_locker = new object();
        private readonly Queue<Shortcut> m_shortcutsToDelete = new Queue<Shortcut>();
        private List<Shortcut> m_shortcuts;

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
            m_shortcuts = new List<Shortcut>();
            m_shortcuts.AddRange(Shortcut.FindByOwnerId(Owner.Id));
        }

        public void AddShortcut(sbyte barType, DofusProtocol.Types.Shortcut shortcut)
        {
            // do not ask me why i use a sbyte, they are fucking idiots
            if (shortcut is ShortcutSpell && barType == 2)
                AddSpellShortcut(shortcut.slot, (shortcut as ShortcutSpell).spellId);
            else if (shortcut is ShortcutObjectItem && barType == 0)
                AddItemShortcut(shortcut.slot, Owner.Inventory.TryGetItem((shortcut as ShortcutObjectItem).itemUID));
            else
            {
                ShortcutHandler.SendShortcutBarAddErrorMessage(Owner.Client);
            }
        }

        public void AddSpellShortcut(int slot, short spellId)
        {
            if (!IsSlotFree(slot))
                RemoveShortcut(ShortcutBarEnum.SPELL, slot);

            var shortcut = new SpellShortcut(Owner.Record, slot, spellId);

            m_shortcuts.Add(shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.SPELL, shortcut);
        }

        public void AddItemShortcut(int slot, Item item)
        {
            if (!IsSlotFree(slot))
                RemoveShortcut(ShortcutBarEnum.OBJECT, slot);

            var shortcut = new ItemShortcut(Owner.Record, slot, item.Template.Id, item.Guid);

            m_shortcuts.Add(shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.OBJECT, shortcut);
        }

        public void SwapShortcuts(ShortcutBarEnum barType, int slot, int newSlot)
        {
            if (IsSlotFree(slot))
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

            m_shortcuts.Remove(shortcut);
            m_shortcutsToDelete.Enqueue(shortcut);

            ShortcutHandler.SendShortcutBarRemovedMessage(Owner.Client, barType, slot);
        }

        public bool IsSlotFree(int slot)
        {
            return m_shortcuts.Count(entry => entry.Slot == slot) == 0;
        }

        public Shortcut GetShortcut(ShortcutBarEnum barType, int slot)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL:
                    return GetSpellShortcut(slot);
                case ShortcutBarEnum.OBJECT:
                    return GetItemShortcut(slot);
                default:
                    return null;
            }
        }

        public IEnumerable<Shortcut> GetShortcuts(ShortcutBarEnum barType)
        {
            switch (barType)
            {
                case ShortcutBarEnum.SPELL:
                    return m_shortcuts.Where(entry => entry is SpellShortcut);
                case ShortcutBarEnum.OBJECT:
                    return m_shortcuts.Where(entry => entry is ItemShortcut);
                default:
                    return new Shortcut[0];
            }
        }

        public SpellShortcut GetSpellShortcut(int slot)
        {
            return m_shortcuts.Where(entry => entry is SpellShortcut && entry.Slot == slot).SingleOrDefault() as SpellShortcut;
        }

        public ItemShortcut GetItemShortcut(int slot)
        {
            return m_shortcuts.Where(entry => entry is ItemShortcut && entry.Slot == slot).SingleOrDefault() as ItemShortcut;
        }

        public void Save()
        {
            lock (m_locker)
            {
                foreach (Shortcut shortcut in m_shortcuts)
                {
                    shortcut.Save();
                }

                while (m_shortcutsToDelete.Count > 0)
                {
                    Shortcut record = m_shortcutsToDelete.Dequeue();

                    if (record != null)
                        record.Delete();
                }
            }
        }
    }
}