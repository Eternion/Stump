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
namespace Stump.DofusProtocol.Enums
{
    public enum ExchangeErrorEnum
    {
        REQUEST_IMPOSSIBLE = 1,
        REQUEST_CHARACTER_OCCUPIED = 2,
        REQUEST_CHARACTER_JOB_NOT_EQUIPED = 3,
        REQUEST_CHARACTER_TOOL_TOO_FAR = 4,
        REQUEST_CHARACTER_OVERLOADED = 5,
        REQUEST_CHARACTER_NOT_SUSCRIBER = 6,
        BUY_ERROR = 7,
        SELL_ERROR = 8,
        MOUNT_PADDOCK_ERROR = 9,
        BID_SEARCH_ERROR = 10,
    }
}