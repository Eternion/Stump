using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class CellOccupedCriterion : Criterion
    {
        public const string Identifier = "CellOccuped";

        public int? CellId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return character.Map.GetActor<Character>(x => x.Cell.Id == CellId) != null;
        }

        public override void Build()
        {
            int cellId;

            if (!int.TryParse(Literal, out cellId))
                throw new Exception(string.Format("Cannot build CellOccupedCriterion, {0} is not a valid cell id", Literal));

            CellId = cellId;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
