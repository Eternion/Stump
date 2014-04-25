using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SubscribeCriterion : Criterion
    {
        public const string Identifier = "PZ";

        public bool SubscriptionState
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return true;
        }

        public override void Build()
        {
            int state;

            if (!int.TryParse(Literal, out state))
                throw new Exception(string.Format("Cannot build SubscribeCriterion, {0} is not a valid subscription state (1 or 0)", Literal));

            SubscriptionState = state != 0;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}