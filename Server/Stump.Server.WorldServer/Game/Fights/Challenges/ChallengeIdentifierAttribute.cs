using System;

namespace Stump.Server.WorldServer.Game.Fights.Challenges
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited = false)]
    public class ChallengeIdentifierAttribute : Attribute
    {
        public ChallengeIdentifierAttribute(params int[] identifiers)
        {
            Identifiers = identifiers;
        }

        public int[] Identifiers
        {
            get;
            set;
        }
    }
}
