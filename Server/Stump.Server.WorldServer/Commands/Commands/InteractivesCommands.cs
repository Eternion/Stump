using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Stump.Core.Extensions;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class InteractivesCommands : SubCommandContainer
    {
        public InteractivesCommands()
        {
            Aliases = new[] { "interactives" };
            Description = "Manage interactives objects";
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class InteractiveShowCommand : SubCommand
    {
        public InteractiveShowCommand()
        {
            Aliases = new[] { "show" };
            Description = "Show interactives objects";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(InteractivesCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            var character = trigger is GameTrigger ? (trigger as GameTrigger).Character : null;

            if (character == null)
                return;
            
            var ios = character.Map.GetInteractiveObjects().ToList();

            var colors = ColorExtensions.ColorValues;
            var i = 0;
            foreach (var io in ios)
            {
                var randomColor = colors[i];
                character.Client.Send(new DebugHighlightCellsMessage(randomColor.ToArgb(), new[] { io.Cell.Id }));
                trigger.Reply(trigger.Color($"Identifier : {io.Id} - Template Id: {io.Template?.Id ?? 0} - CellId: {io.Cell.Id} - Animated - {io.Animated} - Element Id : {io.Spawn.ElementId}", randomColor));

                i++;

                if (i >= colors.Length)
                    i = 0;
            }
        }
    }

    public class InteractiveRefreshCommand : SubCommand
    {
        public InteractiveRefreshCommand()
        {
            Aliases = new[] { "refresh" };
            Description = "Refresh interactives objects";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(InteractivesCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            var methodInteractives = InteractiveManager.Instance.GetType().GetMethod("Initialize", new Type[0]);
            var methodTrigger = CellTriggerManager.Instance.GetType().GetMethod("Initialize", new Type[0]);

            World.Instance.SendAnnounce("[RELOAD] Reloading Interactives ... WORLD PAUSED", Color.DodgerBlue);
            Task.Factory.StartNew(() =>
            {
                World.Instance.UnSpawnInteractives();
                World.Instance.UnSpawnCellTriggers();

                World.Instance.Pause();
                try
                {
                    methodTrigger.Invoke(CellTriggerManager.Instance, new object[0]);
                    methodInteractives.Invoke(InteractiveManager.Instance, new object[0]);
                }
                finally
                {
                    World.Instance.Resume();
                }
               
                World.Instance.SpawnInteractives();
                World.Instance.SpawnCellTriggers();

                World.Instance.SendAnnounce("[RELOAD] Interactives reloaded ... WORLD RESUMED", Color.DodgerBlue);
            });

            trigger.Reply("Successfully refresh interactives objects !");
        }
    }

    public class InteractiveTeleportCommand : AddRemoveSubCommand
    {
        public InteractiveTeleportCommand()
        {
            Aliases = new[] { "teleport" };
            Description = "Add a teleport interactive object to the current map";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(InteractivesCommands);
            AddParameter("templateId", "Interactive templateId", converter: ParametersConverter.InteractiveTemplateConverter);
            AddParameter<int>("elementId", "ElementId");
            AddParameter("skillId", "SkillId", converter: ParametersConverter.InteractiveSkillTemplateConverter);
            AddParameter("mapDst", "map", "Map destination", converter: ParametersConverter.MapConverter);
            AddParameter<short>("cellidDst", "cellDst", "Cell destination");
            AddParameter("orientationId", "orientation", converter: ParametersConverter.DirectionConverter);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var character = trigger is GameTrigger ? (trigger as GameTrigger).Character : null;

            if (character == null)
                return;

            var map = trigger.Get<Map>("mapDst");

            if (map == null)
            {
                trigger.ReplyError("Map '{0}' doesn't exist", trigger.Get<int>("mapid"));
            }
            else
            {
                var mapSrc = character.Map;
                var cellIdDst = map.Cells[trigger.Get<short>("cellidDst")];
                var elementId = trigger.Get<int>("elementId");
                var template = trigger.Get<InteractiveTemplate>("templateId");
                var templateId = template == null ? 0 : template.Id;
                var direction = trigger.Get<DirectionsEnum>("orientationId");

                if (mapSrc.GetInteractiveObject(elementId) != null)
                {
                    trigger.ReplyError("Interactive object {0} already exists on map {1}", elementId, mapSrc.Id);
                    return;
                }

                var spawnId = InteractiveManager.Instance.PopSpawnId();
                var skillId = InteractiveManager.Instance.PopSkillId();

                var skill = new InteractiveCustomSkillRecord
                {
                    Id = skillId,
                    Type = "Teleport",
                    Duration = 0,
                    Parameter0 = map.Id.ToString(),
                    Parameter1 = cellIdDst.Id.ToString(),
                    Parameter2 = direction.ToString("D")
                };

                var spawn = new InteractiveSpawn
                {
                    Id = spawnId,
                    MapId = mapSrc.Id,
                    CustomSkills = { skill },
                    TemplateId = templateId
                };

                var spawnSkill = new InteractiveSpawnSkills
                {
                    InteractiveSpawnId = spawnId,
                    SkillId = skillId
                };

                WorldServer.Instance.IOTaskPool.AddMessage(() =>
                {
                    InteractiveManager.Instance.AddInteractiveSpawn(spawn, skill, spawnSkill);
                    ContextRoleplayHandler.SendMapComplementaryInformationsDataMessage(character.Client);
                });
                trigger.ReplyBold("Add Interactive {0} on map {1}", spawn.Template?.Name ?? "(no name)", spawn.MapId);
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var gameTrigger = trigger as GameTrigger;
            var character = gameTrigger != null ? gameTrigger.Character : null;

            if (character == null)
                return;

            var elementId = trigger.Get<int>("elementId");
            var mapSrc = character.Map;
            var interactive = InteractiveManager.Instance.GetOneSpawn(x => x.MapId == mapSrc.Id && x.Id == elementId);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                InteractiveManager.Instance.RemoveInteractiveSpawn(interactive);
                ContextRoleplayHandler.SendMapComplementaryInformationsDataMessage(character.Client);
            });
            trigger.ReplyBold("Delete Interactive {0} on map {1}", elementId, mapSrc.Id);
        }
    }

    public class InteractivesStarsCommand : SubCommand
    {
        public InteractivesStarsCommand()
        {
            Aliases = new[] { "stars" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set monster group stars bonus";
            ParentCommandType = typeof(InteractivesCommands);
            AddParameter<int>("bonus", "stars", "Bonus bewteen 0 and " + SkillHarvest.StarsBonusLimit + "%");
            AddParameter("map", "m", "Map", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter("subarea", "subarea", isOptional: true, converter: ParametersConverter.SubAreaConverter);
            AddParameter<bool>("all", "all", "All interactives", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            Map map = null;
            SubArea subarea = null;
            var bonus = trigger.Get<int>("bonus");

            if (bonus < 0 || bonus > SkillHarvest.StarsBonusLimit)
            {
                trigger.ReplyError("Bonus between 0 and 200%");
                return;
            }
            if (!trigger.IsArgumentDefined("map") && !trigger.IsArgumentDefined("subarea") && !trigger.IsArgumentDefined("all"))
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
                foreach (var skill in map.GetInteractiveObjects().SelectMany(x => x.GetSkills()).OfType<ISkillWithAgeBonus>())
                {
                    skill.AgeBonus = (short)bonus;
                }
            }
            else if (subarea != null)
            {
                foreach (var skill in subarea.Maps.SelectMany(x => x.GetInteractiveObjects().SelectMany(y => y.GetSkills()).OfType<ISkillWithAgeBonus>()))
                {
                    skill.AgeBonus = (short)bonus;
                }

            }
            else
            {
                foreach (var skill in World.Instance.GetMaps().SelectMany(x => x.GetInteractiveObjects().SelectMany(y => y.GetSkills()).OfType<ISkillWithAgeBonus>()))
                {
                    skill.AgeBonus = (short)bonus;
                }
            }

            trigger.Reply("Interactives stars set to " + bonus);
        }
    }
}