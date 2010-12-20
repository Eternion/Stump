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
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public abstract class  Entity : ILocableIdentified, IEntityLook, INamedEntity
    {
        #region Fields

        protected static Logger logger = LogManager.GetCurrentClassLogger();


        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Entity()
        {
            Colors = new List<int>();
            Skins = new List<uint>();
        }

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Entity(int id)
        {
            Id = id;

            Colors = new List<int>();
            Skins = new List<uint>();
        }

        public virtual void OnCreate()
        {
        }

        public virtual bool IsVisible()
        {
            return true;
        }

        public virtual EntityLook ToNetworkEntityLook()
        {
            return new EntityLook(
                1, // bones id
                Skins,
                ColorsIndexed,
                Scales,
                new List<SubEntity>());
        }

        public virtual GameRolePlayActorInformations ToNetworkActor()
        {
            return new GameRolePlayActorInformations((int) Id,
                                                     ToNetworkEntityLook(),
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
        ///   Indicate or set if this entity is currently in world.
        /// </summary>
        public bool InWorld
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

        private string m_name;

        /// <summary>
        ///   The name of this entity.
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                if (InWorld)
                    throw new NotImplementedException("Dynamic renaming of Entity is not implemented.");

                m_name = value;
            }
        }

        public int BonesId
        {
            get;
            set;
        }

        public List<uint> Skins
        {
            get;
            set;
        }

        public List<int> Colors
        {
            get;
            set;
        }

        public List<int> ColorsIndexed
        {
            get
            {
                return
                    Colors.Select(
                        (color, index) => int.Parse((index + 1) + color.ToString("X6"), NumberStyles.HexNumber)).ToList();
            }
        }

        public int Scale
        {
            get;
            set;
        }

        public List<int> Scales
        {
            get { return new List<int> {Scale}; }
        }

        #endregion
    }
}