using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using Stump.Core.Cache;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

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

        #endregion

        protected internal MapRecord Record;
        private readonly ConcurrentDictionary<int, RolePlayActor> m_actors = new ConcurrentDictionary<int, RolePlayActor>();

        /// <summary>
        /// Array that associate a cell to a map point
        /// </summary>
        public static MapPoint[] PointsGrid;

        static Map()
        {
            PointsGrid = new MapPoint[MapPoint.MapSize];

            for (ushort i = 0; i < MapPoint.MapSize; i++)
            {
                // i is a cell
                PointsGrid[i] = new MapPoint(i);
            }
        }

        public Map(MapRecord record)
        {
            Record = record;

            InitializeValidators();
        }

        #region Properties

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

        public int BottomNeighbourId
        {
            get { return Record.BottomNeighbourId; }
            set { Record.BottomNeighbourId = value; }
        }

        public int LeftNeighbourId
        {
            get { return Record.LeftNeighbourId; }
            set { Record.LeftNeighbourId = value; }
        }

        public int RightNeighbourId
        {
            get { return Record.RightNeighbourId; }
            set { Record.RightNeighbourId = value; }
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

        #region Gets

        public T GetActor<T>(int id)
            where T : RolePlayActor
        {
            return m_actors[id] as T;
        }

        public T GetActor<T>(Predicate<T> predicate)
            where T : RolePlayActor
        {
            return m_actors.OfType<T>().Where(entry => predicate(entry)).SingleOrDefault();
        }

        public IEnumerable<T> GetActors<T>()
        {
            return m_actors.OfType<T>(); // after a benchmark we conclued that it takes approximatively 10 ticks
        }

        public IEnumerable<T> GetActors<T>(Predicate<T> predicate)
        {
            return m_actors.OfType<T>();
        }

        #endregion

        private void OnEnter(RolePlayActor actor)
        {
            m_mapComplementaryInformationsDataMessage.Invalidate();
        }

        private void OnLeave(RolePlayActor actor)
        {
            m_mapComplementaryInformationsDataMessage.Invalidate();
        }

        #region Network

        private void InitializeValidators()
        {
            m_mapComplementaryInformationsDataMessage =
                new ObjectValidator<MapComplementaryInformationsDataMessage>(BuildMapComplementaryInformationsDataMessage);
        }

        #region MapComplementaryInformationsDataMessage

        private ObjectValidator<MapComplementaryInformationsDataMessage> m_mapComplementaryInformationsDataMessage;

        private MapComplementaryInformationsDataMessage BuildMapComplementaryInformationsDataMessage()
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
                new FightCommonInformations[0]);
        }

        public MapComplementaryInformationsDataMessage GetMapComplementaryInformationsDataMessage()
        {
            return m_mapComplementaryInformationsDataMessage;
        }

        #endregion

        #endregion

        public IEnumerable<Character> GetAllCharacters()
        {
            return GetActors<Character>();
        }

        public void Do(Action<Character> action)
        {
            foreach (var character in GetAllCharacters())
            {
                action(character);
            }
        }
    }
}