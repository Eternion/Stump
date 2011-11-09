using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class MoveAction : AIAction
    {
        public MoveAction(AIFighter fighter, Cell destinationCell)
            : base(fighter)
        {
            DestinationCell = destinationCell;
        }

        public MoveAction(AIFighter fighter, MapPoint destination)
            : base(fighter)
        {
            Destination = destination;
        }

        public Cell DestinationCell
        {
            get;
            private set;
        }

        public MapPoint Destination
        {
            get;
            private set;
        }

        public short DestinationId
        {
            get
            {
                return Destination == null ? DestinationCell.Id : Destination.CellId;
            }
        }

        protected override RunStatus Run(object context)
        {
            var pathfinder = new Pathfinder(new AIFightCellsInformationProvider(Fighter.Fight, Fighter));
            var path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false, Fighter.MP);

            if (path == null)
                return RunStatus.Failure;

#if DEBUG            
            var completepath = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false);

            Fighter.Fight.ForEach(entry =>
                                      {
                                          if (entry.Client.Account.Role >= RoleEnum.Moderator)
                                          {
                                              ClearDisplayedCells(entry.Client);
                                              DisplayPath(entry.Client, completepath);
                                          }
                                      });
#endif

            Fighter.StartMove(path);
            return RunStatus.Success;
        }

        private static void ClearDisplayedCells(WorldClient client)
        {
            client.Send(new DebugClearHighlightCellsMessage());
        }

        public static void DisplayPath(WorldClient client, Path path)
        {
            var random = new Random();
            var buffer = new byte[3];
            random.NextBytes(buffer);
            var color = buffer[2] << 16 | buffer[1] << 8 | buffer[0];

            client.Send(new DebugHighlightCellsMessage(color, path.GetServerPathKeys()));
        }

    }
}