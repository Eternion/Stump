using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class EmoteCriterion : Criterion
    {
        public const string Identifier = "PE";

        public int Emote
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            // todo
            return true;
        }

        public override void Build()
        {
            int emote;

            if (!int.TryParse(Literal, out emote))
                throw new Exception(string.Format("Cannot build EmoteCriterion, {0} is not a valid emote id", Literal));

            Emote = emote;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        } 
    }
}