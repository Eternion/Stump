using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class SpellCastAction : AIAction
    {
        public SpellCastAction(AIFighter fighter, Spell spell, Cell target)
            : base(fighter)
        {
            Spell = spell;
            Target = target;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public Cell Target
        {
            get;
            private set;
        }

        public override void Execute()
        {
            Fighter.CastSpell(Spell, Target);
        }
    }
}