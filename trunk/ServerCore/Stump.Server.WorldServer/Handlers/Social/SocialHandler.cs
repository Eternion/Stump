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
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class SocialHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(ContactLookRequestMessage))]
        public static void HandleContactLookRequestMessage(WorldClient client, ContactLookRequestMessage message)
        {

        }

        [WorldHandler(typeof(ContactLookRequestByIdMessage))]
        public static void HandleContactLookRequestByIdMessage(WorldClient client, ContactLookRequestByIdMessage message)
        {

        }

        public static void SendContactLookMessage(WorldClient client)
        {
            client.Send(new ContactLookMessage());
        }

        public static void SendContactLookErrorMessage(WorldClient client)
        {
            client.Send(new ContactLookErrorMessage());
        }
    }
}