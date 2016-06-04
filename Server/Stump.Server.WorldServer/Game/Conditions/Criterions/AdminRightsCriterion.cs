using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AdminRightsCriterion : Criterion
    {
        public const string Identifier = "PX";

        public RoleEnum Role
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare((int)character.UserGroup.Role, (int)Role);
        }

        public override void Build()
        {
            if (Literal == "G")
                Role = RoleEnum.Player; // WTF

            int role;

            if (!int.TryParse(Literal, out role))
                throw new Exception(string.Format("Cannot build AdminRightsCriterion, {0} is not a valid role", Literal));

            Role = (RoleEnum)role;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}