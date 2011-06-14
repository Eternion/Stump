
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Classes.Extensions;
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
            Character target;

            if (trigger is IInGameTrigger && (trigger as IInGameTrigger).Character != null)
                target = World.Instance.GetCharacterByPattern((trigger as IInGameTrigger).Character, entry);
            else
                target = World.Instance.GetCharacterByPattern(entry);

            if (target == null)
                throw new ConverterException(string.Format("'{0}' is not found or not connected", entry));

            return target;
        };

        public static Func<string, TriggerBase, ItemTemplate> ItemTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                ItemTemplate itemById = ItemManager.GetTemplate(outvalue);

                if (itemById == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

                return itemById;
            }

            ItemTemplate itemByName = ItemManager.GetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (itemByName == null)
                throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

            return itemByName;
        };
    }
}