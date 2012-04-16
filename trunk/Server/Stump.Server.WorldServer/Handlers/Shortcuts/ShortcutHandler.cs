using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Shortcut = Stump.Server.WorldServer.Database.Shortcuts.Shortcut;

namespace Stump.Server.WorldServer.Handlers.Shortcuts
{
    public class ShortcutHandler : WorldHandlerContainer
    {
        [WorldHandler(ShortcutBarAddRequestMessage.Id)]
        public static void HandleShortcutBarAddRequestMessage(WorldClient client, ShortcutBarAddRequestMessage message)
        {
            client.Character.Shortcuts.AddShortcut((ShortcutBarEnum) message.barType, message.shortcut);
        }

        [WorldHandler(ShortcutBarRemoveRequestMessage.Id)]
        public static void HandleShortcutBarRemoveRequestMessage(WorldClient client, ShortcutBarRemoveRequestMessage message)
        {
            client.Character.Shortcuts.RemoveShortcut((ShortcutBarEnum)message.barType, message.slot);
        }

        [WorldHandler(ShortcutBarSwapRequestMessage.Id)]
        public static void HandleShortcutBarSwapRequestMessage(WorldClient client, ShortcutBarSwapRequestMessage message)
        {
            client.Character.Shortcuts.SwapShortcuts((ShortcutBarEnum)message.barType, message.firstSlot, message.secondSlot);
        }

        public static void SendShortcutBarContentMessage(WorldClient client, ShortcutBarEnum barType)
        {
            client.Send(new ShortcutBarContentMessage((sbyte)barType,
                client.Character.Shortcuts.GetShortcuts(barType).Select(entry => entry.GetNetworkShortcut())));
        }

        public static void SendShortcutBarRefreshMessage(IPacketReceiver client, ShortcutBarEnum barType, Shortcut shortcut)
        {
            client.Send(new ShortcutBarRefreshMessage((sbyte)barType, shortcut.GetNetworkShortcut()));
        }

        public static void SendShortcutBarRemovedMessage(IPacketReceiver client, ShortcutBarEnum barType, int slot)
        {
            client.Send(new ShortcutBarRemovedMessage((sbyte)barType, slot));
        }

        public static void SendShortcutBarRemoveErrorMessage(IPacketReceiver client)
        {
            client.Send(new ShortcutBarRemoveErrorMessage());
        }

        public static void SendShortcutBarSwapErrorMessage(IPacketReceiver client)
        {
            client.Send(new ShortcutBarSwapErrorMessage());
        }

        public static void SendShortcutBarAddErrorMessage(IPacketReceiver client)
        {
            client.Send(new ShortcutBarAddErrorMessage());
        }
    }
}