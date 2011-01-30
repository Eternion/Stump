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

namespace Stump.Server.WorldServer.World.Zones.Map
{
    public class MapCapabilities
    {

        public MapCapabilities(int capabilities)
        {
            Capabilities = capabilities;
        }

        public readonly int Capabilities;

        public bool AllowChallenge
        {
            get { return (Capabilities & 1) != 0; }
        }

        public bool AllowAggression
        {
            get { return (Capabilities & 2) != 0; }
        }

        public bool AllowTeleportTo
        {
            get { return (Capabilities & 4) != 0; }
        }

        public bool AllowTeleportFrom
        {
            get { return (Capabilities & 8) != 0; }
        }

        public bool AllowExchangeBetweenPlayers
        {
            get { return (Capabilities & 16) != 0; }
        }

        public bool AllowHumanVendor
        {
            get { return (Capabilities & 32) != 0; }
        }

        public bool AllowCollector
        {
            get { return (Capabilities & 64) != 0; }
        }

        public bool AllowSoulCapture
        {
            get { return (Capabilities & 128) != 0; }
        }

        public bool AllowSoulSummon
        {
            get { return (Capabilities & 256) != 0; }
        }

        public bool AllowTavernRegen
        {
            get { return (Capabilities & 512) != 0; }
        }

        public bool AllowTombMode
        {
            get { return (Capabilities & 1024) != 0; }
        }

        public bool TeleportEverywhere
        {
            get { return (Capabilities & 2048) != 0; }
        }

        public bool AllowFightChallenge
        {
            get { return (Capabilities & 4096) != 0; }
        }

    }
}