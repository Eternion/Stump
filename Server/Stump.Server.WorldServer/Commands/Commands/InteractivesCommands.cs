using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class InteractivesCommands : SubCommandContainer
    {
        public InteractivesCommands()
        {
            Aliases = new[] {"interactives"};
            Description = "Manage interactives objects";
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class InteractiveRefreshCommand : SubCommand
    {
        public InteractiveRefreshCommand()
        {
            Aliases = new[] {"refresh"};
            Description = "Refresh interactives objects";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof (InteractivesCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            World.Instance.SpawnInteractives();
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
            AddParameter("templateId",  "Interactive templateId", isOptional:true, converter: ParametersConverter.InteractiveTemplateConverter);
            AddParameter<int>("elementId", "ElementId");
            AddParameter("skillId", "SkillId", converter: ParametersConverter.InteractiveSkillTemplateConverter);
            AddParameter("mapDst", "map", "Map destination", converter: ParametersConverter.MapConverter);
            AddParameter<short>("cellidDst", "cellDst", "Cell destination");
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
                var templateId = trigger.IsArgumentDefined("templateId") ? trigger.Get<InteractiveTemplate>("templateId").Id : -1;

                var Id = InteractiveManager.Instance.PopSpawnId();

                var skill = new InteractiveSkillRecord
                {
                    Id = Id,
                    Type = "Teleport",
                    Duration = 0,
                    CustomTemplateId = skillTemplate.Id,
                    Parameter0 = map.Id.ToString(),
                    Parameter1 = cellIdDst.Id.ToString(),
                    Parameter2 = "7"
                };

                var spawn = new InteractiveSpawn
                {
                    Id = Id,
                    ElementId = elementId,
                    MapId = mapSrc.Id,
                    Skills = { skill },
                    TemplateId = templateId
                };

                var spawnSkill = new InteractiveSpawnSkills
                {
                    InteractiveSpawnId = spawn.Id,
                    SkillId = skill.Id
                };

                WorldServer.Instance.IOTaskPool.ExecuteInContext(() => InteractiveManager.Instance.AddInteractiveSpawn(spawn, skill, spawnSkill));
                trigger.ReplyBold("Add Interactive {0} on map {1}", spawn.Template.Name, spawn.MapId);
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var character = trigger is GameTrigger ? (trigger as GameTrigger).Character : null;

            if (character == null)
                return;

            var elementId = trigger.Get<int>("elementId");
            var mapSrc = character.Map;
            var interactive = InteractiveManager.Instance.GetOneSpawn(x => x.ElementId == elementId);

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                InteractiveManager.Instance.RemoveInteractiveSpawn(interactive);
                trigger.ReplyBold("Delete Interactive {0} on map {1}", elementId, mapSrc.Id);
            });
        }
    }
}
