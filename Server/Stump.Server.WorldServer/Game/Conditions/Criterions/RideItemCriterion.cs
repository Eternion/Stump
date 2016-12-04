using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class RideItemCriterion : Criterion
    {
        public const string Identifier = "Pf";

        public int MountId
        {
            get;
            private set;
        }

        public override bool Eval(Character character)
        {
            return character.HasEquippedMount() && Compare(character.EquippedMount.Template.Id, MountId);
        }

        public override void Build()
        {
                if (!int.TryParse(Literal, out var mountId))
                    throw new Exception(string.Format("Cannot build RideItemCriterion, {0} is not a valid mount id", Literal));

                MountId = mountId;
            }

        public override string ToString()
        {
            return FormatToString(Literal);
        }
    }
}