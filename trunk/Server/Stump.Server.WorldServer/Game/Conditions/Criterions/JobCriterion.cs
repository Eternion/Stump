using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class JobCriterion : Criterion
    {
        public const string Identifier = "PJ";
        public const string Identifier2 = "Pj";

        public int Id
        {
            get;
            set;
        }

        public int Level
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
            int id;
            int level = -1;

            if (Literal.Contains(","))
            {
                var split = Literal.Split(',');

                if (split.Length != 2 ||
                    !int.TryParse(split[0], out id) ||
                    !int.TryParse(split[1], out level))
                    throw new Exception(string.Format("Cannot build JobCriterion, {0} is not a valid job (format 'id,level')", Literal));
            }

            if (!int.TryParse(Literal, out id))
                throw new Exception(string.Format("Cannot build JobCriterion, {0} is not a valid job id", Literal));

            Id = id;
            Level = level;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}