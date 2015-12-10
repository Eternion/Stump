#region License GNU GPL
// Trader.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades
{
    public abstract class Trader : Exchanger
    {
        public delegate void ItemMovedHandler(Trader trader, TradeItem item, bool modified, int difference);

        public delegate void KamasChangedHandler(Trader trader, uint kamasAmount);

        public delegate void ReadyStatusChangedHandler(Trader trader, bool isReady);

        public event ItemMovedHandler ItemMoved;

        protected virtual void OnItemMoved(TradeItem item, bool modified, int difference)
        {
            var handler = ItemMoved;
            if (handler != null)
                handler(this, item, modified, difference);
        }

        public event KamasChangedHandler KamasChanged;

        protected virtual void OnKamasChanged(uint kamasAmount)
        {
            var handler = KamasChanged;
            if (handler != null)
                handler(this, kamasAmount);
        }

        public event ReadyStatusChangedHandler ReadyStatusChanged;

        protected virtual void OnReadyStatusChanged(bool isready)
        {
            var handler = ReadyStatusChanged;
            if (handler != null)
                handler(this, isready);
        }

        private readonly List<TradeItem> m_items = new List<TradeItem>();

        protected Trader(ITrade trade)
            : base (trade)
        {
            Trade = trade;
        }

        public ITrade Trade
        {
            get;
            private set;
        }

        public ReadOnlyCollection<TradeItem> Items
        {
            get { return m_items.AsReadOnly(); }
        }

        public string ItemsString
        {
            get { return string.Join("|", m_items.Select(item => item.Template.Id + "_" + item.Stack)); }
        }

        public abstract int Id
        {
            get;
        }

        public uint Kamas
        {
            get;
            private set;
        }

        public bool ReadyToApply
        {
            get;
            private set;
        }

        protected void AddItem(TradeItem item)
        {
            m_items.Add(item);
        }

        protected bool RemoveItem(TradeItem item)
        {
            return m_items.Remove(item);
        }

        protected void ClearItems()
        {
            m_items.Clear();
        }

        public void ToggleReady()
        {
            ToggleReady(!ReadyToApply);
        }
        public override bool SetKamas(int amount)
        {
            if (amount < 0)
                return false;

            ToggleReady(false);

            Kamas = (uint)amount;

            OnKamasChanged(Kamas);

            return true;
        }

        public virtual void ToggleReady(bool status)
        {
            if (status == ReadyToApply)
                return;

            ReadyToApply = status;

            OnReadyStatusChanged(ReadyToApply);
        }
    }
}