using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Spells
{
    public class SpellInventory
    {
        private readonly Dictionary<int, CharacterSpell> m_spells = new Dictionary<int, CharacterSpell>();
        private readonly Queue<CharacterSpellRecord> m_spellsToDelete = new Queue<CharacterSpellRecord>();
        private readonly object m_locker = new object();

        public SpellInventory(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            private set;
        }

        internal void LoadSpells()
        {
            var records = CharacterSpellRecord.FindAllByOwner(Owner.Id);

            foreach (var record in records)
            {
                var spell = new CharacterSpell(record);

                m_spells.Add(spell.Id, spell);
            }
        }

        public CharacterSpell GetSpell(int id)
        {
            CharacterSpell spell;
            if (m_spells.TryGetValue(id, out spell))
                return spell;

            return null;
        }

        public bool HasSpell(int id)
        {
            return m_spells.ContainsKey(id);
        }

        public bool HasSpell(CharacterSpell spell)
        {
            return m_spells.ContainsKey(spell.Id);
        }

        public IEnumerable<CharacterSpell> GetSpells()
        {
            return m_spells.Values; 
        }

        public CharacterSpell LearnSpell(int id)
        {
            var template = SpellManager.Instance.GetSpellTemplate(id);

            if (template == null)
                return null;

            var record = SpellManager.Instance.CreateSpellRecord(Owner.Record, template);

            var spell = new CharacterSpell(record);
            m_spells.Add(spell.Id, spell);

            InventoryHandler.SendSpellUpgradeSuccessMessage(Owner.Client, spell);

            return spell;
        }

        public void UnLearnSpell(int id)
        {
            var spell = GetSpell(id);

            if (spell == null)
                return;

            m_spells.Remove(id);
            m_spellsToDelete.Enqueue(spell.Record);

            if (spell.CurrentLevel > 1)
                Owner.SpellsPoints += (ushort)(spell.CurrentLevel - 1);

            InventoryHandler.SendSpellUpgradeSuccessMessage(Owner.Client, id, 0);
        }

        public bool CanBoostSpell(Spell spell, bool send = true)
        {
            if (Owner.IsFighting())
            {
                if (send)
                    InventoryHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            if (spell.CurrentLevel >= 6)
            {
                if (send)
                    InventoryHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            if (Owner.SpellsPoints < spell.CurrentLevel)
            {
                if (send)
                    InventoryHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            if (spell.ByLevel[spell.CurrentLevel + 1].MinPlayerLevel > Owner.Level)
            {
                if (send)
                    InventoryHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            return true;
        }

        public bool BoostSpell(int id)
        {
            var spell = GetSpell(id);

            if (spell == null)
            {
                InventoryHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            if (!CanBoostSpell(spell))
                return false;

            Owner.SpellsPoints -= (ushort)spell.CurrentLevel;
            spell.CurrentLevel++;

            InventoryHandler.SendSpellUpgradeSuccessMessage(Owner.Client, spell);

            return true;
        }

        public void MoveSpell(int id, byte position)
        {
            var spell = GetSpell(id);

            if (spell == null)
                return;

            Owner.Shortcuts.AddSpellShortcut(position, (short) id);
        }

        public void Save()
        {
            lock (m_locker)
            {
                foreach (var characterSpell in m_spells)
                {
                    characterSpell.Value.Record.Save();
                }

                while (m_spellsToDelete.Count > 0)
                {
                    var record = m_spellsToDelete.Dequeue();

                    record.Delete();
                }
            }
        }
    }
}