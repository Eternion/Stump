using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class BonesCriterion : Criterion
    {
        public const string Identifier = "PU";

        public int BonesId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.Look.bonesId, BonesId);
        }

        public override void Build()
        {
            if (Literal == "B")
                BonesId = 1;
            else
            {
                int bonesId;

                if (!int.TryParse(Literal, out bonesId))
                    throw new Exception(string.Format("Cannot build BonesCriterion, {0} is not a valid bones id", Literal));

                BonesId = bonesId != 0 ? bonesId : 1;
            }
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        } 
    }
}