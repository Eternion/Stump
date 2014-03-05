using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Matching.Characters
{
    public class CharacterMatching : BaseMatching<Character>
    {
        public CharacterMatching(string pattern)
            : base(pattern)
        {
        }

        public CharacterMatching(string pattern, Character caller)
            : base(pattern, caller)
        {
        }

        protected override string GetName(Character obj)
        {
            return obj.Name;
        }

        protected override IEnumerable<Character> GetSource()
        {
            return World.Instance.GetCharacters();
        }

        protected override BaseCriteria<Character> GetCriteria(string pattern)
        {
            if (pattern.StartsWith("map", StringComparison.InvariantCultureIgnoreCase))
                return new MapCriteria(this, pattern);

            throw new Exception(string.Format("Criterias for string '{0}' not found", pattern));
        }

        protected override Character[] GetByNames(string name)
        {
            var character = World.Instance.GetCharacter(name);

            return character != null ? new []{character} : new Character[0];
        }

        public override Character[] FindMatchs()
        {    
            if (Pattern == "*")
                if (Caller == null)
                    throw new Exception("No caller specified");
                else
                    return new[]{Caller};

            if (Pattern.StartsWith("!"))
            {
                Pattern = Pattern.Remove(0, 1);
                var list = base.FindMatchs().ToList();
                list.Remove(Caller);
                return list.ToArray();
            }

            return base.FindMatchs();
        }
    }
}