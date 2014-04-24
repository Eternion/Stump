using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
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
            Aliases = new[] {"dungeon"};
            Description = "Manage and create dungeons";
            RequiredRole = RoleEnum.GameMaster;
        }
    }

    public class DungeonEnableCommand : SubCommand
    {
        public DungeonEnableCommand()
        {
            Aliases = new[] {"enable", "on"};
            Description = "Unspawn the sub area and define it as a dungeon";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof (DungeonCommands);
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

            foreach (var map in subarea.Maps)
            {
                map.DisableClassicalMonsterSpawns();
            }

            foreach (var spawn in spawns)
            {
                if (spawn.Map != null)
                    spawn.Map.RemoveMonsterSpawn(spawn);
                if (spawn.SubArea != null)
                    foreach (var map in spawn.SubArea.Maps)
                        map.RemoveMonsterSpawn(spawn);
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                foreach (var spawn in spawns)
                {
                    spawn.IsDisabled = true;
                    WorldServer.Instance.DBAccessor.Database.Update(spawn);
                }

                // do something else ?
                trigger.ReplyBold("{0} is now a dungeon", subarea.Record.Name);
            });
        }
    }

      public class DungeonDisableCommand : SubCommand
    {
        public DungeonDisableCommand()
        {
            Aliases = new[] {"disable", "off"};
            Description = "Respawn the sub area and remove the dungeon state";
            RequiredRole = RoleEnum.GameMaster;            
            ParentCommandType = typeof (DungeonCommands);
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
                if (spawn.SubArea != null)
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
            Aliases = new[] {"monster"};
            Description = "Add or remove a monster from the given dungeon map";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof (DungeonCommands);
            AddParameter("monster", "m", "Monster template", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("grade", "g", "Grade of the monster (usually between 1-5)", 1, true);
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


            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var group = pool.Spawns.FirstOrDefault();

                if (group == null)
                {
                    group = new MonsterDungeonSpawn()
                    {
                        Map = map,
                        GroupMonsters = new List<MonsterGrade>() {grade},
                    };
                    WorldServer.Instance.DBAccessor.Database.Insert(group);
                }
                else
                {
                    if (group.GroupMonsters == null)
                        group.GroupMonsters = new List<MonsterGrade>();

                    group.GroupMonsters.Add(grade);
                }

                var record = new MonsterDungeonSpawnEntity()
                {
                    DungeonSpawnId = group.Id,
                    MonsterGradeId = grade.Id,
                };

                WorldServer.Instance.DBAccessor.Database.Insert(record);

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
                    var monsters = spawn.GroupMonsters.Where(y => y.MonsterId == monsterTemplate.Id).ToArray();

                    foreach (var monster in monsters)
                    {
                        spawn.GroupMonsters.Remove(monster);
                        WorldServer.Instance.DBAccessor.Database.Delete<MonsterDungeonSpawnEntity>(
                            string.Format("WHERE DungeonSpawnId={0} AND MonsterGradeId={1}", spawn.Id, monster.Id));
                    }
                }

                var spawnsToRemove = pool.Spawns.Where(x => x.GroupMonsters.Count==0).ToArray();

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
            Aliases = new[] {"teleport"};
            Description = "Set dungeon teleport event to current location";
            ParentCommandType = typeof (DungeonCommands);
            RequiredRole=RoleEnum.GameMaster;
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

            foreach (var pool in pools)
            {
                foreach (var spawn in pool.Spawns)
                {
                    spawn.TeleportEvent = true;
                    spawn.TeleportCell = trigger.Character.Cell.Id;
                    spawn.TeleportMap = trigger.Character.Map;
                    spawn.TeleportDirection = trigger.Character.Direction;

                    WorldServer.Instance.IOTaskPool.AddMessage(() =>
                        WorldServer.Instance.DBAccessor.Database.Update(spawn));
                }
            }

            trigger.Reply("Teleport event defined.");
        }
    }
}