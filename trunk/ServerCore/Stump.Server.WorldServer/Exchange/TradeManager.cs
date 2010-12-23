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
    public class TradeManager : InstanceManager<PlayerTrade>
    {
        public static int CreateTrade(PlayerTrade playerTrade)
        {
            return CreateInstance(playerTrade);
        }

        public static bool RemoveTrade(PlayerTrade playerTrade)
        {
            return RemoveInstance(playerTrade);
        }

        public static PlayerTrade GetTradeById(int id)
        {
            return GetInstanceById(id);
        }
    }
}