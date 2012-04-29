
using System;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Spells;

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
                ItemTemplate itemById = ItemManager.Instance.TryGetTemplate(outvalue);

                if (itemById == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

                return itemById;
            }

            ItemTemplate itemByName = ItemManager.Instance.TryGetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (itemByName == null)
                throw new ConverterException(string.Format("'{0}' is not a valid item", entry));

            return itemByName;
        };

        public static ConverterHandler<ItemSetTemplate> ItemSetTemplateConverter = (entry, trigger) =>
        {
            uint outvalue;
            if (uint.TryParse(entry, out outvalue))
            {
                ItemSetTemplate itemById = ItemManager.Instance.TryGetItemSetTemplate(outvalue);

                if (itemById == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid item set", entry));

                return itemById;
            }

            ItemSetTemplate itemByName = ItemManager.Instance.TryGetItemSetTemplate(entry, CommandBase.IgnoreCommandCase);

            if (itemByName == null)
                throw new ConverterException(string.Format("'{0}' is not a valid item set", entry));

            return itemByName;
        };

        public static ConverterHandler<SpellTemplate> SpellTemplateConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                SpellTemplate spellById = SpellManager.Instance.GetSpellTemplate(outvalue);

                if (spellById == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid spell", entry));

                return spellById;
            }

            SpellTemplate spellByName = SpellManager.Instance.GetSpellTemplate(entry, CommandBase.IgnoreCommandCase);

            if (spellByName == null)
                throw new ConverterException(string.Format("'{0}' is not a valid spell", entry));

            return spellByName;
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

        public static ConverterHandler<SuperArea> SuperAreaConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var superArea = World.Instance.GetSuperArea(outvalue);

                if (superArea == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid super area id", entry));

                return superArea;
            }

            var superAreaByName = World.Instance.GetSuperArea(entry);

            if (superAreaByName == null)
                throw new ConverterException(string.Format("'{0}' is not a super area name", entry));

            return superAreaByName;
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
                throw new ConverterException(string.Format("'{0}' is not an area name", entry));

            return areaByName;
        };

        public static ConverterHandler<SubArea> SubAreaConverter = (entry, trigger) =>
        {
            int outvalue;
            if (int.TryParse(entry, out outvalue))
            {
                var area = World.Instance.GetSubArea(outvalue);

                if (area == null)
                    throw new ConverterException(string.Format("'{0}' is not a valid sub area id", entry));

                return area;
            }

            var areaByName = World.Instance.GetSubArea(entry);

            if (areaByName == null)
                throw new ConverterException(string.Format("'{0}' is not a sub area name", entry));

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