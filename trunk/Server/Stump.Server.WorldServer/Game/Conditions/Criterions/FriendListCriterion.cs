using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class FriendListCriterion : Criterion
    {
        public const string Identifier = "Pb";

        public int Friend
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
            int friendId;

            if (!int.TryParse(Literal, out friendId))
                throw new Exception(string.Format("Cannot build FriendListCriterion, {0} is not a valid friend id", Literal));

            Friend = friendId;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        } 
    }
}