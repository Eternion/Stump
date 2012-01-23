
using System;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Commands
{
    public static class ParametersConverter
    {
        public static ConverterHandler<T> GetEnumConverter<T>()
            where T : struct
        {
            var type = typeof (T);

            if (!type.IsEnum)
                throw new ConverterException("Cannot convert non-enum type");

            return (entry, trigger) =>
                       {
                           T result;

                           if (Enum.TryParse(entry, CommandBase.IgnoreCommandCase, out result))
                               return result;

                           throw new ConverterException(string.Format("Cannot convert '{0}' to a {1}", entry, type.Name));
                       };
        }

        public static ConverterHandler<Character> CharacterConverter = (entry, trigger) =>
        {
            Character target;

            if (trigger is GameTrigger && (trigger as GameTrigger).Character != null)
                target = World.Instance.GetCharacterByPattern((trigger as GameTrigger).Character, entry);
            else
                target = World.Instance.GetCharacterByPattern(entry);

            if (target == null)
                throw new ConverterException(string.Format("'{0}' is not found or not connected", entry));

            return target;
        };

        public static ConverterHandler<ItemTemplate> ItemTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                ItemTemplate itemById = ItemManager.Instance.GetTemplate(outvalue);

                if (itemById == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

                return itemById;
            }

            ItemTemplate itemByName = ItemManager.Instance.GetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (itemByName == null)
                throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

            return itemByName;
        };


        public static ConverterHandler<NpcTemplate> NpcTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var template = NpcManager.Instance.GetNpcTemplate(outvalue);

                if (template == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid npc template id", entry));

                return template;
            }

            var templateByName = NpcManager.Instance.GetNpcTemplate(entry, CommandBase.IgnoreCommandCase);

            if (templateByName == null)
                throw new ConverterException(string.Format("'{0}' is not a npc template name", entry));

            return templateByName;
        };

        public static ConverterHandler<MonsterTemplate> MonsterTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var template = MonsterManager.Instance.GetTemplate(outvalue);

                if (template == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid monster template id", entry));

                return template;
            }

            var templateByName = MonsterManager.Instance.GetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (templateByName == null)
                throw new ConverterException(string.Format("'{0}' is not a monster template name", entry));

            return templateByName;
        };

        public static ConverterHandler<Area> AreaConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var area = World.Instance.GetArea(outvalue);

                if (area == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid area id", entry));

                return area;
            }

            var areaByName = World.Instance.GetArea(entry);

            if (areaByName == null)
                throw new ConverterException(string.Format("'{0}' is not a area name", entry));

            return areaByName;
        };

        public static ConverterHandler<Map> MapConverter = (entry, trigger) =>
        {
            if (entry.Contains(","))
            {
                var splitted = entry.Split(',');

                if (splitted.Length != 2)
                    throw new ConverterException(string.Format("'{0}' is not of 'mapid' or'x,y'", entry));

                int x = int.Parse(splitted[0].Trim());
                int y = int.Parse(splitted[1].Trim());

                var map = World.Instance.GetMap(x, y);

                if (map == null)
                    throw new ConverterException(string.Format("'x:{0} y:{1}' map not found", x, y));

                return map;
            }

            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var map = World.Instance.GetMap(outvalue);

                if (map == null)
                    throw new ConverterException(string.Format("'{0}' map not found", entry));

                return map;
            }

            throw new ConverterException(string.Format("'{0}' is not of format 'mapid' or 'x,y'", entry));
        };
    }
}