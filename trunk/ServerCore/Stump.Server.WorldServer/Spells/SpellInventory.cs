
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Spells
{
    public class SpellInventory
    {
        #region Fields

        #endregion

        #region Events

        public delegate void SpellEventHandler(SpellInventory sender, Spell addedSpell);

        public event SpellEventHandler SpellAdded;

        public void NotifySpellAdded(Spell addedspell)
        {
            SpellEventHandler handler = SpellAdded;

            if (handler != null)
                handler(this, addedspell);
        }

        public event SpellEventHandler SpellRemoved;

        public void NotifySpellRemoved(Spell removedSpell)
        {
            SpellEventHandler handler = SpellRemoved;

            if (handler != null)
                handler(this, removedSpell);
        }

        #endregion


        public SpellInventory(ISpellsOwner owner)
            : this(owner, true)
        {
        }

        protected SpellInventory(ISpellsOwner owner, bool initDictionary)
        {
            if (initDictionary)
                SpellsById = new Dictionary<uint, Spell>(60);

            Owner = owner;

            SpellAdded += OnAdd;
        }

        #region Properties

        public ISpellsOwner Owner
        {
            get;
            protected internal set;
        }

        /// <summary>
        ///   The amount of Spells in this Collection
        /// </summary>
        public int Count
        {
            get { return SpellsById.Count; }
        }

        /// <summary>
        ///   Determines if there are spells in this collection. (At least one)
        /// </summary>
        public bool HasSpells
        {
            get { return SpellsById.Count > 0; }
        }

        public Dictionary<uint, Spell> SpellsById
        {
            get;
            internal set;
        }

        #endregion

        public Spell this[SpellIdEnum id]
        {
            get
            {
                Spell spell;
                SpellsById.TryGetValue((uint) id, out spell);
                return spell;
            }
        }

        public Spell this[uint id]
        {
            get
            {
                Spell spell;
                SpellsById.TryGetValue(id, out spell);
                return spell;
            }
        }

        /// <summary>
        ///   Teaches a new spell to the character.
        /// </summary>
        public void AddSpell(uint spellId)
        {
            Spell spell = SpellManager.GetSpell(spellId);
            AddSpell(spell);
        }

        /// <summary>
        ///   Teaches a new spell to the character.
        /// </summary>
        public void AddSpell(SpellIdEnum spellId)
        {
            Spell spell = SpellManager.GetSpell((uint) spellId);
            AddSpell(spell);
        }

        /// <summary>
        ///   Teaches new spells to the character.
        /// </summary>
        public void AddSpells(Spell[] spells)
        {
            for (int i = 0; i < spells.Length; i++)
            {
                AddSpell(spells[i]);
            }
        }

        /// <summary>
        ///   Teaches a new spell to the character.
        /// </summary>
        public void AddSpell(Spell spell)
        {
            SpellsById[(uint) spell.Id] = spell;

            NotifySpellAdded(spell);
        }

        /// <summary>
        ///   Happens when a spell has been added to collection.
        /// </summary>
        private void OnAdd(SpellInventory sender, Spell addedSpell)
        {
        }

        /// <summary>
        ///   Add a range of spells to this character.
        /// </summary>
        public void AddSpell(IEnumerable<SpellIdEnum> spells)
        {
            foreach (SpellIdEnum spell in spells)
            {
                AddSpell(spell);
            }
        }

        /// <summary>
        ///   Add a range of spells to this character.
        /// </summary>
        public void AddSpell(params SpellIdEnum[] spells)
        {
            foreach (SpellIdEnum spell in spells)
            {
                AddSpell(spell);
            }
        }

        /// <summary>
        ///   Add a range of spells to this character.
        /// </summary>
        public void AddSpell(IEnumerable<Spell> spells)
        {
            foreach (Spell spell in spells)
            {
                AddSpell(spell);
            }
        }

        /// <summary>
        ///   Check if collection contains given spell id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        public bool Contains(uint id)
        {
            return SpellsById.ContainsKey(id);
        }

        /// <summary>
        ///   Check if collection contains given spell id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        public bool Contains(SpellIdEnum id)
        {
            return SpellsById.ContainsKey((uint) id);
        }


        public void Remove(SpellIdEnum spellId)
        {
            Replace(SpellManager.GetSpell(spellId), null);
        }

        public bool Remove(uint spellId)
        {
            Remove((SpellIdEnum) spellId);
            return true;
        }

        public void Remove(Spell spell)
        {
            Replace(spell, null);

            NotifySpellRemoved(spell);
        }

        /// <summary>
        ///   Clear Spells Collections. Empty the entire collection.
        /// </summary>
        public void Clear()
        {
            SpellsById.Clear();
        }

        /// <summary>
        ///   Only works if you have 2 valid spell ids and oldSpellId already exists.
        /// </summary>
        public void Replace(SpellIdEnum oldSpellId, SpellIdEnum newSpellId)
        {
            Spell oldSpell, newSpell = SpellManager.GetSpell(newSpellId);
            if (SpellsById.TryGetValue((uint) oldSpellId, out oldSpell))
            {
                Replace(oldSpell, newSpell);
            }
        }

        /// <summary>
        ///   Replaces or (if newSpell == null) removes oldSpell; does nothing if oldSpell doesn't exist.
        /// </summary>
        public void Replace(Spell oldSpell, Spell newSpell)
        {
            var oldspellid = (uint) oldSpell.Id;
            SpellsById.Remove(oldspellid);

            if (newSpell != null)
            {
                AddSpell(newSpell);
            }
        }

        public void MoveSpell(SpellIdEnum spellid, int newPos)
        {
            if (SpellsById.ContainsKey((uint) spellid))
                SpellsById[(uint) spellid].Position = newPos;
        }

        public IEnumerator<Spell> GetEnumerator()
        {
            return ((IEnumerable<Spell>) SpellsById.Values).GetEnumerator();
        }
    }
}