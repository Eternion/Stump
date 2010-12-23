// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Exchange
{
    public class TradeRequest : IDialogRequest
    {
        public TradeRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        #region IDialogRequest Members

        public Character Source
        {
            get;
            set;
        }

        public Character Target
        {
            get;
            set;
        }

        public void AcceptDialog()
        {
            try
            {
                var trade = new PlayerTrade(Source, Target);

                Source.Dialoger = trade.SourceTrader;
                Target.Dialoger = trade.TargetTrader;

                TradeManager.CreateTrade(trade);

                InventoryHandler.SendExchangeStartedWithPodsMessage(Source.Client, trade);
                InventoryHandler.SendExchangeStartedWithPodsMessage(Target.Client, trade);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        public void DeniedDialog()
        {
            try
            {
                InventoryHandler.SendExchangeLeaveMessage(Source.Client, false);
                InventoryHandler.SendExchangeLeaveMessage(Target.Client, false);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        #endregion
    }
}