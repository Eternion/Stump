using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class InteractiveStateCriterion : Criterion
    {
        public const string Identifier = "InteractiveState";

        public int ElementId
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            var element = character.Map.GetInteractiveObject(ElementId);
            return element.State != InteractiveStateEnum.STATE_NORMAL;
        }

        public override void Build()
        {
            int elementId;

            if (!int.TryParse(Literal, out elementId))
                throw new Exception(string.Format("Cannot build InteractiveStateCriterion, {0} is not a valid element id", Literal));

            ElementId = elementId;
        }

        public override string ToString() => FormatToString(Identifier);
    }
}
