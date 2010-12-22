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
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Commands
{
    public static class ParametersConverter
    {
        public static Func<string, TriggerBase, Character> CharacterConverter = (entry, trigger) =>
        {
            if (trigger is IInGameTrigger && (trigger as IInGameTrigger).Character != null)
                return World.Instance.GetCharacterByPattern((trigger as IInGameTrigger).Character, entry);

            return World.Instance.GetCharacterByPattern(entry);
        };

        public static Func<string, TriggerBase, ItemTemplate> ItemTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var itemById = ItemManager.GetTemplate(outvalue);

                if (itemById == null)
                    throw new Exception(string.Format("'{0}' is not a valid item", entry));

                return itemById;
            }

            var itemByName = ItemManager.GetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (itemByName == null)
                throw new Exception(string.Format("'{0}' is not a valid item", entry));

            return itemByName;
        };
    }
}