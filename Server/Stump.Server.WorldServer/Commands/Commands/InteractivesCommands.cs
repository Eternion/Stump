using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Interactives;
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

            var ios = character.Map.Record.Elements.ToList();

            foreach (var io in ios)
            {
                var randomColor = GetRandomColor();
                character.Client.Send(new DebugHighlightCellsMessage(randomColor.ToArgb() & 16777215, new[] { io.CellId }));
                character.SendServerMessage(string.Format("Element Id: {0} - CellId: {1}", io.ElementId, io.CellId), randomColor);
            }
        }

        private static Color GetRandomColor()
        {
            var randomGen = new Random();
            var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            var randomColorName = names[randomGen.Next(names.Length)];
            return Color.FromKnownColor(randomColorName);
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
                    methodInteractives.Invoke(InteractiveManager.Instance, new object[0]);
                    methodTrigger.Invoke(CellTriggerManager.Instance, new object[0]);
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
                var skillTemplate = trigger.Get<InteractiveSkillTemplate>("skillId");
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

                var skill = new InteractiveSkillRecord
                {
                    Id = skillId,
                    Type = "Teleport",
                    Duration = 0,
                    CustomTemplateId = skillTemplate.Id,
                    Parameter0 = map.Id.ToString(),
                    Parameter1 = cellIdDst.Id.ToString(),
                    Parameter2 = direction.ToString("D")
                };

                var spawn = new InteractiveSpawn
                {
                    Id = spawnId,
                    ElementId = elementId,
                    MapId = mapSrc.Id,
                    Skills = { skill },
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
                trigger.ReplyBold("Add Interactive {0} on map {1}", spawn.Template.Name, spawn.MapId);
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
            var interactive = InteractiveManager.Instance.GetOneSpawn(x => x.MapId == mapSrc.Id && x.ElementId == elementId);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                InteractiveManager.Instance.RemoveInteractiveSpawn(interactive);
                ContextRoleplayHandler.SendMapComplementaryInformationsDataMessage(character.Client);
            });
            trigger.ReplyBold("Delete Interactive {0} on map {1}", elementId, mapSrc.Id);
        }
    }
}