using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;

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

        public SpellCastAction(AIFighter fighter, Spell spell, Cell target, bool multipleCast)
            : base(fighter)
        {
            Spell = spell;
            Target = target;
            MultipleCast = multipleCast;
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

        public bool MultipleCast
        {
            get;
            set;
        }

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanCastSpell(Spell, Target))
                return RunStatus.Failure;
            do
            {
                Fighter.CastSpell(Spell, Target);
            } while (MultipleCast && Fighter.CanCastSpell(Spell, Target));

            return RunStatus.Success;
        }
    }
}