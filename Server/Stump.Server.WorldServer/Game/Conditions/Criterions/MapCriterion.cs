using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class MapCriterion : Criterion
    {
        public const string Identifier = "Mp";

        public int? MapId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(MapId, character.Map.Id);
        }

        public override void Build()
        {
            int mapId;

            if (!int.TryParse(Literal, out mapId))
                throw new Exception(string.Format("Cannot build MapCriterion, {0} is not a valid map id", Literal));

            MapId = mapId;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
