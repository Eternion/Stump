using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.PvP
{
    public class PvPHandler : WorldHandlerContainer
    {
        [WorldHandler(SetEnablePVPRequestMessage.Id)]
        public static void HandleSetEnablePVPRequestMessage(WorldClient client, SetEnablePVPRequestMessage message)
        {
            client.Character.TogglePvPMode(message.enable);
        }

        public static void SendAlignmentRankUpdateMessage(IPacketReceiver client, Character character)
        {
            var alignmentRank = 0;

            switch(character.AlignmentSide)
            {
                case AlignmentSideEnum.ALIGNMENT_ANGEL:
                    alignmentRank = 1;
                    break;
                case AlignmentSideEnum.ALIGNMENT_EVIL:
                    alignmentRank = 18;
                    break;
            }

            client.Send(new AlignmentRankUpdateMessage((sbyte)alignmentRank, false));
        }
    }
}