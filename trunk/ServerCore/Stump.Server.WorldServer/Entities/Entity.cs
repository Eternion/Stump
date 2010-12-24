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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Look;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public abstract class  Entity : ILocableIdentified, INamedEntity
    {
        #region Fields

        protected static Logger logger = LogManager.GetCurrentClassLogger();


        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Entity(int id)
        {
            Id = id;
        }

        public virtual void OnCreate()
        {
        }

        public virtual bool IsVisible()
        {
            return true;
        }

        public virtual GameRolePlayActorInformations ToNetworkActor(WorldClient client)
        {
            return new GameRolePlayActorInformations((int) Id,
                                                     Look.EntityLook,
                                                     GetEntityDisposition());
        }

        public virtual IdentifiedEntityDispositionInformations GetIdentifiedEntityDisposition()
        {
            return new IdentifiedEntityDispositionInformations(Position.CellId, (uint)Position.Direction, (int) Id);
        }

        public virtual EntityDispositionInformations GetEntityDisposition()
        {
            return new EntityDispositionInformations(Position.CellId, (uint) Position.Direction);
        }

        #region Properties

        /// <summary>
        ///   The Id of this character.
        /// </summary>
        public long Id
        {
            get;
            set;
        }



        /// <summary>
        ///   Representation of Entity's World Position
        /// </summary>
        public VectorIsometric Position
        {
            get;
            set;
        }

        public Map Map
        {
            get { return Position.Map; }
            set { Position = new VectorIsometric(value, Position); }
        }

        public Zone Zone
        {
            get { return (Zone) Map.ParentSpace;  }
        }

        public Region Region
        {
            get { return (Region) Zone.ParentSpace; }
        }

        /// <summary>
        ///   The name of this entity.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public CharacterLook Look
        {
            get;
            set;
        }

        #endregion
    }
}