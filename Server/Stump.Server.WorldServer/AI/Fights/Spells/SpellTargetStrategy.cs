using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public abstract class SpellTargetStrategy
    {
        public Spell Spell
        {
            get;
            private set;
        }

        public SpellTargetStrategy(Spell spell)
        {
            Spell = spell;
        }

        public abstract AISpellCastPossibility FindBestCast();
    }
}