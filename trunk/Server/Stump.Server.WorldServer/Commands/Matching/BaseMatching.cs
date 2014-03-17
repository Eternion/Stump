using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Matching
{
    public abstract class BaseMatching<T>
    {
        public BaseMatching(string pattern)
        {
            Pattern = pattern;
        }

        public BaseMatching(string pattern, Character caller)
            : this (pattern)
        {
            Caller = caller;
        }

        public string Pattern
        {
            get;
            protected set;
        }

        public Character Caller
        {
            get;
            private set;
        }

        protected abstract string GetName(T obj);
        protected abstract IEnumerable<T> GetSource();
        protected abstract BaseCriteria<T> GetCriteria(string pattern);

        protected virtual T[] GetByNames(string name)
        {
            return GetSource()
                    .Where(x => GetName(x).Equals(Pattern, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
        }

        public virtual T[] FindMatchs()
        {
            var matches = new List<T>();
            if (Pattern.StartsWith("{"))
            {
                if (!Pattern.EndsWith("}"))
                    throw new Exception("Unexcepted token. Enters special criterias between { }");

                var criteriasStr = Pattern.Substring(1, Pattern.Length - 2);
                var split = criteriasStr.SplitAdvanced("&&", "\"");

                foreach (var criteriaStr in split)
                {
                    matches.AddRange(GetCriteria(criteriasStr).GetMatchings());
                }

                return matches.ToArray();
            }
            else
            {
                string name;
                if (Pattern.StartsWith("*"))
                {
                    if (Pattern.EndsWith("*"))
                    {
                        name = Pattern.Substring(1, Pattern.Length - 2);
                        return GetSource()
                            .Where(x => GetName(x).IndexOf(name, StringComparison.InvariantCultureIgnoreCase) != -1)
                            .ToArray();
                    }

                    name = Pattern.Substring(1, Pattern.Length - 1);
                    return GetSource()
                        .Where(x => GetName(x).EndsWith(name, StringComparison.InvariantCultureIgnoreCase))
                        .ToArray();
                }
                else if (Pattern.EndsWith("*"))
                {
                    name = Pattern.Substring(0, Pattern.Length - 1);
                    return GetSource()
                        .Where(x => GetName(x).StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                        .ToArray();
                }

                return GetByNames(Pattern);
            }
        }
    }
}