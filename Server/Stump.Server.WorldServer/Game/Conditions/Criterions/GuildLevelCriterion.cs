using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class GuildLevelCriterion : Criterion
    {
        public const string Identifier = "Py";

        public int Level
        {
            get;
            set;
        }

        public override bool Eval(Character character) => character.Guild != null && character.Guild.Level > Level;

        public override void Build()
        {
            int level;

            if (!int.TryParse(Literal, out level))
                throw new Exception(string.Format("Cannot build GuildLevelCriterion, {0} is not a valid level", Literal));

            Level = level;
        }

        public override string ToString() => FormatToString(Identifier);
    }
}