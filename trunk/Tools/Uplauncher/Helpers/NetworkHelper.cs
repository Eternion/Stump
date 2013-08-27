#region License GNU GPL
// NetworkHelper.cs
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

using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Uplauncher.Helpers
{
    public class NetworkHelper
    {
        public static int FindFreePort(int min, int max)
        {
            var start = new Random().Next(min, max);
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var connections = properties.GetActiveTcpConnections();

            for (int i = start; i < max; i++)
            {
                if (connections.All(x => x.LocalEndPoint.Port != i))
                    return i;
            }

            for (int i = min; i < start; i++)
            {
                if (connections.All(x => x.LocalEndPoint.Port != i))
                    return i;
            }

            throw new Exception(string.Format("No free port available in range {0}-{1}", min, max));
        }
    }
}