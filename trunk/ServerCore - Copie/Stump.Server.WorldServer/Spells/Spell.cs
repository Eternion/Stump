
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Spells
{
    public class Spell
    {
        #region Properties

        public SpellIdEnum Id
        {
            get;
            set;
        }

        public SpellTypeEnum SpellType
        {
            get;
            set;
        }

        public int CurrentLevel
        {
            get;
            set;
        }

        public SpellLevel CurrentSpellLevel
        {
            get
            {
                return !ByLevel.ContainsKey(CurrentLevel) ? ByLevel[1] : ByLevel[CurrentLevel];
            }
        }

        public int Position
        {
            get;
            set;
        }

        public Dictionary<int, SpellLevel> ByLevel
        {
            get;
            set;
        }

        #endregion

        public Spell()
        {
            ByLevel = new Dictionary<int, SpellLevel>();
        }

        public SpellItem ToNetworkSpell()
        {
            return new SpellItem((uint) Position, (int) Id, CurrentLevel);
        }
    }
}