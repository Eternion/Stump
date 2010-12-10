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
                var sourceTrade = new Trader(Source);
                var targetTrade = new Trader(Target);

                var trade = new Trade(sourceTrade,
                                      targetTrade);

                Source.Dialoger = sourceTrade;
                Target.Dialoger = targetTrade;
                Source.Dialog = trade;
                Target.Dialog = trade;

                TradeManager.CreateTrade(trade);

                ExchangeHandler.SendExchangeStartedWithPodsMessage((trade.SourceTrader.Entity as Character).Client,
                                                                   trade);
                ExchangeHandler.SendExchangeStartedWithPodsMessage((trade.TargetTrader.Entity as Character).Client,
                                                                   trade);
            }
            catch
            {
                /* ... */
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
                ExchangeHandler.SendExchangeLeaveMessage(Source.Client, false);
                ExchangeHandler.SendExchangeLeaveMessage(Target.Client, false);
            }
            catch
            {
                /* ... */
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