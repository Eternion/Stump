using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Compass
{
    public class CompassHandler : WorldHandlerContainer
    {
        public static void SendCompassUpdatePartyMemberMessage(WorldClient client, Character character, bool active)
        {
            client.Send(new CompassUpdatePartyMemberMessage(
                (sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY,
                new MapCoordinates((short)character.Map.Position.X, (short)character.Map.Position.Y),
                character.Id,
                active));
        }

        public static void SendCompassUpdatePvpSeekMessage(WorldClient client, Character character)
        {
            client.Send(new CompassUpdatePvpSeekMessage((int)CompassTypeEnum.COMPASS_TYPE_PVP_SEEK, new MapCoordinates((short)character.Map.Position.X, (short)character.Map.Position.Y), character.Id, character.Name));
        }

        public static void SendCompassResetMessage(WorldClient client, CompassTypeEnum type)
        {
            client.Send(new CompassResetMessage((sbyte)type));
        }
    }
}
