using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class MapCharactersCriterion : Criterion
    {
        public const string Identifier = "MK";

        public int? MapId
        {
            get;
            set;
        }

        public int CharactersCount
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            var mapCharsCount = character.Map.Clients.Count;

            if (MapId != null)
                return Compare(character.Map.Id, MapId.Value) && Compare(mapCharsCount, CharactersCount);

            return Compare(mapCharsCount, CharactersCount);
        }

        public override void Build()
        {
            var split = Literal.Split(',');

            if (split.Length == 1)
            {
                MapId = null;

                int count;
                if (!int.TryParse(Literal, out count))
                    throw new Exception(string.Format("Cannot build MapCharactersCriterion, {0} is not a valid characters count", Literal));

                CharactersCount = count;
            }
            else if (split.Length == 2)
            {

                int mapId;
                if (!int.TryParse(split[0], out mapId))
                    throw new Exception(string.Format("Cannot build MapCharactersCriterion, {0} is not a valid map id", split[0]));

                MapId = mapId;

                int count;
                if (!int.TryParse(split[1], out count))
                    throw new Exception(string.Format("Cannot build MapCharactersCriterion, {0} is not a valid characters count", split[1]));

                CharactersCount = count;
            }
            else
            {
                throw new Exception(string.Format("Cannot build MapCharactersCriterion, {0} is mal formatted : 'id,count' or 'count'", split[1]));
            }
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}