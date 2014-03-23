using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class GuildRightsCriterion : Criterion
    {
        public const string Identifier = "Px";

        public int RankId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return character.GuildMember != null && (character.GuildMember.RankId == RankId);
        }

        public override void Build()
        {
            int rank;

            if (!int.TryParse(Literal, out rank))
                throw new Exception(string.Format("Cannot build GuildRightsCriterion, {0} is not a valid rank", Literal));

            RankId = rank;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
