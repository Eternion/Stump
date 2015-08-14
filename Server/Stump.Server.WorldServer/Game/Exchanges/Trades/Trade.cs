#region License GNU GPL

// Trade.cs
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

using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades
{
    public abstract class Trade<TFirst, TSecond> : ITrade
        where TFirst : Trader
        where TSecond : Trader
    {
        public TFirst FirstTrader
        {
            get;
            protected set;
        }

        public TSecond SecondTrader
        {
            get;
            protected set;
        }

        Trader ITrade.FirstTrader
        {
            get { return FirstTrader; }
        }

        Trader ITrade.SecondTrader
        {
            get { return SecondTrader; }
        }

        #region ITrade Members

        public virtual void Open()
        {
            FirstTrader.ItemMoved += OnTraderItemMoved;
            FirstTrader.KamasChanged += OnTraderKamasChanged;
            FirstTrader.ReadyStatusChanged += OnTraderReadyStatusChanged;

            SecondTrader.ItemMoved += OnTraderItemMoved;
            SecondTrader.KamasChanged += OnTraderKamasChanged;
            SecondTrader.ReadyStatusChanged += OnTraderReadyStatusChanged;
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        public abstract ExchangeTypeEnum ExchangeType
        {
            get;
        }

        public virtual void Close()
        {
            if (FirstTrader.ReadyToApply && SecondTrader.ReadyToApply)
                Apply();

            FirstTrader.ItemMoved -= OnTraderItemMoved;
            FirstTrader.KamasChanged -= OnTraderKamasChanged;
            FirstTrader.ReadyStatusChanged -= OnTraderReadyStatusChanged;

            SecondTrader.ItemMoved -= OnTraderItemMoved;
            SecondTrader.KamasChanged -= OnTraderKamasChanged;
            SecondTrader.ReadyStatusChanged -= OnTraderReadyStatusChanged;
        }

        #endregion

        protected abstract void Apply();

        protected virtual void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            FirstTrader.ToggleReady(false);
            SecondTrader.ToggleReady(false);
        }

        protected virtual void OnTraderKamasChanged(Trader trader, uint amount)
        {
            FirstTrader.ToggleReady(false);
            SecondTrader.ToggleReady(false);
        }

        protected virtual void OnTraderReadyStatusChanged(Trader trader, bool status)
        {
            if (FirstTrader.ReadyToApply && SecondTrader.ReadyToApply)
                Close();
        }
    }
}