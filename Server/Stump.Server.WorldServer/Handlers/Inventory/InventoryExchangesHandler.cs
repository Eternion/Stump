using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.BidHouse;
using Stump.Server.WorldServer.Game.Exchanges.Craft;
using Stump.Server.WorldServer.Game.Exchanges.Merchant;
using Stump.Server.WorldServer.Game.Exchanges.Paddock;
using Stump.Server.WorldServer.Game.Exchanges.TaxCollector;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items.BidHouse;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Dialogs.Jobs;
using Stump.Server.WorldServer.Game.Exchanges.Bank;
using Stump.Server.WorldServer.Game.Exchanges.Craft.Runes;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        [WorldHandler(ExchangePlayerMultiCraftRequestMessage.Id)]
        public static void HandleExchangePlayerMultiCraftRequestMessage(WorldClient client, ExchangePlayerMultiCraftRequestMessage message)
        {
            var target = client.Character.Map.GetActor<Character>((int) message.target);
            
            if (target == null)
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                return;
            }

            if (target.Map.Id != client.Character.Map.Id)
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                return;
            }

            if (target.IsBusy() || target.IsTrading())
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                return;
            }

            if (target.FriendsBook.IsIgnored(client.Account.Id))
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_RESTRICTED);
                return;
            }

            if (!target.IsAvailable(client.Character, false))
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                return;
            }

            if (!client.Character.Map.AllowExchangesBetweenPlayers)
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                return;
            }

            var interactives = client.Character.Map.
                GetInteractiveObjects().Where(x => x.GetSkills().Any(y => y.SkillTemplate?.Id == message.skillId && y.IsEnabled(client.Character))).ToArray();

            if (interactives.All(x => x.Position.Point.EuclideanDistanceTo(client.Character.Position.Point) >= 2))
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                return;
            }

            var interactive = interactives.FirstOrDefault();

            if (interactive == null)
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                return;
            }

            var skill = interactive.GetSkills().First(x => x.SkillTemplate?.Id == message.skillId);

            var dialog = new MultiCraftRequest(client.Character, target, interactive, skill);
            dialog.Open();
        }

        [WorldHandler(ExchangePlayerRequestMessage.Id)]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            switch ((ExchangeTypeEnum)message.exchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    var target = World.Instance.GetCharacter((int) message.target);

                    if (target == null)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                        return;
                    }

                    if (target.Map.Id != client.Character.Map.Id)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                        return;
                    }

                    if (target.IsBusy() || target.IsTrading())
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }

                    if (target.FriendsBook.IsIgnored(client.Account.Id))
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_RESTRICTED);
                        return;
                    }

                    if (!target.IsAvailable(client.Character, false))
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }
                    
                    if (!client.Character.Map.AllowExchangesBetweenPlayers)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                        return;
                    }

                    var request = new PlayerTradeRequest(client.Character, target);
                    client.Character.OpenRequestBox(request);
                    target.OpenRequestBox(request);

                    request.Open();

                    break;
                default:
                    SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                    break;
            }
        }

        [WorldHandler(ExchangeAcceptMessage.Id)]
        public static void HandleExchangeAcceptMessage(WorldClient client, ExchangeAcceptMessage message)
        {
            if (client.Character.IsInRequest() &&
                client.Character.RequestBox.IsExchangeRequest)
            {
                client.Character.AcceptRequest();
            }
        }

        [WorldHandler(ExchangeObjectMoveKamaMessage.Id)]
        public static void HandleExchangeObjectMoveKamaMessage(WorldClient client, ExchangeObjectMoveKamaMessage message)
        {
            if (!client.Character.IsInExchange())
                return;

            client.Character.Exchanger.SetKamas(message.quantity);
        }

        [WorldHandler(ExchangeCraftPaymentModificationRequestMessage.Id)]
        public static void HandleExchangeCraftPaymentModificationRequestMessage(WorldClient client, ExchangeCraftPaymentModificationRequestMessage message)
        {
            if (!(client.Character.Dialoger is CraftCustomer))
                return;

            client.Character.Exchanger.SetKamas(message.quantity);
        }

        [WorldHandler(ExchangeObjectMoveMessage.Id)]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
            if (client.Character.IsInExchange())
                client.Character.Exchanger.MoveItem(message.objectUID, message.quantity);
        }

        [WorldHandler(ExchangeReadyMessage.Id)]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
            if (message == null || client == null)
                return;

            client.Character.Trader?.ToggleReady(message.ready);
        }

        [WorldHandler(ExchangeBuyMessage.Id)]
        public static void HandleExchangeBuyMessage(WorldClient client, ExchangeBuyMessage message)
        {
            var dialog = client.Character.Dialog as IShopDialog;
            if (dialog != null)
                dialog.BuyItem(message.objectToBuyId, message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            var dialog = client.Character.Dialog as IShopDialog;
            if (dialog != null) 
                dialog.SellItem(message.objectToSellId, message.quantity);
        }

        [WorldHandler(ExchangeShowVendorTaxMessage.Id)]
        public static void HandleExchangeShowVendorTaxMessage(WorldClient client, ExchangeShowVendorTaxMessage message)
        {
            const int objectValue = 0;
            var totalTax = client.Character.MerchantBag.GetMerchantTax();

            if (totalTax <= 0)
                totalTax = 1;

            client.Send(new ExchangeReplyTaxVendorMessage(
                            objectValue,
                            totalTax));
        }

        [WorldHandler(ExchangeRequestOnShopStockMessage.Id)]
        public static void HandleExchangeRequestOnShopStockMessage(WorldClient client, ExchangeRequestOnShopStockMessage message)
        {
            if (client.Character.IsBusy())
                return;

            var exchange = new MerchantExchange(client.Character);
            exchange.Open();
        }

        [WorldHandler(ExchangeObjectMovePricedMessage.Id)]
        public static void HandleExchangeObjectMovePricedMessage(WorldClient client, ExchangeObjectMovePricedMessage message)
        {
            if (message.price <= 0)
                return;

            if (!client.Character.IsInExchange())
                return;

            if(client.Character.Exchanger is CharacterMerchant)
            {
                ((CharacterMerchant)client.Character.Exchanger).MovePricedItem(message.objectUID, message.quantity, (uint)message.price);
            }
            else if (client.Character.Exchanger is BidHouseExchanger)
            {
                ((BidHouseExchanger)client.Character.Exchanger).MovePricedItem(message.objectUID, message.quantity, (uint)message.price);
            }
        }

        [WorldHandler(ExchangeObjectModifyPricedMessage.Id)]
        public static void HandleExchangeObjectModifyPricedMessage(WorldClient client, ExchangeObjectModifyPricedMessage message)
        {
            if (!client.Character.IsInExchange())
                return;

            if (client.Character.Exchanger is CharacterMerchant)
            {
                ((CharacterMerchant)client.Character.Exchanger).ModifyItem(message.objectUID, message.quantity, (uint)message.price);
            }
            else if (client.Character.Exchanger is BidHouseExchanger)
            {
                if (message.price <= 0)
                    return;

                ((BidHouseExchanger)client.Character.Exchanger).ModifyItem(message.objectUID, (uint)message.price);
            }
        }

        [WorldHandler(ExchangeStartAsVendorMessage.Id)]
        public static void HandleExchangeStartAsVendorMessage(WorldClient client, ExchangeStartAsVendorMessage message)
        {
            client.Character.EnableMerchantMode();
        }

        [WorldHandler(ExchangeOnHumanVendorRequestMessage.Id)]
        public static void HandleExchangeOnHumanVendorRequestMessage(WorldClient client, ExchangeOnHumanVendorRequestMessage message)
        {
            var merchant = client.Character.Map.GetActor<Merchant>((int) message.humanVendorId);

            if (merchant == null || merchant.Cell.Id != message.humanVendorCell)
                return;

            var shop = new MerchantShopDialog(merchant, client.Character);
            shop.Open();
        }

        [WorldHandler(ExchangeRequestOnTaxCollectorMessage.Id)]
        public static void HandleExchangeRequestOnTaxCollectorMessage(WorldClient client, ExchangeRequestOnTaxCollectorMessage message)
        {
            if (client.Character.Guild == null)
                return;

            var taxCollectorNpc = client.Character.Map.TaxCollector;
            if (taxCollectorNpc == null)
                return;

            var guildMember = client.Character.GuildMember;

            if (!taxCollectorNpc.IsTaxCollectorOwner(guildMember))
            {
                client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NOT_OWNED));
                return;
            }

            if (!((string.Equals(taxCollectorNpc.Record.CallerName, client.Character.Name) &&
                              guildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_COLLECT_MY_TAX_COLLECTOR)) || guildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_COLLECT)))
            {
                client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS));
                return;
            }

            if (taxCollectorNpc.IsBusy())
                return;

            var exchange = new TaxCollectorExchange(taxCollectorNpc, client.Character);
            exchange.Open();
        }

        [WorldHandler(ExchangeHandleMountsStableMessage.Id)]
        public static void HandleExchangeHandleMountStableMessage(WorldClient client, ExchangeHandleMountsStableMessage message)
        {
            if (!client.Character.IsInExchange())
                return;

            var exchanger = client.Character.Exchanger as PaddockExchanger;
            if (exchanger == null)
                return;

            foreach (var rideId in message.ridesId)
            {
                switch ((StableExchangeActionsEnum)message.actionType)
                {
                    case StableExchangeActionsEnum.EQUIP_TO_STABLE:
                        exchanger.EquipToStable(rideId);
                        break;
                    case StableExchangeActionsEnum.STABLE_TO_EQUIP:
                        exchanger.StableToEquip(rideId);
                        break;
                    case StableExchangeActionsEnum.STABLE_TO_INVENTORY:
                        exchanger.StableToInventory(rideId);
                        break;
                    case StableExchangeActionsEnum.INVENTORY_TO_STABLE:
                        exchanger.InventoryToStable(rideId);
                        break;
                    case StableExchangeActionsEnum.STABLE_TO_PADDOCK:
                        exchanger.StableToPaddock(rideId);
                        break;
                    case StableExchangeActionsEnum.PADDOCK_TO_STABLE:
                        exchanger.PaddockToStable(rideId);
                        break;
                    case StableExchangeActionsEnum.EQUIP_TO_PADDOCK:
                        exchanger.EquipToPaddock(rideId);
                        break;
                    case StableExchangeActionsEnum.PADDOCK_TO_EQUIP:
                        exchanger.PaddockToEquip(rideId);
                        break;
                    case StableExchangeActionsEnum.EQUIP_TO_INVENTORY:
                        exchanger.EquipToInventory(rideId);
                        break;
                    case StableExchangeActionsEnum.PADDOCK_TO_INVENTORY:
                        exchanger.PaddockToInventory(rideId);
                        break;
                    case StableExchangeActionsEnum.INVENTORY_TO_EQUIP:
                        exchanger.InventoryToEquip(rideId);
                        break;
                    case StableExchangeActionsEnum.INVENTORY_TO_PADDOCK:
                        exchanger.InventoryToPaddock(rideId);
                        break;
                }
            }
        }

        [WorldHandler(ExchangeBidHouseTypeMessage.Id)]
        public static void HandleExchangeBidHouseTypeMessage(WorldClient client, ExchangeBidHouseTypeMessage message)
        {
            var exchange = client.Character.Exchange as BidHouseExchange;
            if (exchange == null)
                return;

            var items = BidHouseManager.Instance.GetBidHouseItems((ItemTypeEnum)message.type, exchange.MaxItemLevel).ToArray();

            SendExchangeTypesExchangerDescriptionForUserMessage(client, items.Select(x => x.Template.Id));
        }

        [WorldHandler(ExchangeBidHouseListMessage.Id)]
        public static void HandleExchangeBidHouseListMessage(WorldClient client, ExchangeBidHouseListMessage message)
        {
            var exchange = client.Character.Exchange as BidHouseExchange;
            if (exchange == null)
                return;

            exchange.UpdateCurrentViewedItem(message.id);
        }

        [WorldHandler(ExchangeBidHousePriceMessage.Id)]
        public static void HandleExchangeBidHousePriceMessage(WorldClient client, ExchangeBidHousePriceMessage message)
        {
            if (!client.Character.IsInExchange())
                return;

            var averagePrice = BidHouseManager.Instance.GetAveragePriceForItem(message.genId);

            SendExchangeBidPriceMessage(client, message.genId, averagePrice);
        }

        [WorldHandler(ExchangeBidHouseSearchMessage.Id)]
        public static void HandleExchangeBidHouseSearchMessage(WorldClient client, ExchangeBidHouseSearchMessage message)
        {
            var exchange = client.Character.Exchange as BidHouseExchange;
            if (exchange == null)
                return;

            if (!exchange.Types.Contains(message.type))
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                return;
            }

            var categories = BidHouseManager.Instance.GetBidHouseCategories(message.genId, exchange.MaxItemLevel).Select(x => x.GetBidExchangerObjectInfo()).ToArray();

            if (!categories.Any())
            {
                SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                return;
            }

            SendExchangeTypesItemsExchangerDescriptionForUserMessage(client, categories);
        }

        [WorldHandler(ExchangeBidHouseBuyMessage.Id)]
        public static void HandleExchangeBidHouseBuyMessage(WorldClient client, ExchangeBidHouseBuyMessage message)
        {
            if (!client.Character.IsInExchange())
                return;

            var category = BidHouseManager.Instance.GetBidHouseCategory(message.uid);

            if (category == null)
                return;

            var item = category.GetItem(message.qty, message.price);
            if (item == null)
            {
                //Cet objet n'est plus disponible à ce prix. Quelqu'un a été plus rapide...
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 64);

                SendExchangeBidHouseBuyResultMessage(client, message.uid, false);
                return;
            }

            if (client.Character.Inventory.IsFull(item.Template, (int)item.Stack))
            {
                //Action annulée pour cause de surcharge...
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 70);

                SendExchangeBidHouseBuyResultMessage(client, message.uid, false);
                return;
            }

            if (!item.SellItem(client.Character))
            {
                SendExchangeBidHouseBuyResultMessage(client, item.Guid, false);
                return;
            }

            var result = client.Character.Exchanger.MoveItem(item.Guid, (int)item.Stack);

            if (result)
                client.Character.Inventory.SubKamas((int)item.Price);

            SendExchangeBidHouseBuyResultMessage(client, item.Guid, result);

            //%3 x {item,%1,%2} (%4 kamas)
            client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 252, item.Template.Id, item.Guid, item.Stack, item.Price);
        }

        [WorldHandler(ExchangeCraftCountRequestMessage.Id)]
        public static void HandleExchangeCraftCountRequestMessage(WorldClient client, ExchangeCraftCountRequestMessage message)
        {
            (client.Character.Dialog as BaseCraftDialog)?.ChangeAmount(message.count);
        }

        [WorldHandler(ExchangeSetCraftRecipeMessage.Id)]
        public static void HandleExchangeSetCraftRecipeMessage(WorldClient client, ExchangeSetCraftRecipeMessage message)
        {
            if (!JobManager.Instance.Recipes.ContainsKey(message.objectGID))
                return;

            var craftActor = client.Character.Dialoger as CraftingActor;

            if (craftActor == null)
                return;

            var dialog = craftActor.CraftDialog as CraftDialog;

            if (dialog == null)
                return;

            var recipe = JobManager.Instance.Recipes[message.objectGID];
            dialog.ChangeRecipe(craftActor, recipe);
        }
        
        [WorldHandler(JobCrafterDirectoryListRequestMessage.Id)]
        public static void HandleJobCrafterDirectoryListRequestMessage(WorldClient client, JobCrafterDirectoryListRequestMessage message)
        {
            (client.Character.Dialog as JobIndexDialog)?.RequestAvailableCrafters(message.jobId);
        }

        [WorldHandler(ExchangeObjectTransfertAllFromInvMessage.Id)]
        public static void HandleExchangeObjectTransfertAllFromInvMessage(WorldClient client, ExchangeObjectTransfertAllFromInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(true, new int[0], true, false);
        }

        [WorldHandler(ExchangeObjectTransfertExistingFromInvMessage.Id)]
        public static void HandleExchangeObjectTransfertExistingFromInvMessage(WorldClient client, ExchangeObjectTransfertExistingFromInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(true, new int[0], false, true);
        }

        [WorldHandler(ExchangeObjectTransfertListFromInvMessage.Id)]
        public static void HandleExchangeObjectTransfertListFromInvMessage(WorldClient client, ExchangeObjectTransfertListFromInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(true, message.ids, false, false);
        }

        [WorldHandler(ExchangeObjectTransfertAllToInvMessage.Id)]
        public static void HandleExchangeObjectTransfertAllToInvMessage(WorldClient client, ExchangeObjectTransfertAllToInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(false, new int[0], true, false);
        }

        [WorldHandler(ExchangeObjectTransfertExistingToInvMessage.Id)]
        public static void HandleExchangeObjectTransfertExistingToInvMessage(WorldClient client, ExchangeObjectTransfertExistingToInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(false, new int[0], false, true);
        }

        [WorldHandler(ExchangeObjectTransfertListToInvMessage.Id)]
        public static void HandleExchangeObjectTransfertListToInvMessage(WorldClient client, ExchangeObjectTransfertListToInvMessage message)
        {
            var bank = client.Character.Dialog as BankDialog;
            if (bank == null)
                return;

            bank.Customer.MoveItems(false, message.ids, false, false);
        }

        [WorldHandler(ExchangeReplayStopMessage.Id)]
        public static void HandleExchangeReplayStopMessage(WorldClient client, ExchangeReplayStopMessage message)
        {
            (client.Character.Dialog as RuneMagicCraftDialog)?.StopAutoCraft();
        }

        [WorldHandler(ExchangeObjectUseInWorkshopMessage.Id)]
        public static void HandleExchangeObjectUseInWorkshopMessage(WorldClient client, ExchangeObjectUseInWorkshopMessage message)
        {
            (client.Character.Dialog as MultiRuneMagicCraftDialog)?.MoveItemFromBag(message.objectUID, message.quantity);
        }

        public static void SendExchangeRequestedTradeMessage(IPacketReceiver client, ExchangeTypeEnum type, Character source,
                                                             Character target)
        {
            client.Send(new ExchangeRequestedTradeMessage(
                            (sbyte)type,
                            source.Id,
                            target.Id));
        }

        public static void SendExchangeStartedWithPodsMessage(IPacketReceiver client, PlayerTrade playerTrade)
        {
            client.Send(new ExchangeStartedWithPodsMessage(
                            (sbyte)ExchangeTypeEnum.PLAYER_TRADE,
                            playerTrade.FirstTrader.Character.Id,
                            playerTrade.FirstTrader.Character.Inventory.Weight,
                            (int)playerTrade.FirstTrader.Character.Inventory.WeightTotal,
                            playerTrade.SecondTrader.Character.Id,
                            playerTrade.SecondTrader.Character.Inventory.Weight,
                            (int)playerTrade.SecondTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeStartedWithStorageMessage(IPacketReceiver client, ExchangeTypeEnum type, int storageMaxSlot)
        {
            client.Send(new ExchangeStartedWithStorageMessage((sbyte)type, storageMaxSlot));
        }

        public static void SendExchangeStartedMessage(IPacketReceiver client, ExchangeTypeEnum type)
        {
            client.Send(new ExchangeStartedMessage((sbyte)type));
        }

        public static void SendExchangeStartedTaxCollectorShopMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new ExchangeStartedTaxCollectorShopMessage(taxCollector.Bag.Select(x => x.GetObjectItem()), taxCollector.GatheredKamas));
        }

        public static void SendExchangeStartOkHumanVendorMessage(IPacketReceiver client, Merchant merchant)
        {
            client.Send(new ExchangeStartOkHumanVendorMessage(merchant.Id, merchant.Bag.Where(x => x.Stack > 0).Select(x => x.GetObjectItemToSellInHumanVendorShop())));
        }

        public static void SendExchangeStartOkNpcTradeMessage(IPacketReceiver client, NpcTrade trade)
        {
            client.Send(new ExchangeStartOkNpcTradeMessage(trade.SecondTrader.Npc.Id));
        }

        public static void SendExchangeStartOkNpcShopMessage(IPacketReceiver client, NpcShopDialog dialog)
        {
            client.Send(new ExchangeStartOkNpcShopMessage(dialog.Npc.Id, dialog.Token != null ? (short)dialog.Token.Id : (short)0, dialog.Items.Select(entry => entry.GetNetworkItem() as ObjectItemToSellInNpcShop)));
        }

        public static void SendExchangeLeaveMessage(IPacketReceiver client, DialogTypeEnum dialogType, bool success)
        {
            client.Send(new ExchangeLeaveMessage((sbyte)dialogType, success));
        }

        public static void SendExchangeObjectAddedMessage(IPacketReceiver client, bool remote, TradeItem item)
        {
            client.Send(new ExchangeObjectAddedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectModifiedMessage(IPacketReceiver client, bool remote, TradeItem item)
        {
            client.Send(new ExchangeObjectModifiedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectRemovedMessage(IPacketReceiver client, bool remote, int guid)
        {
            client.Send(new ExchangeObjectRemovedMessage(remote, guid));
        }

        public static void SendExchangeIsReadyMessage(IPacketReceiver client, Trader trader, bool ready)
        {
            client.Send(new ExchangeIsReadyMessage(trader.Id, ready));
        }

        public static void SendExchangeErrorMessage(IPacketReceiver client, ExchangeErrorEnum errorEnum)
        {
            client.Send(new ExchangeErrorMessage((sbyte)errorEnum));
        }

        public static void SendExchangeShopStockStartedMessage(IPacketReceiver client, CharacterMerchantBag merchantBag)
        {
            client.Send(new ExchangeShopStockStartedMessage(
                            merchantBag.Where(x => x.Stack > 0).Select(x => x.GetObjectItemToSell())));
        }

        public static void SendExchangeStartOkMountMessage(IPacketReceiver client, List<Mount> stabledMounts, List<Mount> paddockedMounts)
        {
            client.Send(new ExchangeStartOkMountMessage(stabledMounts.Select(x => x.GetMountClientData()), paddockedMounts.Select(x => x.GetMountClientData())));
        }

        public static void SendExchangeMountPaddockAddMessage(IPacketReceiver client, Mount mount)
        {
            client.Send(new ExchangeMountsPaddockAddMessage(new[] { mount.GetMountClientData() }));
        }

        public static void SendExchangeMountStableAddMessage(IPacketReceiver client, Mount mount)
        {
            client.Send(new ExchangeMountsStableAddMessage(new[] { mount.GetMountClientData() }));
        }

        public static void SendExchangeMountPaddockRemoveMessage(IPacketReceiver client, Mount mount)
        {
            client.Send(new ExchangeMountsPaddockRemoveMessage(new[] { mount.Id }));
        }

        public static void SendExchangeMountStableRemoveMessage(IPacketReceiver client, Mount mount)
        {
            client.Send(new ExchangeMountsStableRemoveMessage(new[] { mount.Id }));
        }

        public static void SendExchangeStartedBidBuyerMessage(IPacketReceiver client, BidHouseExchange exchange)
        {
            client.Send(new ExchangeStartedBidBuyerMessage(exchange.GetBuyerDescriptor()));
        }

        public static void SendExchangeStartedBidSellerMessage(IPacketReceiver client, BidHouseExchange exchange, IEnumerable<ObjectItemToSellInBid> items)
        {
            client.Send(new ExchangeStartedBidSellerMessage(exchange.GetBuyerDescriptor(), items));
        }

        public static void SendExchangeTypesExchangerDescriptionForUserMessage(IPacketReceiver client, IEnumerable<int> items)
        {
            client.Send(new ExchangeTypesExchangerDescriptionForUserMessage(items));
        }

        public static void SendExchangeTypesItemsExchangerDescriptionForUserMessage(IPacketReceiver client, IEnumerable<BidExchangerObjectInfo> items)
        {
            client.Send(new ExchangeTypesItemsExchangerDescriptionForUserMessage(items));
        }

        public static void SendExchangeBidPriceMessage(IPacketReceiver client, int itemId, int averagePrice)
        {
            client.Send(new ExchangeBidPriceMessage((short)itemId, averagePrice));
        }

        public static void SendExchangeBidHouseItemAddOkMessage(IPacketReceiver client, ObjectItemToSellInBid item)
        {
            client.Send(new ExchangeBidHouseItemAddOkMessage(item));
        }

        public static void SendExchangeBidHouseItemRemoveOkMessage(IPacketReceiver client, int sellerId)
        {
            client.Send(new ExchangeBidHouseItemRemoveOkMessage(sellerId));
        }

        public static void SendExchangeBidHouseBuyResultMessage(IPacketReceiver client, int guid, bool bought)
        {
            client.Send(new ExchangeBidHouseBuyResultMessage(guid, bought));
        }

        public static void SendExchangeBidHouseInListAddedMessage(IPacketReceiver client, BidHouseCategory category)
        {
            client.Send(new ExchangeBidHouseInListAddedMessage(category.Id, category.TemplateId, category.Effects.Select(x => x.GetObjectEffect()), category.GetPrices()));
        }

        public static void SendExchangeBidHouseInListRemovedMessage(IPacketReceiver client, BidHouseCategory category)
        {
            client.Send(new ExchangeBidHouseInListRemovedMessage(category.Id));
        }

        public static void SendExchangeBidHouseInListUpdatedMessage(IPacketReceiver client, BidHouseCategory category)
        {
            client.Send(new ExchangeBidHouseInListUpdatedMessage(category.Id, category.TemplateId, category.Effects.Select(x => x.GetObjectEffect()), category.GetPrices()));
        }

        public static void SendExchangeBidHouseGenericItemAddedMessage(IPacketReceiver client, BidHouseItem item)
        {
            client.Send(new ExchangeBidHouseGenericItemAddedMessage((short)item.Template.Id));
        }

        public static void SendExchangeBidHouseGenericItemRemovedMessage(IPacketReceiver client, BidHouseItem item)
        {
            client.Send(new ExchangeBidHouseGenericItemRemovedMessage((short)item.Template.Id));
        }

        public static void SendExchangeOfflineSoldItemsMessage(IPacketReceiver client, ObjectItemGenericQuantityPrice[] merchantItems, ObjectItemGenericQuantityPrice[] bidHouseItems)
        {
            client.Send(new ExchangeOfflineSoldItemsMessage(bidHouseItems, merchantItems));
        }

        public static void SendExchangeStartOkCraftWithInformationMessage(IPacketReceiver client, Skill skill)
        {
            client.Send(new ExchangeStartOkCraftWithInformationMessage(skill.SkillTemplate.Id));
        }

        public static void SendExchangeCraftCountModifiedMessage(IPacketReceiver client, int amount)
        {
            client.Send(new ExchangeCraftCountModifiedMessage(amount));
        }

        public static void SendExchangeCraftResultMessage(IPacketReceiver client, ExchangeCraftResultEnum result)
        {
            client.Send(new ExchangeCraftResultMessage((sbyte)result));
        }

        public static void SendExchangeCraftResultWithObjectIdMessage(IPacketReceiver client, ExchangeCraftResultEnum result, ItemTemplate item)
        {
            client.Send(new ExchangeCraftResultWithObjectIdMessage((sbyte)result, (short)item.Id));
        }

        public static void SendExchangeCraftResultWithObjectDescMessage(IPacketReceiver client, ExchangeCraftResultEnum result, BasePlayerItem createdItem, int amount)
        {
            client.Send(new ExchangeCraftResultWithObjectDescMessage((sbyte)result, new ObjectItemNotInContainer((short) createdItem.Template.Id, createdItem.Effects.Select(x => x.GetObjectEffect()), createdItem.Guid, amount)));
        }

        public static void SendExchangeCraftInformationObjectMessage(IPacketReceiver client, BasePlayerItem item, Character owner)
        {
            client.Send(new ExchangeCraftInformationObjectMessage((sbyte)ExchangeCraftResultEnum.CRAFT_SUCCESS, (short)item.Template.Id, owner.Id));
        }

        public static void SendExchangeOkMultiCraftMessage(IPacketReceiver client, Character initiator, Character other, ExchangeTypeEnum role)
        {
            client.Send(new ExchangeOkMultiCraftMessage(initiator.Id, other.Id, (sbyte)role));
        }

        public static void SendExchangeStartOkMulticraftCrafterMessage(IPacketReceiver client, InteractiveSkillTemplate skillTemplate)
        {
            client.Send(new ExchangeStartOkMulticraftCrafterMessage(skillTemplate.Id));
        }

        public static void SendExchangeStartOkMulticraftCustomerMessage(IPacketReceiver client, InteractiveSkillTemplate skillTemplate, Job job)
        {
            client.Send(new ExchangeStartOkMulticraftCustomerMessage(skillTemplate.Id, (byte)job.Level));
        }

        public static void SendExchangeCraftPaymentModifiedMessage(IPacketReceiver client, int kamas)
        {
            client.Send(new ExchangeCraftPaymentModifiedMessage(kamas));
        }

        public static void SendExchangeStartOkJobIndexMessage(IPacketReceiver client, IEnumerable<JobTemplate> jobs)
        {
            client.Send(new ExchangeStartOkJobIndexMessage(jobs.Select(x => x.Id)));
        }

        public static void SendJobCrafterDirectoryListMessage(IPacketReceiver client, IEnumerable<Job> entries)
        {
            client.Send(new JobCrafterDirectoryListMessage(entries.Select(x => x.GetJobCrafterDirectoryListEntry())));
        }

        public static void SendJobCrafterDirectoryAddMessage(IPacketReceiver client, Job entry)
        {
            client.Send(new JobCrafterDirectoryAddMessage(entry.GetJobCrafterDirectoryListEntry()));
        }

        public static void SendJobCrafterDirectoryRemoveMessage(IPacketReceiver client, Job entry)
        {
            client.Send(new JobCrafterDirectoryRemoveMessage((sbyte)entry.Template.Id, entry.Owner.Id));
        }

        public static void SendExchangeCraftResultMagicWithObjectDescMessage(IPacketReceiver client, CraftResultEnum craftResult, BasePlayerItem item, MagicPoolStatus poolStatus)
        {
            client.Send(new ExchangeCraftResultMagicWithObjectDescMessage((sbyte)craftResult, item.GetObjectItemNotInContainer(), (sbyte)poolStatus));
        }

        public static void SendExchangeItemAutoCraftStopedMessage(IPacketReceiver client, ExchangeReplayStopReasonEnum reason)
        {
            client.Send(new ExchangeItemAutoCraftStopedMessage((sbyte)reason));
        }

        public static void SendExchangeStartOkRunesTradeMessage(IPacketReceiver client)
        {
            client.Send(new ExchangeStartOkRunesTradeMessage());
        }

        public static void SendDecraftResultMessage(IPacketReceiver client, IEnumerable<DecraftedItemStackInfo> itemsInfo )
        {
            client.Send(new DecraftResultMessage(itemsInfo));
        }
    }
}