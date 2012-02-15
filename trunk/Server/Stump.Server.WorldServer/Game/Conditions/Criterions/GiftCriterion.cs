using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class GiftCriterion : Criterion
    {
        public const string Identifier = "Pg";

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
            // todo
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
                    throw new Exception(string.Format("Cannot build GiftCriterion, {0} is not a valid gift (format 'id,level')", Literal));

            }

            if (!int.TryParse(Literal, out id))
                throw new Exception(string.Format("Cannot build GiftCriterion, {0} is not a valid gift id", Literal));

            Id = id;
            Level = level;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}