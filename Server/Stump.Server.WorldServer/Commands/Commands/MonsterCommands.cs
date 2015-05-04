using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
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
            Aliases = new[] { "monster" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage monsters";
        }
    }

    public class MonsterSpawnCommand : SubCommand
    {
        public MonsterSpawnCommand()
        {
            Aliases = new[] { "spawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Spawn a monster on the current location";
            ParentCommandType = typeof(MonsterCommands);
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
            Aliases = new[] { "spells" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Enumerate monster spells";
            ParentCommandType = typeof(MonsterCommands);
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
                    trigger.ReplyBold("\t{0} ({1}) Managed : {2}", description, (int)effect.EffectId,
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
            ParentCommandType = typeof(MonsterCommands);
            AddParameter("map", "m", "Map", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter("subarea", "subarea", "If defined spawn a monster on each map", isOptional: true, converter: ParametersConverter.SubAreaConverter);
        }


        public override void Execute(TriggerBase trigger)
        {
            Map map = null;
            SubArea subarea = null;

            if (!trigger.IsArgumentDefined("map") && !trigger.IsArgumentDefined("subarea"))
            {
                if (!(trigger is GameTrigger))
                {
                    trigger.ReplyError("You have to define a map or a subarea if your are not ingame");
                    return;
                }

                map = (trigger as GameTrigger).Character.Map;
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

    #region Static Monsters

    public class StaticCommands : SubCommandContainer
    {
        public StaticCommands()
        {
            Aliases = new[] { "monsterstatic" };
            Description = "Manage static monsters";
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class StaticEnableCommand : SubCommand
    {
        public StaticEnableCommand()
        {
            Aliases = new[] { "enable", "on" };
            Description = "Unspawn the map and define it as static";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(StaticCommands);
            AddParameter("map", "m", "Map to turn into static", isOptional: true,
                converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var map = trigger.IsArgumentDefined("map") ?
                trigger.Get<Map>("map") : (trigger as GameTrigger).Character.Map;

            var spawns = map.MonsterSpawns.ToArray();
            map.DisableClassicalMonsterSpawns();

            foreach (var spawn in spawns)
            {
                spawn.Map.RemoveMonsterSpawn(spawn);
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                foreach (var spawn in spawns)
                {
                    spawn.IsDisabled = true;
                    WorldServer.Instance.DBAccessor.Database.Update(spawn);
                }

                trigger.ReplyBold("{0} is now static", string.Format("{0}/{1}", map.Position.X, map.Position.Y));
            });
        }
    }

    public class StaticDisableCommand : SubCommand
    {
        public StaticDisableCommand()
        {
            Aliases = new[] { "disable", "off" };
            Description = "Respawn the map and remove the static state";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(StaticCommands);
            AddParameter("map", "m", "Map to turn into static", isOptional: true,
                converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var map = trigger.IsArgumentDefined("map") ?
                trigger.Get<Map>("map") : (trigger as GameTrigger).Character.Map;

            var spawns = map.MonsterSpawns.ToArray();

            foreach (var spawn in spawns.Where(spawn => spawn.Map != null))
            {
                spawn.Map.AddMonsterSpawn(spawn);
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                foreach (var spawn in spawns)
                {
                    spawn.IsDisabled = false;
                    WorldServer.Instance.DBAccessor.Database.Update(spawn);
                }

                // do something else ?
                trigger.ReplyBold("{0} is not static anymore", string.Format("{0}/{1}", map.Position.X, map.Position.Y));
            });
        }
    }

    public class StaticMonster : AddRemoveSubCommand
    {
        public StaticMonster()
        {
            Aliases = new[] { "spawn" };
            Description = "Add or remove a monster from the given static map";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(StaticCommands);
            AddParameter("monster", "m", "Monster template", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("cell", "c", "Given cell", isOptional: true, converter: ParametersConverter.CellConverter);
            AddParameter("direction", "d", "Given direction", isOptional: true, converter: ParametersConverter.DirectionConverter);
            AddParameter("grade", "g", "Grade of the monster (usually between 1-5)", 1, true);
            AddParameter("map", "map", "Given map", isOptional: true, converter: ParametersConverter.MapConverter);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            Map map;
            if (!trigger.IsArgumentDefined("map"))
                if (trigger is GameTrigger)
                    map = ((GameTrigger) trigger).Character.Map;
                else
                {
                    trigger.ReplyError("No map defined");
                    return;
                }
            else
                map = trigger.Get<Map>("map");

            var monsterTemplate = trigger.Get<MonsterTemplate>("monster");
            var grade = MonsterManager.Instance.GetMonsterGrade(monsterTemplate.Id, trigger.Get<int>("grade"));

            if (grade == null)
            {
                trigger.ReplyError("Monster grade {0}({1}) not found", monsterTemplate.Name, trigger.Get<int>("grade"));
                return;
            }

            var pool = map.SpawningPools.OfType<StaticSpawningPool>().FirstOrDefault();

            if (pool == null)
            {
                pool = new StaticSpawningPool(map);
                map.AddSpawningPool(pool);
            }

            var cell = trigger.IsArgumentDefined("cell") ? trigger.Get<short>("cell") : ((GameTrigger)trigger).Character.Cell.Id;
            var direction = trigger.IsArgumentDefined("direction") ? trigger.Get<DirectionsEnum>("direction") : ((GameTrigger)trigger).Character.Direction;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var group = new MonsterStaticSpawn
                {
                    Map = map,
                    GroupMonsters = new List<MonsterGrade> { grade },
                    CellId = cell,
                    Direction = (uint)direction
                };

                WorldServer.Instance.DBAccessor.Database.Insert(group);

                var record = new MonsterStaticSpawnEntity
                {
                    StaticSpawnId = group.Id,
                    MonsterGradeId = grade.Id,
                };

                WorldServer.Instance.DBAccessor.Database.Insert(record);

                if (group.GroupMonsters.Count == 1)
                    pool.AddSpawn(group);
                map.Area.ExecuteInContext(pool.StartAutoSpawn);
            });
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            Map map;
            if (!trigger.IsArgumentDefined("map"))
                if (trigger is GameTrigger)
                    map = ((GameTrigger) trigger).Character.Map;
                else
                {
                    trigger.ReplyError("No map defined");
                    return;
                }
            else
                map = trigger.Get<Map>("map");

            var monsterTemplate = trigger.Get<MonsterTemplate>("monster");

            var pool = map.SpawningPools.OfType<StaticSpawningPool>().FirstOrDefault();

            if (pool == null)
            {
                trigger.ReplyError("No static spawn here");
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var group = pool.Spawns.FirstOrDefault();

                if (group == null)
                    return;

                foreach (var spawn in pool.Spawns)
                {
                    var monsters = spawn.GroupMonsters.Where(y => y.MonsterId == monsterTemplate.Id).ToArray();

                    foreach (var monster in monsters)
                    {
                        spawn.GroupMonsters.Remove(monster);
                        WorldServer.Instance.DBAccessor.Database.Delete<MonsterStaticSpawnEntity>(
                            string.Format("WHERE StaticSpawnId={0} AND MonsterGradeId={1}", spawn.Id, monster.Id));
                    }
                }

                var spawnsToRemove = pool.Spawns.Where(x => x.GroupMonsters.Count == 0).ToArray();

                foreach (var spawn in spawnsToRemove)
                {
                    pool.RemoveSpawn(spawn);

                    WorldServer.Instance.DBAccessor.Database.Delete(spawn);
                }

                map.Area.ExecuteInContext(() =>
                {
                    if (pool.SpawnsCount == 0)
                        map.RemoveSpawningPool(pool);
                });
            });
        }
    }

    #endregion

}