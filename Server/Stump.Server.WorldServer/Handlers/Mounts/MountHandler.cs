using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;

namespace Stump.Server.WorldServer.Handlers.Mounts
{
    public class MountHandler : WorldHandlerContainer
    {
        [WorldHandler(MountToggleRidingRequestMessage.Id)]
        public static void HandleMountToggleRidingRequestMessage(WorldClient client, MountToggleRidingRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.ToggleRiding(client.Character);
        }

        [WorldHandler(MountRenameRequestMessage.Id)]
        public static void HandleMountRenameRequestMessage(WorldClient client, MountRenameRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.RenameMount(client.Character, message.name);
        }

        [WorldHandler(MountReleaseRequestMessage.Id)]
        public static void HandleMountReleaseRequestMessage(WorldClient client, MountReleaseRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.Release(client.Character);
        }

        [WorldHandler(MountSterilizeRequestMessage.Id)]
        public static void HandleMountSterilizeRequestMessage(WorldClient client, MountSterilizeRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.Sterelize(client.Character);
        }

        [WorldHandler(MountSetXpRatioRequestMessage.Id)]
        public static void HandleMountSetXpRatioRequestMessage(WorldClient client, MountSetXpRatioRequestMessage message)
        {
            if (client.Character.HasEquipedMount())
                client.Character.Mount.SetGivenExperience(client.Character, message.xpRatio);
        }

        [WorldHandler(MountInformationRequestMessage.Id)]
        public static void HandleMountInformationRequestMessage(WorldClient client, MountInformationRequestMessage message)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var record = MountManager.Instance.TryGetMount((int) message.id);
                if (record == null)
                    return;

                var mount = new Mount(record);

                SendMountDataMessage(client, mount.GetMountClientData());
            });
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

        public static void SendMountReleaseMessage(WorldClient client, int mountId)
        {
            client.Send(new MountReleasedMessage(mountId));
        }

        public static void SendMountSterelizeMessage(WorldClient client, int mountId)
        {
            client.Send(new MountSterilizedMessage(mountId));
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
