// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.World.Actors.Extensions
{
    public static class CharacterExtension
    {

        #region Send
        
        public static void Send(this Character.Character character, Action<Character.Character> action)
        {
            action(character);
        }

        public static void Send(Character.Character character, Action<Character.Character> action, GameContextEnum context)
        {
            if (character.Context == context)
                action(character);
        }

        public static void Send(this Character.Character character, Action<Character.Character> action, Predicate<Character.Character> predicate)
        {
            if (predicate(character))
                action(character);
        }

        public static void Send(this IEnumerable<Character.Character> characters, Action<Character.Character> action)
        {
            foreach (var character in characters)
                action(character);
        }

        public static void Send(this IEnumerable<Character.Character> characters, Action<Character.Character> action, Predicate<Character.Character> predicate)
        {
            foreach (var character in characters.Where(c => predicate(c)))
                action(character);
        }

        public static void Send(this IEnumerable<KeyValuePair<long, Character.Character>> characters, Action<Character.Character> action)
        {
            foreach (var key in characters)
                action(key.Value);
        }

        public static void Send(this IEnumerable<KeyValuePair<long, Character.Character>> characters, Action<Character.Character> action, Predicate<Character.Character> predicate)
        {
            foreach (var key in characters.Where(c => predicate(c.Value)))
                action(key.Value);
        }

        #endregion

    }
}