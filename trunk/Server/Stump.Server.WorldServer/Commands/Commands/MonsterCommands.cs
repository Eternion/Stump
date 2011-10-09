using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class MonsterCommands : SubCommandContainer
    {
        public MonsterCommands()
        {
            Aliases = new[] {"monster"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Manage monsters";
        }
    }

    public class MonsterSpawnCommand : SubCommand
    {
        public MonsterSpawnCommand()
        {
            Aliases = new[] {"spawn"};
            RequiredRole = RoleEnum.GameMaster;
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
}