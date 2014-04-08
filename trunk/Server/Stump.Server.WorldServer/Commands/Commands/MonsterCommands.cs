using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Spawns;
using Monster = Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters.Monster;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class MonsterCommands : SubCommandContainer
    {
        public MonsterCommands()
        {
            Aliases = new[] {"monster"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage monsters";
        }
    }

    public class MonsterSpawnCommand : SubCommand
    {
        public MonsterSpawnCommand()
        {
            Aliases = new[] {"spawn"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Spawn a monster on the current location";
            ParentCommand = typeof (MonsterCommands);
            AddParameter("monster", "m", "Monster template Id", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter<sbyte>("grade", "g", "Monster grade", isOptional: true);
            AddParameter<sbyte>("id", "id", "Monster group id", isOptional: true);
            AddParameter("map", "map", "Map id", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter<short>("cell", "cell", "Cell id", isOptional: true);
            AddParameter("direction", "dir", "Direction", isOptional: true, converter: ParametersConverter.GetEnumConverter<DirectionsEnum>());
        }


        public override void Execute(TriggerBase trigger)
        {
            var template = trigger.Get<MonsterTemplate>("monster");
            ObjectPosition position = null;
            MonsterGroup group;

            if (template.Grades.Count <= trigger.Get<sbyte>("grade"))
            {
                trigger.ReplyError("Unexistant grade '{0}' for this monster", trigger.Get<sbyte>("grade"));
                return;
            }

            MonsterGrade grade = template.Grades[trigger.Get<sbyte>("grade")];

            if (grade.Template.EntityLook == null)
            {
                trigger.ReplyError("Cannot display this monster");
                return;
            }

            if (trigger.IsArgumentDefined("map") && trigger.IsArgumentDefined("cell") && trigger.IsArgumentDefined("direction"))
            {
                var map = trigger.Get<Map>("map");
                var cell = trigger.Get<short>("cell");
                var direction = trigger.Get<DirectionsEnum>("direction");

                position = new ObjectPosition(map, cell, direction);
            }
            else if (trigger is GameTrigger)
            {
                position = (trigger as GameTrigger).Character.Position;
            }

            if (position == null)
            {
                trigger.ReplyError("Position of monster is not defined");
                return;
            }

            if (trigger.IsArgumentDefined("id"))
            {
                group = position.Map.GetActor<MonsterGroup>(trigger.Get<sbyte>("id"));

                if (group == null)
                {
                    trigger.ReplyError("Group with id '{0}' not found", trigger.Get<sbyte>("id"));
                    return;
                }

                group.AddMonster(new Monster(grade, group));
            }
            else
                group = position.Map.SpawnMonsterGroup(grade, position);

            trigger.Reply("Monster '{0}' added to the group '{1}'", template.Id, group.Id);
        }
    }

    public class MonsterSpellsCommand : SubCommand
    {
        public MonsterSpellsCommand()
        {
            Aliases = new[] {"spells"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Enumerate monster spells";
            ParentCommand = typeof (MonsterCommands);
            AddParameter("monster", "m", "Monster template Id", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter<sbyte>("grade", "g", "Monster grade", isOptional: true);
        }


        public override void Execute(TriggerBase trigger)
        {
            var template = trigger.Get<MonsterTemplate>("monster");

            if (template.Grades.Count <= trigger.Get<sbyte>("grade"))
            {
                trigger.ReplyError("Unexistant grade '{0}' for this monster", trigger.Get<sbyte>("grade"));
                return;
            }

            var grade = template.Grades[trigger.Get<sbyte>("grade")];

            foreach (var spell in grade.Spells)
            {
                trigger.ReplyBold("- {0} ({1})", spell.Template.Name, spell.Template.Id);
                foreach (var effect in spell.CurrentSpellLevel.Effects)
                {
                    var description = TextManager.Instance.GetText(effect.Template.DescriptionId);
                    trigger.ReplyBold("\t{0} ({1}) Managed : {2}", description, (int) effect.EffectId,
                        SpellIdentifier.GetEffectCategories(effect.EffectId) != SpellCategory.None);
                }
            }
        }
    }
    public class MonsterSpawnNextCommand : SubCommand
    {
        public MonsterSpawnNextCommand()
        {
            Aliases = new[] { "spawnnext" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Spawn the next monster of the spawning pool";
            ParentCommand = typeof(MonsterCommands);
            AddParameter("map", "m", "Map", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter("subarea", "subarea", "If defined spawn a monster on each map", isOptional: true, converter: ParametersConverter.SubAreaConverter);
        }


        public override void Execute(TriggerBase trigger)
        {
            Map map = null;
            SubArea subarea = null;

            if (!trigger.IsArgumentDefined("map") && !trigger.IsArgumentDefined("subarea"))
            {
                if (!( trigger is GameTrigger ))
                {
                    trigger.ReplyError("You have to define a map or a subarea if your are not ingame");
                    return;
                }

                map = ( trigger as GameTrigger ).Character.Map;
            }
            else if (trigger.IsArgumentDefined("map"))
                map = trigger.Get<Map>("map");
            else if (trigger.IsArgumentDefined("subarea"))
                subarea = trigger.Get<SubArea>("subarea");

            if (map != null)
            {
                var pool = map.SpawningPools.OfType<ClassicalSpawningPool>().FirstOrDefault();

                if (pool == null)
                {
                    trigger.ReplyError("No spawning pool on the map");
                    return;
                }

                if (pool.SpawnNextGroup())
                    trigger.Reply("Next group spawned");
                else
                    trigger.ReplyError("Spawns limit reached");
            }

            else if (subarea != null)
            {
                var i = subarea.Maps.Select(subMap => subMap.SpawningPools.OfType<ClassicalSpawningPool>().FirstOrDefault()).Where(pool => pool != null).Count(pool => pool.SpawnNextGroup());

                trigger.Reply("{0} groups spawned", i);
            }
        }
    }
}