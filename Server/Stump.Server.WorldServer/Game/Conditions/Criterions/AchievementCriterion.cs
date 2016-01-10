using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AchievementCriterion : Criterion
    {
        public const string Identifier = "OA";

        public override void Build()
        {
        }

        public override bool Eval(Character character)
        {
            return true;
        }
    }
}
