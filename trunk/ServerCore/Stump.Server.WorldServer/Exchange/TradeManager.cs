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
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Exchange
{
    public class TradeManager : InstanceManager<Trade>
    {
        public static int CreateTrade(Trade trade)
        {
            return CreateInstance(trade);
        }

        public static bool RemoveTrade(Trade trade)
        {
            return RemoveInstance(trade);
        }

        public static Trade GetTradeById(int id)
        {
            return GetInstanceById(id);
        }
    }
}