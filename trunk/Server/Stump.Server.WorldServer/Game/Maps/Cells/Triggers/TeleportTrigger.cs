using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Triggers
{
    public class TeleportTrigger : CellTrigger
    {
        public TeleportTrigger(TeleportTriggerRecord record)
            : base(record)
        {
            DestinationPosition = record.GetDestinationPosition();
        }

        public ObjectPosition DestinationPosition
        {
            get;
            private set;
        }

        public override void Apply(Character character)
        {
            character.Teleport(DestinationPosition.Map, DestinationPosition.Cell);
        }
    }
}