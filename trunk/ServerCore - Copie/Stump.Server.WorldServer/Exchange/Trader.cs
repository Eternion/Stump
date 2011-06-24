
using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Exchange
{
    public class Trader : Dialoger
    {
        public Trader(Character trader, PlayerTrade trade)
            : base(trader, trade)
        {
            Items = new List<Item>();
        }

        public PlayerTrade PlayerTrade
        {
            get { return Dialog as PlayerTrade; }
        }

        public List<Item> Items
        {
            get;
            set;
        }

        public uint Kamas
        {
            get;
            set;
        }

        public bool Ready
        {
            get;
            set;
        }
    }
}