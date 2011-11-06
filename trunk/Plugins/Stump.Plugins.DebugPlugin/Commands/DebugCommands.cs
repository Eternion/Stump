using System;
using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class DebugCommands : SubCommandContainer
    {
        public DebugCommands()
        {
            Aliases = new[] { "debug" };
            Description = "Many commands to debug things";
        }
    }

    public class PathDisplayCommand : SubCommand
    {
        public PathDisplayCommand()
        {
            Aliases = new[] { "path" };
            ParentCommand = typeof(DebugCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display each movement paths";
        }

        public override void Execute(TriggerBase trigger)
        {
            var source = trigger.GetSource() as WorldClient;

            if (source == null || source.ActiveCharacter == null)
                return;

            source.ActiveCharacter.StartMoving += OnCharacterMoving;
            source.ActiveCharacter.StopMoving += OnCharacterStopMoving;
        }

        private void OnCharacterMoving(ContextActor actor, Path path)
        {
            var character = actor as Character;
            
            if (character == null)
                return;

            DisplayPath(character.Client, path);
        }

        private void OnCharacterStopMoving(ContextActor actor, Path path, bool canceled)
        {
            var character = actor as Character;

            if (character == null)
                return;

            character.Client.Send(new DebugClearHighlightCellsMessage());
        }
        
    }
}