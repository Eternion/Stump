using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class GuildRightsCriterion : Criterion
    {
        public const string Identifier = "Px";

        public GuildRightsBitEnum Right
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return character.GuildMember != null && character.GuildMember.HasRight(Right);
        }

        public override void Build()
        {
            int right;

            if (!int.TryParse(Literal, out right))
                throw new Exception(string.Format("Cannot build GuildRightsCriterion, {0} is not a valid right", Literal));

            Right = (GuildRightsBitEnum)right;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
