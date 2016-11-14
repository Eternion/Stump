using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class DungeonCommands : SubCommandContainer
    {
        public DungeonCommands()
        {
            Aliases = new[] { "dungeon" };
            Description = "Manage and create dungeons";
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class DungeonEnableCommand : SubCommand
    {
        public DungeonEnableCommand()
        {
            Aliases = new[] { "enable", "on" };
            Description = "Unspawn the sub area and define it as a dungeon";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(DungeonCommands);
            AddParameter("subarea", "s", "Sub area to turn into dungeon", isOptional: true,
                converter: ParametersConverter.SubAreaConverter);
            AddParameter("map", "m", "Map to turn into dungeon", isOptional: true,
                converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            SubArea subarea;
            Map map;
            MonsterSpawn[] spawns;

            if (trigger.IsArgumentDefined("map"))
            {
                map = trigger.Get<Map>("map");
                subarea = null;

                spawns = map.MonsterSpawns.ToArray();
                map.DisableClassicalMonsterSpawns();
            }
            else
            {
                if (!trigger.IsArgumentDefined("subarea"))
                    if (trigger is GameTrigger)
                        subarea = (trigger as GameTrigger).Character.SubArea;
                    else
                    {
                        trigger.ReplyError("No sub area defined");
                        return;
                    }
                else
                    subarea = trigger.Get<SubArea>("subarea");
                map = null;

                spawns = subarea.Maps.SelectMany(x => x.MonsterSpawns).Distinct().ToArray();

                foreach (var submap in subarea.Maps)
                {
                    submap.DisableClassicalMonsterSpawns();
                }
            }

            foreach (var spawn in spawns)
            {
                if (map != null)
                    spawn.Map.RemoveMonsterSpawn(spawn);
                if (subarea == null)
                    continue;

                foreach (var submap in spawn.SubArea.Maps)
                    submap.RemoveMonsterSpawn(spawn);
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                foreach (var spawn in spawns)
                {
                    spawn.IsDisabled = true;
                    WorldServer.Instance.DBAccessor.Database.Update(spawn);
                }

                // do something else ?
                var name = subarea == null ? string.Format("{0}/{1}", map.Position.X, map.Position.Y) : subarea.Record.Name;
                trigger.ReplyBold("{0} is now a dungeon", name);
            });
        }
    }

    public class DungeonDisableCommand : SubCommand
    {
        public DungeonDisableCommand()
        {
            Aliases = new[] { "disable", "off" };
            Description = "Respawn the sub area and remove the dungeon state";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(DungeonCommands);
            AddParameter("subarea", "s", "Sub area to turn into dungeon", isOptional: true,
                converter: ParametersConverter.SubAreaConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            SubArea subarea;
            if (!trigger.IsArgumentDefined("subarea"))
                if (trigger is GameTrigger)
                    subarea = (trigger as GameTrigger).Character.SubArea;
                else
                {
                    trigger.ReplyError("No sub area defined");
                    return;
                }
            else
                subarea = trigger.Get<SubArea>("subarea");

            var spawns = subarea.Maps.SelectMany(x => x.MonsterSpawns).Distinct().ToArray();

            foreach (var spawn in spawns)
            {
                if (spawn.Map != null)
                    spawn.Map.AddMonsterSpawn(spawn);
                if (spawn.SubArea == null)
                    continue;

                foreach (var map in spawn.SubArea.Maps)
                    map.AddMonsterSpawn(spawn);
            }

            foreach (var map in subarea.Maps)
            {
                map.EnableClassicalMonsterSpawns();
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                foreach (var spawn in spawns)
                {
                    spawn.IsDisabled = false;
                    WorldServer.Instance.DBAccessor.Database.Update(spawn);
                }

                // do something else ?
                trigger.ReplyBold("{0} is not a dungeon anymore", subarea.Record.Name);
            });
        }
    }

    public class DungeonMonster : AddRemoveSubCommand
    {
        public DungeonMonster()
        {
            Aliases = new[] { "monster" };
            Description = "Add or remove a monster from the given dungeon map";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(DungeonCommands);
            AddParameter("monster", "m", "Monster template", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("grade", "g", "Grade of the monster (usually between 1-5)", 1, true);
            AddParameter<int>("minmembers", "min", "Minimum party members to fight this monster", isOptional: true);
            AddParameter("map", "map", "Given map", isOptional: true, converter: ParametersConverter.MapConverter);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            Map map;
            if (!trigger.IsArgumentDefined("map"))
                if (trigger is GameTrigger)
                    map = (trigger as GameTrigger).Character.Map;
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

            var pool = map.SpawningPools.OfType<DungeonSpawningPool>().FirstOrDefault();

            if (pool == null)
            {
                pool = new DungeonSpawningPool(map);
                map.AddSpawningPool(pool);
            }

            var minPartyMembers = trigger.Get<int>("minmembers");


            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var group = pool.Spawns.FirstOrDefault();

                if (group == null)
                {
                    group = new MonsterDungeonSpawn() {Map = map};
                    WorldServer.Instance.DBAccessor.Database.Insert(group);
                }

                var record = new MonsterDungeonSpawnEntity(group, grade, minPartyMembers);

                WorldServer.Instance.DBAccessor.Database.Insert(record);

                group.GroupMonsters.Add(record);
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
                    map = (trigger as GameTrigger).Character.Map;
                else
                {
                    trigger.ReplyError("No map defined");
                    return;
                }
            else
                map = trigger.Get<Map>("map");

            var monsterTemplate = trigger.Get<MonsterTemplate>("monster");

            var pool = map.SpawningPools.OfType<DungeonSpawningPool>().FirstOrDefault();

            if (pool == null)
            {
                trigger.ReplyError("No dungeon spawn here");
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var group = pool.Spawns.FirstOrDefault();

                if (group == null)
                    return;

                foreach (var spawn in pool.Spawns)
                {
                    var monsters = spawn.GroupMonsters.Where(y => y.MonsterGrade.MonsterId == monsterTemplate.Id).ToArray();

                    foreach (var monster in monsters)
                    {
                        spawn.GroupMonsters.Remove(monster);
                        WorldServer.Instance.DBAccessor.Database.Delete<MonsterDungeonSpawnEntity>(
                            string.Format("WHERE DungeonSpawnId={0} AND MonsterGradeId={1}", spawn.Id, monster.Id));
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

    public class DungeonTeleport : InGameSubCommand
    {
        public DungeonTeleport()
        {
            Aliases = new[] { "teleport" };
            Description = "Set dungeon teleport event to current location";
            ParentCommandType = typeof(DungeonCommands);
            RequiredRole = RoleEnum.Administrator;
            AddParameter("map", "m", converter: ParametersConverter.MapConverter);
        }

        public override void Execute(GameTrigger trigger)
        {
            var map = trigger.Get<Map>("map");

            var pools = map.SpawningPools.OfType<DungeonSpawningPool>().ToArray();

            if (pools.Length == 0)
            {
                trigger.ReplyError("Target map {0} has no dungeon spawn", map.Id);
                return;
            }

            foreach (var spawn in pools.SelectMany(pool => pool.Spawns))
            {
                spawn.TeleportEvent = true;
                spawn.TeleportCell = trigger.Character.Cell.Id;
                spawn.TeleportMap = trigger.Character.Map;
                spawn.TeleportDirection = trigger.Character.Direction;

                WorldServer.Instance.IOTaskPool.AddMessage(() =>
                    WorldServer.Instance.DBAccessor.Database.Update(spawn));
            }

            trigger.Reply("Teleport event defined.");
        }
    }
}