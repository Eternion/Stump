using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Handlers.Shortcuts;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using DofusShortcut = Stump.DofusProtocol.Types.Shortcut;
using Item = Stump.Server.WorldServer.Worlds.Items.Item;
using Shortcut = Stump.Server.WorldServer.Database.Shortcuts.Shortcut;

namespace Stump.Server.WorldServer.Worlds.Shortcuts
{
    public class ShortcutBar
    {
        private readonly IList<Shortcut> m_shortcuts;

        public ShortcutBar(Character owner)
        {
            Owner = owner;

            m_shortcuts = Owner.Record.Shortcuts;
        }

        public Character Owner
        {
            get;
            private set;
        }

        public void AddShortcut(sbyte barType, DofusShortcut shortcut)
        {
            // do not ask me why i use a sbyte, they are fucking idiots
            if (shortcut is ShortcutSpell && barType == 2)
                AddSpellShortcut(shortcut.slot, ( shortcut as ShortcutSpell ).spellId);
            else if (shortcut is ShortcutObjectItem && barType == 0)
                AddItemShortcut(shortcut.slot, Owner.Inventory.GetItem(( shortcut as ShortcutObjectItem ).itemUID));
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
            shortcut.Save();

            Owner.Record.Shortcuts.Add(shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.SPELL, shortcut);
        }

        public void AddItemShortcut(int slot, Item item)
        {
            if (!IsSlotFree(slot))
                RemoveShortcut(ShortcutBarEnum.OBJECT, slot);

            var shortcut = new ItemShortcut(Owner.Record, slot, item.Template.Id, item.Guid);
            shortcut.Save();

            Owner.Record.Shortcuts.Add(shortcut);
            ShortcutHandler.SendShortcutBarRefreshMessage(Owner.Client, ShortcutBarEnum.OBJECT, shortcut);
        }

        public void SwapShortcuts(ShortcutBarEnum barType, int slot, int newSlot)
        {
            if (IsSlotFree(slot))
                return;

            var shortcutToSwitch = GetShortcut(barType, slot);
            var shortcutDestination = GetShortcut(barType, newSlot);

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
            if (IsSlotFree(slot))
                return;

            m_shortcuts.Remove(GetShortcut(barType, slot));
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
    }
}