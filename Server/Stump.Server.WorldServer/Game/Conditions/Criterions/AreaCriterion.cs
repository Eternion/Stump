using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AreaCriterion : Criterion
    {
        public const string Identifier = "Po";

        public int? AreaId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(AreaId, character.Area.Id);
        }

        public override void Build()
        {
            int areaId;

            if (!int.TryParse(Literal, out areaId))
                throw new Exception(string.Format("Cannot build AreaCriterion, {0} is not a valid area id", Literal));

            AreaId = areaId;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
