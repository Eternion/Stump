﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcShopDialogLogger : NpcShopDialog
    {
        public NpcShopDialogLogger(Character character, Npc npc, IEnumerable<NpcItem> items) : base(character, npc, items)
        {
            Character = character;
            Npc = npc;
            Items = items;
            CanSell = true;
        }

        public NpcShopDialogLogger(Character character, Npc npc, IEnumerable<NpcItem> items, ItemTemplate token) : base(character, npc, items, token)
        {
            Character = character;
            Npc = npc;
            Items = items;
            Token = token;
            CanSell = true;
        }

        public override bool BuyItem(int itemId, uint amount)
        {
            if (!base.BuyItem(itemId, amount)) 
                return false;

            var itemToSell = Items.FirstOrDefault(entry => entry.Item.Id == itemId);

            var document = new BsonDocument
            {
                { "AcctId", Character.Account.Id },
                { "CharacterId", Character.Id },
                { "ItemId", itemToSell.ItemId },
                { "Amount", amount },
                { "FinalPrice", (itemToSell.Price * amount) },
                { "IsToken", Token != null },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("NpcShopBuy", document);

            return true;
        }
    }
}