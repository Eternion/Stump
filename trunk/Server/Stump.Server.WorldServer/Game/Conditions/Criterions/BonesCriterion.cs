using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class BonesCriterion : Criterion
    {
        public const string Identifier = "PU";

        public short BonesId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.Look.BonesID, BonesId);
        }

        public override void Build()
        {
            if (Literal == "B")
                BonesId = 1;
            else
            {
                short bonesId;

                if (!short.TryParse(Literal, out bonesId))
                    throw new Exception(string.Format("Cannot build BonesCriterion, {0} is not a valid bones id", Literal));

                BonesId = (short) (bonesId != 0 ? bonesId : 1);
            }
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        } 
    }
}