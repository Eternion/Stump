
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
            Context.Do(charac => ChatHandler.SendChatSmileyMessage(charac.Client, this, smileyId));
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
        public ObjectPosition Position
        {
            get;
            protected set;
        }

        public Map Map
        {
            get { return Position.Map; }
            internal set {  Position = new ObjectPosition(value, Position);}
        }

        public Zone Zone
        {
            get { return (Zone) Map.ParentSpace;  }
        }

        public Region Region
        {
            get { return (Region) Zone.ParentSpace; }
        }

        public Continent Continent
        {
            get { return (Continent) Region.ParentSpace; }
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