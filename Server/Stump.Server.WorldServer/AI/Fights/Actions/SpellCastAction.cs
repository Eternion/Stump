using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class SpellCastAction : AIAction
    {
        public const int MaxCastLimit = 20;

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
            if (Spell == null)
                return RunStatus.Failure;

            if (Fighter.CanCastSpell(Spell, Target) != SpellCastResult.OK)
                return RunStatus.Failure;

            var i = 0;
            do
            {
                if (!Fighter.CastSpell(Spell, Target))
                    break;

                // avoid infinite loops
                i++;
            } while (MultipleCast && Fighter.CanCastSpell(Spell, Target) == SpellCastResult.OK && i <= MaxCastLimit);

            return RunStatus.Success;
        }
    }
}