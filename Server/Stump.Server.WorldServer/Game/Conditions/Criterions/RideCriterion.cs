using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class RideCriterion : Criterion
    {
        public const string Identifier = "Pf";

        public int MountId
        {
            get;
            set;
        }

        public override bool Eval(Character character) => character.HasEquippedMount() && character.IsRiding && Compare(character.EquippedMount.Template.Id, MountId);

        public override void Build()
        {
            int mountId;

            if (!int.TryParse(Literal, out mountId))
                throw new Exception(string.Format("Cannot build RideCriterion, {0} is not a valid mount id", Literal));

            MountId = mountId;
        }

        public override string ToString() => FormatToString(Identifier);
    }
}