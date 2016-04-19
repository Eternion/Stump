using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
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

            foreach (var spell in database.Query<CharacterSpellRecord>(string.Format(CharacterSpellRelator.FetchByOwner, Owner.Id)).Select(record => new CharacterSpell(record)))
            {
                m_spells.Add(spell.Id, spell);
            }
        }

        public CharacterSpell GetSpell(int id)
        {
            CharacterSpell spell;
            return m_spells.TryGetValue(id, out spell) ? spell : null;
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

            //ContextRoleplayHandler.SendSpellUpgradeSuccessMessage(Owner.Client, spell);

            return spell;
        }

        public bool UnLearnSpell(int id)
        {
            var spell = GetSpell(id);

            if (spell == null)
                return true;

            m_spells.Remove(id);
            m_spellsToDelete.Enqueue(spell.Record);

            Owner.SpellsPoints += (ushort)CalculateSpellPoints(spell.CurrentLevel);

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

        public int CalculateSpellPoints(int level, int currentLevel = 1)
        {
            var spentPoints = 0;
            if (currentLevel > 1)
                spentPoints = CalculateSpellPoints(currentLevel);

            return ((level * (level - 1)) / 2) - spentPoints;
        }

        public bool CanBoostSpell(Spell spell, ushort level, bool send = true)
        {
            if (Owner.IsFighting())
            {
                if (send)
                    //ContextRoleplayHandler.SendSpellUpgradeFailureMessage(Owner.Client);

                return false;
            }

            if (spell.CurrentLevel >= level || level > 6)
            {
                if (send)
                    //ContextRoleplayHandler.SendSpellUpgradeFailureMessage(Owner.Client);

                return false;
            }

            if (Owner.SpellsPoints < CalculateSpellPoints(level, spell.CurrentLevel))
            {
                if (send)
                    //ContextRoleplayHandler.SendSpellUpgradeFailureMessage(Owner.Client);

                return false;
            }

            if (spell.ByLevel[level].MinPlayerLevel > Owner.Level)
            {
                if (send)
                    //ContextRoleplayHandler.SendSpellUpgradeFailureMessage(Owner.Client);

                return false;
            }

            return true;
        }

        public bool BoostSpell(int id, ushort level)
        {
            var spell = GetSpell(id);

            if (spell == null)
            {
                //ContextRoleplayHandler.SendSpellUpgradeFailureMessage(Owner.Client);
                return false;
            }

            if (!CanBoostSpell(spell, level))
                return false;

            Owner.SpellsPoints -= (ushort)CalculateSpellPoints(level, spell.CurrentLevel);
            spell.CurrentLevel = (byte)level;

            //ContextRoleplayHandler.SendSpellUpgradeSuccessMessage(Owner.Client, spell);

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

            var level = spell.CurrentLevel;
            for (var i = 1; i < level; i++)
            {
                DowngradeSpell(spell, false);
            }

            InventoryHandler.SendSpellListMessage(Owner.Client, true); 
            return true;
        }

        public void ForgetAllSpells()
        {
            foreach (var spell in m_spells)
            {
                var level = spell.Value.CurrentLevel;
                for (var i = 1; i < level; i++)
                {
                    DowngradeSpell(spell.Value, false);
                }
            }

            InventoryHandler.SendSpellListMessage(Owner.Client, true);
            Owner.RefreshStats();
        }

        public int DowngradeSpell(SpellTemplate spell)
        {
            return DowngradeSpell(spell.Id);
        }

        public int DowngradeSpell(int id)
        {
            if (!HasSpell(id))
                return 0;

            var spell = GetSpell(id);

            return DowngradeSpell(spell);
        }

        public int DowngradeSpell(CharacterSpell spell, bool send = true)
        {
            if (!HasSpell(spell.Id))
                return 0;

            if (spell.CurrentLevel <= 1)
                return 0;

            spell.CurrentLevel -= 1;
            Owner.SpellsPoints += spell.CurrentLevel;

            if (!send)
                return spell.CurrentLevel;

            InventoryHandler.SendSpellListMessage(Owner.Client, true);
            //ContextRoleplayHandler.SendSpellForgottenMessage(Owner.Client, spell, spell.CurrentLevel);

            Owner.RefreshStats();

            return spell.CurrentLevel;
        }

        public void MoveSpell(int id, byte position)
        {
            var spell = GetSpell(id);

            if (spell == null)
                return;

            Owner.Shortcuts.AddSpellShortcut(position, (short) id);
        }

        public int CountSpentBoostPoint()
        {
            var count = 0;
            foreach (var spell in this)
            {
                for (var i = 1; i < spell.CurrentLevel; i++)
                {
                    count += i;
                }
            }

            return count;
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