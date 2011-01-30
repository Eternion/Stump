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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Handlers;


namespace Stump.Server.WorldServer.Entities
{
    public static class CharacterExtension
    {

        #region Send
        
        public static void Send(this Character character, Action<Character> action)
        {
            action(character);
        }

        public static void Send(Character character, Action<Character> action, GameContextEnum context)
        {
            if (character.Context == context)
                action(character);
        }

        public static void Send(this Character character, Action<Character> action, Predicate<Character> predicate)
        {
            if (predicate(character))
                action(character);
        }

        public static void Send(this IEnumerable<Character> characters, Action<Character> action)
        {
            foreach (var character in characters)
                action(character);
        }

        public static void Send(this IEnumerable<Character> characters, Action<Character> action, GameContextEnum context)
        {
            foreach (var character in characters.Where(c => c.Context == context))
                action(character);
        }

        public static void Send(this IEnumerable<Character> characters, Action<Character> action, Predicate<Character> predicate)
        {
            foreach (var character in characters.Where(c => predicate(c)))
                action(character);
        }

        public static void Send(this IEnumerable<KeyValuePair<long, Character>> characters, Action<Character> action)
        {
            foreach (var key in characters)
                action(key.Value);
        }

        public static void Send(this IEnumerable<KeyValuePair<long, Character>> characters, Action<Character> action, GameContextEnum context)
        {
            foreach (var key in characters.Where(c => c.Value.Context == context))
                action(key.Value);
        }

        public static void Send(this IEnumerable<KeyValuePair<long, Character>> characters, Action<Character> action, Predicate<Character> predicate)
        {
            foreach (var key in characters.Where(c => predicate(c.Value)))
                action(key.Value);
        }

        #endregion

        #region Get

        public static void Get(this IEnumerable<Character> characters, string name)
        {
            foreach (var character in characters)
                action(character);
        }

        public static void Get(this IEnumerable<Character> characters, GameContextEnum context)
        {
            return characters.Where(c => c.Context == context);
        }

        #endregion
    }
}