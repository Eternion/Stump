using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AlignmentCriterion : Criterion
    {
        public const string Identifier = "Ps";

        public AlignmentSideEnum Alignement
        {
            get;
            set;
        }

        public override bool Eval(Character character) => Compare((int)character.AlignmentSide, (int)Alignement);

        public override void Build()
        {
            int id;

            if (!int.TryParse(Literal, out id))
                throw new Exception(string.Format("Cannot build AlignmentCriterion, {0} is not a valid alignement id", Literal));

            Alignement = (AlignmentSideEnum)id;
        }

        public override string ToString() => FormatToString(Identifier);
    }
}