using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Compass
{
    public class CompassHandler : WorldHandlerContainer
    {
        public static void SendCompassUpdatePartyMemberMessage(WorldClient client, Game.Parties.Party party, Character character)
        {
            client.Send(new CompassUpdatePartyMemberMessage((int)CompassTypeEnum.COMPASS_TYPE_PARTY, (short)character.Map.Position.X, (short)character.Map.Position.Y, character.Id));
        }

        public static void SendCompassResetMessage(WorldClient client, CompassTypeEnum type)
        {
            client.Send(new CompassResetMessage((sbyte)type));
        }
    }
}
