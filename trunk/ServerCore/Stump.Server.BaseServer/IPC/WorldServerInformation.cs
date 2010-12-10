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
using System;

namespace Stump.Server.BaseServer.IPC
{
    [Serializable]
    public class WorldServerInformation
    {
        #region Properties

        /// <summary>
        ///   World address.
        /// </summary>
        public string Address;

        /// <summary>
        ///   Internally assigned unique Id of this World.
        /// </summary>
        public int Id;

        public DateTime LastPing;
        public DateTime LastUpdate;

        /// <summary>
        ///   World name.
        /// </summary>
        public string Name;

        public ushort Port;

        public string AddressString
        {
            get { return Address + ":" + Port; }
        }

        #endregion
    }
}