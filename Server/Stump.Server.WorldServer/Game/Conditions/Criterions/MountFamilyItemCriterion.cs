using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class MountFamilyItemCriterion : Criterion
    {
        public const string Identifier = "Of";

        public int FamilyId
        {
            get;
            private set;
        }

        public override bool Eval(Character character)
        {
            return character.HasEquippedMount() && Compare(character.EquippedMount.Template.FamilyId, FamilyId);
        }

        public override void Build()
        {
                int familyId;

                if (!int.TryParse(Literal, out familyId))
                    throw new Exception(string.Format("Cannot build MountFamilyItemCriterion, {0} is not a valid family id", Literal));

                FamilyId = familyId;
            }

        public override string ToString()
        {
            return FormatToString(Literal);
        }
    }
}