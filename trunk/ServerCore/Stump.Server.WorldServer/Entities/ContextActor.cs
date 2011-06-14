using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities.Actors
{
    public class ContextActor
    {
        protected ContextActor(long id, EntityLook look, ObjectPosition position)
        {
            Id = id;
            Look = look;
            Position = position;
        }

        #region Fields

        public long Id
        {
            get;
            private set;
        }
        public ObjectPosition Position
        {
            get;
            protected set;
        }

        public EntityLook Look
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

        public virtual IContext Context
        {
            get { return Map; }
        }

        #endregion

        #region Actions


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