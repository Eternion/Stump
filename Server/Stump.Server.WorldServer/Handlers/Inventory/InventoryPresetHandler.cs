using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(InventoryPresetSaveMessage.Id)]
        public static void HandleInventoryPresetSaveMessage(WorldClient client, InventoryPresetSaveMessage message)
        {
            var result = client.Character.Inventory.AddPreset(message.presetId, message.symbolId, message.saveEquipment);

            SendInventoryPresetSaveResultMessage(client, message.presetId, result);
        }

        [WorldHandler(InventoryPresetDeleteMessage.Id)]
        public static void HandleInventoryPresetDeleteMessage(WorldClient client, InventoryPresetDeleteMessage message)
        {
            var result = client.Character.Inventory.RemovePreset(message.presetId);

            SendInventoryPresetDeleteResultMessage(client, message.presetId, result);
        }

        [WorldHandler(InventoryPresetItemUpdateRequestMessage.Id)]
        public static void HandleInventoryPresetItemUpdateRequestMessage(WorldClient client, InventoryPresetItemUpdateRequestMessage message)
        {
            var result = client.Character.Inventory.RemovePresetItem(message.presetId, message.position);

            if (result != PresetSaveUpdateErrorEnum.PRESET_UPDATE_ERR_UNKNOWN)
                SendInventoryPresetUpdateErrorMessage(client, result);
        }

        [WorldHandler(InventoryPresetUseMessage.Id)]
        public static void HandleInventoryPresetUse(WorldClient client, InventoryPresetUseMessage message)
        {
            var result = client.Character.Inventory.EquipPreset(message.presetId);

            SendInventoryPresetUseResultMessage(client, (sbyte)(message.presetId + 1), result);
        }

        public static void SendInventoryPresetUpdateMessage(WorldClient client, Preset preset)
        {
            client.Send(new InventoryPresetUpdateMessage(preset));
        }

        public static void SendInventoryPresetSaveResultMessage(WorldClient client, sbyte presetId, PresetSaveResultEnum result)
        {
            client.Send(new InventoryPresetSaveResultMessage(presetId, (sbyte)result));
        }

        public static void SendInventoryPresetDeleteResultMessage(WorldClient client, sbyte presetId, PresetDeleteResultEnum result)
        {
            client.Send(new InventoryPresetDeleteResultMessage(presetId, (sbyte)result));
        }

        public static void SendInventoryPresetUseResultMessage(WorldClient client, sbyte presetId, PresetUseResultEnum result)
        {
            client.Send(new InventoryPresetUseResultMessage(presetId, (sbyte)result, new byte[0]));
        }

        public static void SendInventoryPresetUpdateErrorMessage(WorldClient client, PresetSaveUpdateErrorEnum result)
        {
            client.Send(new InventoryPresetItemUpdateErrorMessage((sbyte)result));
        }
    }
}