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
using System.Reflection;
using Ciloci.Flee;
using NLog;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Entities
{
    public abstract class  Entity : ILocableIdentified, INamedEntity
    {
        #region Fields

        protected static Logger logger = LogManager.GetCurrentClassLogger();


        #endregion

        public event Action<Entity, Map> EnterWorld;

        protected void NotifyEnterWorld(Map map)
        {
            Action<Entity, Map> handler = EnterWorld;
            if (handler != null)
                handler(this, map);
        }

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Entity(int id)
        {
            Id = id;

            ExpressionContext = new ExpressionContext(this);
            ExpressionContext.Options.OwnerMemberAccess = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

            EnterWorld += OnEnterWorld;
        }

        private void OnEnterWorld(Entity entity, Map map)
        {
            Context = map;
        }

        public virtual void OnCreate()
        {
        }

        public virtual bool IsVisible()
        {
            return true;
        }

        public void Say(string text)
        {
            ChatManager.SayGeneral(this, ChannelId.General, text);
        }
        
        public void DisplaySmiley(byte smileyId)
        {
            Context.CallOnAllCharacters(charac => ChatHandler.SendChatSmileyMessage(charac.Client, this, smileyId));
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
            protected set;
        }

        public Map Map
        {
            get { return Position.Map; }
            internal set {  Position = new VectorIsometric(value, Position);}
        }

        public Zone Zone
        {
            get { return (Zone) Map.ParentSpace;  }
        }

        public Region Region
        {
            get { return (Region) Zone.ParentSpace; }
        }

        public SuperArea Continent
        {
            get { return (SuperArea) Region.ParentSpace; }
        }

        /// <summary>
        ///   The name of this entity.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public ExtendedLook Look
        {
            get;
            set;
        }

        public IContext Context
        {
            get;
            protected set;
        }

        public ExpressionContext ExpressionContext
        {
            get;
            set;
        }

        #endregion
    }
}