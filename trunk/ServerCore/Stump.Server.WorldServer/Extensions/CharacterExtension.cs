
using System;
using System.Linq;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.World.Extensions
{
    public static class CharacterExtension
    {

        public static void Do(this Entities.Characters.Character character, Action<Entities.Characters.Character> action)
        {
            action(character);
        }

        public static void Do(this Entities.Characters.Character character, Action<Entities.Characters.Character> action, GameContextEnum context)
        {
            if (character.Context == context)
                action(character);
        }

        public static void Do(this Entities.Characters.Character character, Action<Entities.Characters.Character> action, Predicate<Entities.Characters.Character> predicate)
        {
            if (predicate(character))
                action(character);
        }

        public static void Do(this IEnumerable<Entities.Characters.Character> characters, Action<Entities.Characters.Character> action)
        {
            foreach (var character in characters)
                action(character);
        }

        public static void Do(this IEnumerable<Entities.Characters.Character> characters, Action<Entities.Characters.Character> action, Predicate<Entities.Characters.Character> predicate)
        {
            foreach (var character in characters.Where(c => predicate(c)))
                action(character);
        }

        public static void Do(this IEnumerable<KeyValuePair<long, Entities.Characters.Character>> characters, Action<Entities.Characters.Character> action)
        {
            foreach (var key in characters)
                action(key.Value);
        }

        public static void Do(this IEnumerable<KeyValuePair<long, Entities.Characters.Character>> characters, Action<Entities.Characters.Character> action, Predicate<Entities.Characters.Character> predicate)
        {
            foreach (var key in characters.Where(c => predicate(c.Value)))
                action(key.Value);
        }

    }
}