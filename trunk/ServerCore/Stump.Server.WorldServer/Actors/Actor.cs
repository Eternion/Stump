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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public abstract class Actor
    {

        protected Actor(long id, ExtendedLook look, VectorIsometric position)
        {
            Id = id;
            Look = look;
            Position = position;
        }

        #region Fields

        public long Id
        {
            get;
            set;
        }

        public VectorIsometric Position
        {
            get;
            protected set;
        }

        public ExtendedLook Look
        {
            get;
            protected set;
        }

        public Map Map
        {
            get { return Position.Map; }
            internal set { Position = new VectorIsometric(value, Position); }
        }

        #endregion

        #region Actions


        #endregion

        #region Network

        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations(Position.CellId, (uint)Position.Direction, (int)Id);
        }

        public EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations(Position.CellId, (uint)Position.Direction);
        }

        public GameRolePlayActorInformations ToGameRolePlayActorInformations()
        {
            return new GameRolePlayActorInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations());
        }

        #endregion

    }
}