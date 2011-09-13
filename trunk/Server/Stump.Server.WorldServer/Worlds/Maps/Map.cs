using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Cache;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public class Map : IContext
    {
        #region Events

        public event Action<Map, RolePlayActor> ActorEnter;

        private void NotifyActorEnter(RolePlayActor actor)
        {
            OnEnter(actor);

            Action<Map, RolePlayActor> handler = ActorEnter;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, RolePlayActor> ActorLeave;

        private void NotifyActorLeave(RolePlayActor actor)
        {
            OnLeave(actor);

            Action<Map, RolePlayActor> handler = ActorLeave;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, Fight> FightCreated;

        private void NotifyFightCreated(Fight fight)
        {
            Action<Map, Fight> handler = FightCreated;
            if (handler != null)
                handler(this, fight);
        }

        public event Action<Map, Fight> FightRemoved;

        private void NotifyFightRemoved(Fight fight)
        {
            Action<Map, Fight> handler = FightRemoved;
            if (handler != null)
                handler(this, fight);
        }

        #endregion

        /// <summary>
        /// Array that associate a cell to a map point
        /// </summary>
        public static MapPoint[] PointsGrid;

        private readonly ConcurrentDictionary<int, RolePlayActor> m_actors = new ConcurrentDictionary<int, RolePlayActor>();
        private readonly UniqueIdProvider m_contextualIds = new UniqueIdProvider(sbyte.MinValue);
        private readonly List<Fight> m_fights = new List<Fight>();
        private readonly Dictionary<int, MapNeighbour> m_mapsAround = new Dictionary<int, MapNeighbour>();

        protected internal MapRecord Record;

        static Map()
        {
            PointsGrid = new MapPoint[MapPoint.MapSize];

            for (short i = 0; i < MapPoint.MapSize; i++)
            {
                // i is a cell
                PointsGrid[i] = new MapPoint(i);
            }
        }

        public Map(MapRecord record)
        {
            Record = record;

            InitializeValidators();
            InitializeMapArrounds();
        }

        private void InitializeMapArrounds()
        {
            m_mapsAround.Add(TopNeighbourId, MapNeighbour.Top);
            m_mapsAround.Add(BottomNeighbourId, MapNeighbour.Bottom);
            m_mapsAround.Add(LeftNeighbourId, MapNeighbour.Left);
            m_mapsAround.Add(RightNeighbourId, MapNeighbour.Right);
        }

        public bool Equals(Map other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Map)) return false;
            return Equals((Map) obj);
        }

        public override int GetHashCode()
        {
            return (Record != null ? Record.GetHashCode() : 0);
        }

        public static bool operator ==(Map left, Map right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Map left, Map right)
        {
            return !Equals(left, right);
        }

        #region Properties

        private Map m_bottomNeighbour;
        private Map m_leftNeighbour;
        private Map m_rightNeighbour;
        private Map m_topNeighbour;

        public int Id
        {
            get { return Record.Id; }
        }

        public Cell[] Cells
        {
            get { return Record.Cells; }
        }

        public SubArea SubArea
        {
            get;
            internal set;
        }

        public Area Area
        {
            get { return SubArea.Area; }
        }

        public SuperArea SuperArea
        {
            get { return Area.SuperArea; }
        }

        public uint RelativeId
        {
            get { return Record.RelativeId; }
        }

        public int MapType
        {
            get { return Record.MapType; }
        }

        public Point Position
        {
            get { return Record.Position.Pos; }
        }

        public bool Outdoor
        {
            get { return Record.Outdoor; }
        }

        public int TopNeighbourId
        {
            get { return Record.TopNeighbourId; }
            set { Record.TopNeighbourId = value; }
        }

        public Map TopNeighbour
        {
            get { return m_topNeighbour ?? (m_topNeighbour = World.Instance.GetMap(TopNeighbourId)); }
        }

        public int BottomNeighbourId
        {
            get { return Record.BottomNeighbourId; }
            set { Record.BottomNeighbourId = value; }
        }

        public Map BottomNeighbour
        {
            get { return m_bottomNeighbour ?? (m_bottomNeighbour = World.Instance.GetMap(BottomNeighbourId)); }
        }

        public int LeftNeighbourId
        {
            get { return Record.LeftNeighbourId; }
            set { Record.LeftNeighbourId = value; }
        }

        public Map LeftNeighbour
        {
            get { return m_leftNeighbour ?? (m_leftNeighbour = World.Instance.GetMap(LeftNeighbourId)); }
        }

        public int RightNeighbourId
        {
            get { return Record.RightNeighbourId; }
            set { Record.RightNeighbourId = value; }
        }

        public Map RightNeighbour
        {
            get { return m_rightNeighbour ?? (m_rightNeighbour = World.Instance.GetMap(RightNeighbourId)); }
        }

        public int ShadowBonusOnEntities
        {
            get { return Record.ShadowBonusOnEntities; }
            set { Record.ShadowBonusOnEntities = value; }
        }

        public bool UseLowpassFilter
        {
            get { return Record.UseLowpassFilter; }
            set { Record.UseLowpassFilter = value; }
        }

        public bool UseReverb
        {
            get { return Record.UseReverb; }
            set { Record.UseReverb = value; }
        }

        public int PresetId
        {
            get { return Record.PresetId; }
        }

        #endregion

        #region Gets

        public IEnumerable<Character> GetAllCharacters()
        {
            return GetActors<Character>();
        }

        public void ForEach(Action<Character> action)
        {
            foreach (Character character in GetAllCharacters())
            {
                action(character);
            }
        }

        public sbyte GetNextContextualId()
        {
            return (sbyte) m_contextualIds.Pop();
        }

        public void FreeContextualId(sbyte id)
        {
            m_contextualIds.Push(id);
        }

        public T GetActor<T>(int id)
            where T : RolePlayActor
        {
            return m_actors[id] as T;
        }

        public T GetActor<T>(Predicate<T> predicate)
            where T : RolePlayActor
        {
            return m_actors.Values.OfType<T>().Where(entry => predicate(entry)).SingleOrDefault();
        }

        public IEnumerable<T> GetActors<T>()
        {
            return m_actors.Values.OfType<T>(); // after a benchmark we conclued that it takes approximatively 10 ticks
        }

        public IEnumerable<T> GetActors<T>(Predicate<T> predicate)
        {
            return m_actors.Values.OfType<T>();
        }

        #region Neighbour

        public Map GetNeighbouringMap(MapNeighbour mapNeighbour)
        {
            switch (mapNeighbour)
            {
                case MapNeighbour.Top:
                    return TopNeighbour;
                case MapNeighbour.Bottom:
                    return BottomNeighbour;
                case MapNeighbour.Right:
                    return RightNeighbour;
                case MapNeighbour.Left:
                    return LeftNeighbour;
                default:
                    throw new ArgumentException("mapNeighbour");
            }
        }

        public MapNeighbour GetMapRelativePosition(int mapid)
        {
            return !m_mapsAround.ContainsKey(mapid) ? MapNeighbour.None : m_mapsAround[mapid];
        }

        /// <summary>
        ///   Calculate which cell our character will walk on once map changed. Returns 0 if not found
        /// </summary>
        public short GetCellAfterChangeMap(short currentCell, MapNeighbour mapneighbour)
        {
            switch (mapneighbour)
            {
                case MapNeighbour.Top:
                    return (short) (currentCell + 532);
                case MapNeighbour.Bottom:
                    return (short) (currentCell - 532);
                case MapNeighbour.Right:
                    return (short) (currentCell - 13);
                case MapNeighbour.Left:
                    return (short) (currentCell + 13);
                default:
                    return 0;
            }
        }

        #endregion

        #endregion

        #region Fights

        public void AddFight(Fight fight)
        {
            if (fight.Map != this)
                return;

            m_fights.Add(fight);

            ForEach(character => ContextRoleplayHandler.SendMapFightCountMessage(character.Client,
                                                                                 (short) m_fights.Count));

            NotifyFightCreated(fight);
        }

        public void RemoveFight(Fight fight)
        {
            m_fights.Remove(fight);

            ForEach(character => ContextRoleplayHandler.SendMapFightCountMessage(character.Client,
                                                                                 (short) m_fights.Count));

            NotifyFightRemoved(fight);
        }

        #endregion

        #region Enter/Leave

        public void Enter(RolePlayActor actor)
        {
            if (m_actors.TryAdd(actor.Id, actor))
            {
                NotifyActorEnter(actor);
            }
            else
                throw new Exception(string.Format("Could not add actor '{0}' to the map", actor));
        }

        public void Leave(RolePlayActor actor)
        {
            if (m_actors.TryRemove(actor.Id, out actor))
            {
                NotifyActorLeave(actor);
            }
            else
                throw new Exception(string.Format("Could not remove actor '{0}' of the map", actor));
        }

        private void OnEnter(RolePlayActor actor)
        {
            actor.StartMoving += OnActorStartMoving;
            actor.StopMoving += OnActorStopMoving;

            ForEach(entry => ContextRoleplayHandler.SendGameRolePlayShowActorMessage(entry.Client, actor));

            if (actor is Character)
            {
                var character = actor as Character;

                ContextRoleplayHandler.SendCurrentMapMessage(character.Client, Id);

                if (m_fights.Count > 0)
                    ContextRoleplayHandler.SendMapFightCountMessage(character.Client, (short)m_fights.Count);

                SendActorsActions(character);

                BasicHandler.SendBasicTimeMessage(character.Client);
            }
        }

        private void SendActorsActions(Character character)
        {
            foreach (RolePlayActor actor in m_actors.Values)
            {
                if (actor.IsMoving())
                {
                    List<short> moveKeys = actor.MovementPath.GetServerMovementKeys();
                    RolePlayActor actorMoving = actor;

                    ContextHandler.SendGameMapMovementMessage(character.Client, moveKeys, actorMoving);
                    BasicHandler.SendBasicNoOperationMessage(character.Client);
                }
            }
        }

        private void OnLeave(RolePlayActor actor)
        {
            actor.StartMoving -= OnActorStartMoving;
            actor.StopMoving -= OnActorStopMoving;

            ForEach(entry => ContextHandler.SendGameContextRemoveElementMessage(entry.Client, actor));
        }

        #endregion

        #region Actor Actions

        private void OnActorStartMoving(ContextActor actor, MovementPath path)
        {
            List<short> movementsKey = path.GetServerMovementKeys();

            ForEach(delegate(Character entry)
                        {
                            ContextHandler.SendGameMapMovementMessage(entry.Client, movementsKey, actor);
                            BasicHandler.SendBasicNoOperationMessage(entry.Client);
                        });
        }

        private void OnActorStopMoving(ContextActor actor, MovementPath path, bool canceled)
        {
        }

        #endregion

        #region Network

        private void InitializeValidators()
        {
            // for later
        }

        #region MapComplementaryInformationsDataMessage

        public MapComplementaryInformationsDataMessage GetMapComplementaryInformationsDataMessage()
        {
            return new MapComplementaryInformationsDataMessage(
                (short) SubArea.Id,
                Id,
                0,
                new HouseInformations[0],
                m_actors.Select(entry => entry.Value.GetGameContextActorInformations() as GameRolePlayActorInformations),
                new InteractiveElement[0],
                new StatedElement[0],
                new MapObstacle[0],
                m_fights.Select(entry => entry.GetFightCommonInformations()));
        }

        #endregion

        #endregion
    }
}