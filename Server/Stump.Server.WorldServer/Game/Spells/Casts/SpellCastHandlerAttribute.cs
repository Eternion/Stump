using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SpellCastHandlerAttribute : Attribute
    {
        public SpellCastHandlerAttribute(int spellId)
        {
            Spell = spellId;            
        }

        public SpellCastHandlerAttribute(SpellIdEnum spellId)
        {
            Spell = (int) spellId;
        }

        public int Spell
        {
            get;
            set;
        }
    }
}