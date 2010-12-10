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
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public class HandlerAuth : Handler
    {
        public HandlerAuth(Type message)
            : base(message)
        {
        }

        public HandlerAuth(Type message, Predicate<AuthClient> predicate)
            : base(message)
        {
            Predicate = predicate;
        }

        public Predicate<AuthClient> Predicate
        {
            get;
            set;
        }

        public override bool PredicateSuccess(BaseClient client)
        {
            if (Predicate == null)
                return true;

            else if (!(client is AuthClient))
                return false;

            return Predicate(client as AuthClient);
        }
    }
}