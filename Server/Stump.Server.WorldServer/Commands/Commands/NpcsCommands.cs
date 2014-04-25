using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class NpcsCommands : SubCommandContainer
    {
        public NpcsCommands()
        {
            Aliases = new[] { "npcs" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage npcs";
        }
    }

    public class NpcSpawnCommand : SubCommand
    {
        public NpcSpawnCommand()
        {
            Aliases = new[] { "spawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Spawn a npc on the current location";
            ParentCommandType = typeof(NpcsCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
            AddParameter("map", "map", "Map id", isOptional: true, converter:ParametersConverter.MapConverter);
            AddParameter<short>("cell", "cell", "Cell id", isOptional:true);
            AddParameter("direction", "dir", "Direction", isOptional:true, converter: ParametersConverter.GetEnumConverter<DirectionsEnum>());
        }


        public override void Execute(TriggerBase trigger)
        {
            var template = trigger.Get<NpcTemplate>("npc");
            ObjectPosition position = null;

            if (trigger.IsArgumentDefined("map") && trigger.IsArgumentDefined("cell") && trigger.IsArgumentDefined("direction"))
            {
                var map = trigger.Get<Map>("map");
                var cell = trigger.Get<short>("cell");
                var direction = trigger.Get<DirectionsEnum>("direction");

                position = new ObjectPosition(map, cell, direction);
            }
            else if (trigger is GameTrigger)
            {
                position = ( trigger as GameTrigger ).Character.Position;
            }

            if (position == null)
            {
                trigger.ReplyError("Position of npc is not defined");
                return;
            }

            var npc = position.Map.SpawnNpc(template, position, template.Look);

            trigger.Reply("Npc {0} spawned", npc.Id);
        }
    }

    public class NpcUnSpawnCommand : SubCommand
    {
        public NpcUnSpawnCommand()
        {
            Aliases = new[] { "unspawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Unspawn the npc by the given contextual id";
            ParentCommandType = typeof(NpcsCommands);
            AddParameter<short>("npcid", "npc", "Npc Contextual id");
            AddParameter("map", "map", "Npc Map", isOptional: true, converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var npcId = trigger.Get<short>("npcid");

            if (trigger.IsArgumentDefined("map"))
            {
                trigger.Get<Map>("map").UnSpawnNpc(npcId);
            }
            else if (trigger is GameTrigger)
            {
                ( trigger as GameTrigger ).Character.Map.UnSpawnNpc(npcId);
            }
            else
            {
                trigger.ReplyError("Npc Map must be defined !");
                return;
            }

            trigger.Reply("Npc {0} unspawned", npcId);
        }
    }
}