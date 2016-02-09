using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Timers;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Game.Maps.Spawns
{
    public enum SpawningPoolState
    {
        Stoped,
        Running,
        Paused
    }

    public abstract class SpawningPoolBase
    {
        protected SpawningPoolBase(Map map)
        {
            Map = map;
            Map.ActorLeave += OnMapActorLeave;
            Spawns = new List<MonsterGroup>();
        }

        protected SpawningPoolBase(Map map, int interval)
            : this(map)
        {
            Interval = interval;
        }

        public Map Map
        {
            get;
            protected set;
        }

        public int Interval
        {
            get;
            protected set;
        }

        public int RemainingTime => State != SpawningPoolState.Running ? 0 : (int)(SpawnTimer.NextTick - DateTime.Now).TotalMilliseconds;

        protected List<MonsterGroup> Spawns
        {
            get;
            set;
        }

        public int SpawnsCount => Spawns.Count;

        protected TimedTimerEntry SpawnTimer
        {
            get;
            set;
        }

        public SpawningPoolState State
        {
            get;
            private set;
        }

        public bool AutoSpawnEnabled => State != SpawningPoolState.Stoped;

        public void StartAutoSpawn()
        {
            lock (this)
            {
                if (!Map.CanSpawnMonsters())
                {
                    return;
                }

                if (AutoSpawnEnabled)
                    return;

                ResetTimer();
                State = SpawningPoolState.Running;
                OnAutoSpawnEnabled();
            }
        }

        protected virtual void OnAutoSpawnEnabled()
        {
            SpawnNextGroup();
        }

        public void StopAutoSpawn()
        {
            lock (this)
            {
                if (!AutoSpawnEnabled)
                    return;

                if (SpawnTimer != null)
                    SpawnTimer.Dispose();

                State = SpawningPoolState.Stoped;
                OnAutoSpawnDisabled();
            }
        }

        protected virtual void OnAutoSpawnDisabled()
        {

        }

        protected void PauseAutoSpawn()
        {
            lock (this)
            {
                if (State != SpawningPoolState.Running)
                    return;

                if (SpawnTimer != null)
                    SpawnTimer.Dispose();

                State = SpawningPoolState.Paused;
            }
        }

        protected void ResumeAutoSpawn()
        {
            lock (this)
            {
                if (State != SpawningPoolState.Paused)
                    return;

                ResetTimer();
                State = SpawningPoolState.Running;
                OnAutoSpawnEnabled();
            }
        }

        void TimerCallBack()
        {
            if (IsLimitReached())
                PauseAutoSpawn();
            else
            {
                lock (this)
                {
                    SpawnNextGroup();
                }

                if (IsLimitReached())
                    PauseAutoSpawn();
                else
                    ResetTimer();
            }
        }

        void ResetTimer()
        {
            SpawnTimer = Map.Area.CallDelayed(GetNextSpawnInterval(), TimerCallBack);
        }

        public bool SpawnNextGroup()
        {
            var group = DequeueNextGroupToSpawn();

            return SpawnGroup(group);
        }

        public bool SpawnGroup(MonsterGroup group)
        {            
            if (group == null)
                return false;

            Map.Enter(group);

            OnGroupSpawned(group);

            return true;
        }

        public void SetTimer(int time)
        {
            lock (this)
            {
                Interval = time;

                ResetTimer();
            }
        }

        protected abstract bool IsLimitReached();
        protected abstract int GetNextSpawnInterval();

        protected abstract MonsterGroup DequeueNextGroupToSpawn();

        private void OnMapActorLeave(Map map, RolePlayActor actor)
        {
            var group = actor as MonsterGroup;
            if (group != null && (Spawns.Contains(group)))
                OnGroupUnSpawned(group);
        }

        public void UnSpawnGroup(MonsterGroup group)
        {
            OnGroupUnSpawned(group);
        }

        public event Action<SpawningPoolBase, MonsterGroup> Spawned;

        protected virtual void OnGroupSpawned(MonsterGroup group)
        {
            lock (Spawns)
                Spawns.Add(group);

            var handler = Spawned;
            if (handler != null)
                handler(this, group);
        }

        protected virtual void OnGroupUnSpawned(MonsterGroup monster)
        {
            var monsterToDelete = Spawns.FirstOrDefault(x => x.Id == monster.Id);

            lock (Spawns)
                Spawns.Remove(monsterToDelete);

            if (!IsLimitReached() && State == SpawningPoolState.Paused)
                ResumeAutoSpawn();
        }
    }
}