using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class StateCriterion : TargetCriterion
    {
        public StateCriterion(int state, bool caster, bool required)
        {
            State = state;
            Caster = caster;
            Required = required;
        }

        public int State
        {
            get;
            set;
        }

        public bool Caster
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            
        }
    }
}
