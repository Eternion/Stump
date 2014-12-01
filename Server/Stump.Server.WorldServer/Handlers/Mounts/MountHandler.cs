using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Mounts
{
    public class MountHandler : WorldHandlerContainer
    {
        [WorldHandler(MountToggleRidingRequestMessage.Id)]
        public static void HandleMountToggleRidingRequestMessage(WorldClient client, MountToggleRidingRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.ToggleRiding();
        }

        [WorldHandler(MountRenameRequestMessage.Id)]
        public static void HandleMountRenameRequestMessage(WorldClient client, MountRenameRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.RenameMount(message.name);
        }

        [WorldHandler(MountSetXpRatioRequestMessage.Id)]
        public static void HandleMountSetXpRatioRequestMessage(WorldClient client, MountSetXpRatioRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.SetGivenExperience(message.xpRatio);
        }

        public static void SendMountDataMessage(IPacketReceiver client, MountClientData mountClientData)
        {
            client.Send(new MountDataMessage(mountClientData));
        }

        public static void SendMountSetMessage(IPacketReceiver client, MountClientData mountClientData)
        {
            client.Send(new MountSetMessage(mountClientData));
        }

        public static void SendMountUnSetMessage(IPacketReceiver client)
        {
            client.Send(new MountUnSetMessage());
        }

        public static void SendMountRidingMessage(IPacketReceiver client, bool riding)
        {
            client.Send(new MountRidingMessage(riding));
        }

        public static void SendMountRenamedMessage(WorldClient client, int mountId, string name)
        {
            if (client.Character.HasEquipedMount())
                client.Send(new MountRenamedMessage(mountId, name));
        }

        public static void SendMountXpRatioMessage(WorldClient client, sbyte xp)
        {
            if (client.Character.HasEquipedMount())
                client.Send(new MountXpRatioMessage(xp));
        }

        public static void SendMountReleasedMessage(IPacketReceiver client, int mountId)
        {
            client.Send(new MountReleasedMessage());
        }
    }
}
