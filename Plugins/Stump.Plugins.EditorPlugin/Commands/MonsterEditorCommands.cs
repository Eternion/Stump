using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Plugins.EditorPlugin.Commands
{
    public class MonsterEditorCommands : SubCommandContainer
    {
        public MonsterEditorCommands()
        {
            Aliases = new[] { "medit", "monsteredit" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Monster editor";
        }
    }

    public class MonsterSpawnCommand : AddRemoveSubCommand
    {
        public MonsterSpawnCommand()
        {
            Aliases = new [] { "spawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Add/Remove a monster spawn";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Monster to spawn", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("subarea", "subarea", isOptional: true, converter: ParametersConverter.SubAreaConverter);
            AddParameter("map", "map", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter("frequency", "freq", defaultValue: 1.0);
            AddParameter("mingrade", "min", defaultValue: 1);
            AddParameter("maxgrade", "max", defaultValue: 5);
        }
        public override void ExecuteAdd(TriggerBase trigger)
        {
            var spawn = new MonsterSpawn {MonsterId = trigger.Get<MonsterTemplate>("monster").Id};


            if (trigger.IsArgumentDefined("map"))
                spawn.Map = trigger.Get<Map>("map") ?? (trigger as GameTrigger).Character.Map;
            else if (trigger.IsArgumentDefined("subarea"))
                spawn.SubArea = trigger.Get<SubArea>("subarea") ?? ( trigger as GameTrigger ).Character.SubArea;
            else
                throw new Exception("SubArea neither Map is defined");

            spawn.Frequency = trigger.Get<double>("frequency");
            spawn.MinGrade = trigger.Get<int>("mingrade");
            spawn.MaxGrade = trigger.Get<int>("maxgrade");

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                    {
                        MonsterManager.Instance.AddMonsterSpawn(spawn);

                        if (spawn.Map != null)
                            spawn.Map.AddMonsterSpawn(spawn);
                        else if (spawn.SubArea != null)
                            spawn.SubArea.AddMonsterSpawn(spawn);

                        trigger.Reply("Monster spawn {0} added", spawn.Id);
                    });
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            Map map = null;
            SubArea subarea = null; 

            if (trigger.IsArgumentDefined("map"))
                map = trigger.Get<Map>("map") ?? (trigger as GameTrigger).Character.Map;
            else if (trigger.IsArgumentDefined("subarea"))
                subarea = trigger.Get<SubArea>("subarea") ?? (trigger as GameTrigger).Character.SubArea;
            else
                throw new Exception("SubArea neither Map is defined");

            var spawns = MonsterManager.Instance.GetMonsterSpawns().Where(entry =>
                entry.MonsterId == trigger.Get<MonsterTemplate>("monster").Id &&
                ( map == null || entry.MapId == map.Id ) &&
                ( subarea == null || entry.SubAreaId == subarea.Id )).ToArray();


            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    foreach (var spawn in spawns)
                    {
                        MonsterManager.Instance.RemoveMonsterSpawn(spawn);

                        if (spawn.Map != null)
                            spawn.Map.RemoveMonsterSpawn(spawn);
                        else if (spawn.SubArea != null)
                            spawn.SubArea.RemoveMonsterSpawn(spawn);

                        trigger.Reply("Monster spawn {0} removed", spawn.Id);
                    }
                });
        }
    }

    public class MonsterDisableCommand : SubCommand
    {
        public MonsterDisableCommand()
        {
            Aliases = new[] { "disable" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Disable a monster spawn";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Monster to disable", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("subarea", "subarea", isOptional: true, converter: ParametersConverter.SubAreaConverter);
        }
        public override void Execute(TriggerBase trigger)
        {
            IEnumerable<MonsterSpawn> spawns;

            var monster = trigger.Get<MonsterTemplate>("monster");

            if (trigger.IsArgumentDefined("subarea"))
            {
                var subArea = trigger.Get<SubArea>("subarea");

                spawns = subArea.Maps.SelectMany(x => x.MonsterSpawns).Distinct().Where(x => x.MonsterId == monster.Id).ToArray();
            }
            else
            {
                spawns = MonsterManager.Instance.GetMonsterSpawns().Where(x => x.MonsterId == monster.Id).ToArray();
            }

            foreach (var spawn in spawns)
            {
                if (spawn.Map != null)
                    spawn.Map.RemoveMonsterSpawn(spawn);

                if (spawn.SubArea == null)
                    continue;

                foreach (var map in spawn.SubArea.Maps)
                {
                    map.RemoveMonsterSpawn(spawn);
                }
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    WorldServer.Instance.DBAccessor.Database.BeginTransaction();
                    var count = 0;
                    foreach (var spawn in spawns)
                    {
                        if (!spawn.IsDisabled)
                            count ++;

                        spawn.IsDisabled = true;
                        WorldServer.Instance.DBAccessor.Database.Update(spawn);
                    }
                    WorldServer.Instance.DBAccessor.Database.CompleteTransaction();

                    trigger.ReplyBold("Disabled {0} spawns", count);
                });
        }
    }

    public class MonsterEnableCommand : SubCommand
    {
        public MonsterEnableCommand()
        {
            Aliases = new[] { "enable" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Enable a monster spawn";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Monster to enable", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("subarea", "subarea", isOptional: true, converter: ParametersConverter.SubAreaConverter);
        }
        public override void Execute(TriggerBase trigger)
        {
             IEnumerable<MonsterSpawn> spawns;

            var monster = trigger.Get<MonsterTemplate>("monster");

            if (trigger.IsArgumentDefined("subarea"))
            {
                var subArea = trigger.Get<SubArea>("subarea");

                spawns = subArea.Maps.SelectMany(x => x.MonsterSpawns).Distinct().Where(x => x.MonsterId == monster.Id);
            }
            else
            {
                spawns = MonsterManager.Instance.GetMonsterSpawns().Where(x => x.MonsterId == monster.Id);
            }

            foreach (var spawn in spawns)
            {
                if (spawn.Map != null)
                    spawn.Map.AddMonsterSpawn(spawn);

                if (spawn.SubArea == null)
                    continue;

                foreach (var map in spawn.SubArea.Maps)
                {
                    map.AddMonsterSpawn(spawn);
                }
            }


            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    WorldServer.Instance.DBAccessor.Database.BeginTransaction();
                    var count = 0;
                    foreach (var spawn in spawns)
                    {
                        if (spawn.IsDisabled)
                            count ++;

                        spawn.IsDisabled = false;
                        WorldServer.Instance.DBAccessor.Database.Update(spawn);
                    }
                    WorldServer.Instance.DBAccessor.Database.CompleteTransaction();

                    trigger.ReplyBold("Re-Enabled {0} spawns", count);
                });
        }
    }

    public class MonsterDropCommand : AddRemoveSubCommand
    {
        public MonsterDropCommand()
        {
            Aliases = new[] { "drop" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Add/Remove a monster drop";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Targetted monster", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("item", "item", converter: ParametersConverter.ItemTemplateConverter);
            AddParameter<double>("rate", "rate");
            AddParameter("lock", "lock", "Prospecting lock", 100);
            AddParameter("limit", "limit", defaultValue: 0);
            AddParameter("rolls", "max", defaultValue: 1);
            AddParameter("group", "group", "Only one item per group can be dropped (except for group 0)", defaultValue:  0);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var drop = new DroppableItem
            {
                ItemId = (short) trigger.Get<ItemTemplate>("item").Id,
                ProspectingLock = trigger.Get<int>("lock"),
                DropLimit = trigger.Get<int>("limit"),
                RollsCounter = trigger.Get<int>("rolls"),
                DropGroup = trigger.Get<int>("group"),
                MonsterOwner = monster,
            };

            drop.SetDropRate(trigger.Get<double>("rate"));

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    MonsterManager.Instance.AddMonsterDrop(drop);
                    monster.DroppableItems.Add(drop);
                    trigger.Reply("Drop {0} added", drop.Id);
                });
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var itemid = (short) trigger.Get<ItemTemplate>("item").Id;
            var drops = monster.DroppableItems.Where(entry => entry.ItemId == itemid).ToArray();

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    foreach (var drop in drops)
                    {
                        MonsterManager.Instance.RemoveMonsterDrop(drop);
                        monster.DroppableItems.Remove(drop);
                        trigger.Reply("Drop {0} removed", drop.Id);
                    }
                });
        }
    }

    public class MonsterSpellCommand : AddRemoveSubCommand
    {
        public MonsterSpellCommand()
        {
            Aliases = new[] { "spell" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Add/Remove a monster spell";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Monster to spawn", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter("spell", "spell", converter: ParametersConverter.SpellTemplateConverter);
            AddParameter<int>("level", "level", "Spell level (1 to 6)", isOptional: true);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var spell = trigger.Get<SpellTemplate>("spell");
            var level = trigger.IsArgumentDefined("level") ? (int?) trigger.Get<int>("level") : null;

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    foreach (var grade in monster.Grades)
                    {
                        var spellLevel = Math.Min(level ?? (int)grade.GradeId, spell.SpellLevelsIds.Length + 1);

                        var monsterSpell = new MonsterSpell()
                        {
                            SpellId = spell.Id,
                            Level = (sbyte)spellLevel,
                            MonsterGrade = grade,
                        };

                        MonsterManager.Instance.AddMonsterSpell(monsterSpell);
                        grade.SpellsTemplates.Add(monsterSpell);
                        grade.ReloadSpells();
                    }

                    trigger.Reply("Spell '{0}' added to '{1}'", spell.Name, monster.Name);
                });
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var spell = trigger.Get<SpellTemplate>("spell");

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    foreach (var grade in monster.Grades)
                    {
                        var spells = grade.SpellsTemplates.Where(entry => entry.SpellId == spell.Id).ToArray();

                        foreach (var monsterSpell in spells)
                        {
                            MonsterManager.Instance.RemoveMonsterSpell(monsterSpell);
                            grade.SpellsTemplates.Remove(monsterSpell);
                            grade.ReloadSpells();
                        }
                    }

                    trigger.Reply("Spell '{0}' remove from '{1}'", spell.Name, monster.Name);
                });
        }
    }

    public class MonsterKamasCommand : SubCommand
    {
        public MonsterKamasCommand()
        {
            Aliases = new[] { "kamas" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set dropped kamas";
            ParentCommandType = typeof(MonsterEditorCommands);
            AddParameter("monster", "m", "Monster to spawn", converter: ParametersConverter.MonsterTemplateConverter);
            AddParameter<int>("minkamas", "min"); 
            AddParameter<int>("maxkamas", "max");
        }

        public override void Execute(TriggerBase trigger)
        {
            var monster = trigger.Get<MonsterTemplate>("monster");
            var min = trigger.Get<int>("min");
            var max = trigger.Get<int>("max");

            if (min > max)
            {
                trigger.ReplyError("Minkamas must be smaller than maxkamas");
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    monster.MinDroppedKamas = min;
                    monster.MaxDroppedKamas = max;

                    WorldServer.Instance.DBAccessor.Database.Update(monster);

                    trigger.Reply("'{0}' now drop {1} to {2} kamas", monster.Name, min, max);
                });
        }
    }
}