using System.Collections;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Spells
{
    public class SpellInventory : IEnumerable<CharacterSpell>
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
            var database = WorldServer.Instance.DBAccessor.Database;

            foreach (var record in database.Query<CharacterSpellRecord>(string.Format(CharacterSpellRelator.FetchByOwner, Owner.Id)))
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

            return template == null ? null : LearnSpell(template);
        }

        public CharacterSpell LearnSpell(SpellTemplate template)
        {
            var record = SpellManager.Instance.CreateSpellRecord(Owner.Record, template);

            var spell = new CharacterSpell(record);
            m_spells.Add(spell.Id, spell);

            InventoryHandler.SendSpellUpgradeSuccessMessage(Owner.Client, spell);

            return spell;
        }

        public bool UnLearnSpell(int id)
        {
            var spell = GetSpell(id);

            if (spell == null)
                return true;

            m_spells.Remove(id);
            m_spellsToDelete.Enqueue(spell.Record);

            if (spell.CurrentLevel > 1)
            {
                var resetPoints = 0;
                for (var i = 1; i < spell.CurrentLevel; i++)
                {
                    resetPoints += i;
                }
                Owner.SpellsPoints += (ushort)resetPoints;
            }

            InventoryHandler.SendSpellListMessage(Owner.Client, true);
            return true;
        }

        public bool UnLearnSpell(CharacterSpell spell)
        {
            return UnLearnSpell(spell.Id);
        }

        public bool UnLearnSpell(SpellTemplate spell)
        {
            return UnLearnSpell(spell.Id);
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

        public bool ForgetSpell(SpellTemplate spell)
        {
            return ForgetSpell(spell.Id);
        }

        public bool ForgetSpell(int id)
        {
            if (!HasSpell(id))
                return false;

            var spell = GetSpell(id);

            return ForgetSpell(spell);
        }

        public bool ForgetSpell(CharacterSpell spell)
        {
            if (!HasSpell(spell.Id))
                return false;

            var resetPoints = 0;
            for (var i = 1; i < spell.CurrentLevel; i++)
            {
                resetPoints += i;
            }

            spell.CurrentLevel = 1;
            Owner.SpellsPoints += (ushort)resetPoints;

            InventoryHandler.SendSpellListMessage(Owner.Client, true); 
            return true;
        }

        public int ForgetAllSpells()
        {
            var resetPoints = 0;

            foreach (var spell in m_spells)
            {
                for (var i = 1; i < spell.Value.CurrentLevel; i++)
                {
                    resetPoints += i;
                }

                spell.Value.CurrentLevel = 1;
            }

            Owner.SpellsPoints += (ushort)resetPoints;

            InventoryHandler.SendSpellListMessage(Owner.Client, true);
            Owner.RefreshStats();
            return resetPoints;
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
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var characterSpell in m_spells)
                {
                    database.Save(characterSpell.Value.Record);
                }

                while (m_spellsToDelete.Count > 0)
                {
                    var record = m_spellsToDelete.Dequeue();

                    database.Delete(record);
                }
            }
        }

        public IEnumerator<CharacterSpell> GetEnumerator()
        {
            return m_spells.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}