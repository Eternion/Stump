
using System;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public abstract partial class Actor
    {

        protected Actor(long id, string name, ExtendedLook look, ObjectPosition position)
        {
            Id = id;
            Look = look;
            Position = position;
            Name = name;
        }


        #region Fields

        public long Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public ObjectPosition Position
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
            get
            {
                return Position.Map;
            }
            internal set { Position.Map = value; }
        }

        public abstract IContext Context
        {
            get;
        }

        #endregion

        #region Actions

        public void Move()
        {
        }

        public void Teleport()
        {
        }

        public void Say()
        {
        }

        public void ChangeLook()
        {
        }

        public void ShowSmiley()
        {
        }

        public void ChangeDirection()
        {
        }

        #endregion

        #region Network

        public virtual GameContextActorInformations GetActorInformations()
        {
            return new GameContextActorInformations((int)Id, Look.EntityLook, GetDispositionInformations());
        }

        public virtual EntityDispositionInformations GetDispositionInformations()
        {
            return new EntityDispositionInformations(Position.CellId, (uint)Position.Direction);
        }

        public virtual IdentifiedEntityDispositionInformations GetIdentifiedDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations(Position.CellId, (uint)Position.Direction, (int)Id);
        }

        #endregion

    }
}