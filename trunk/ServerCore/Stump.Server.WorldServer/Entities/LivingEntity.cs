
using System;
using System.Threading.Tasks;
using Stump.Core.Threading;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public abstract partial class LivingEntity : Entity, ILivingEntity, IMovable
    {
        protected LivingEntity(int id) : base(id)
        {
            EntityChangeMap += OnMapChanged;
            EntityEnterFight += OnEnterFight;
            EntityMovingStart += OnMove;
        }

        public int CurrentHealth
        {
            get { return Stats.Health.TotalSafe; }
        }

        public int MaxHealth
        {
            get { return Stats.Health.TotalMax; }
        }

        #region ILivingEntity Members

        public int Level
        {
            get;
            set;
        }

        public int BaseHealth
        {
            get { return Stats.Health.Base; }
            set { Stats.Health.Base = value; }
        }

        public int DamageTaken
        {
            get { return Stats.Health.DamageTaken; }
            set { Stats.Health.DamageTaken = value; }
        }

        public StatsFields Stats
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Spell container of this entity.
        /// </summary>
        public SpellInventory Spells
        {
            get;
            protected set;
        }

        public bool IsInFight
        {
            get
            {
                return Fighter != null &&
                       (Fight.State == FightState.Fighting ||
                        Fight.State == FightState.PreparePosition);
            }
        }

        public Fight Fight
        {
            get { return FightGroup.Fight; }
        }

        public FightGroup FightGroup
        {
            get { return Fighter.GroupOwner; }
        }

        public FightGroupMember Fighter
        {
            get;
            private set;
        }

        #endregion

        public EmotesEnum Emote
        {
            get;
            private set;
        }

        public bool StartEmote(EmotesEnum emotesEnum, uint duration)
        {
            if (Context.ContextType == ContextType.RolePlay)
            {
                if (Emote == emotesEnum && // already sit/rest, so now he is standing
                    (emotesEnum == EmotesEnum.EMOTE_SIT ||
                    emotesEnum == EmotesEnum.EMOTE_REST))
                {
                    Emote = EmotesEnum.NONE;
                }
                else
                {
                    Emote = emotesEnum;
                }

                Context.Do(
                    charac => ContextHandler.SendEmotePlayMessage(charac.Client, this, emotesEnum, duration));

                if (duration > 0)
                {
                    Task.Factory.StartNewDelayed((int) duration, StopEmote);
                }

                NotifyEntityEmoteStart(emotesEnum);

                return true;
            }

            return false;
        }

        public void StopEmote()
        {
            var lastEmote = Emote;

            Emote = EmotesEnum.NONE;

            NotifyEntityEmoteStop(lastEmote);
        }

        internal bool EnterFight(FightGroup team)
        {
            if (Context.ContextType == ContextType.RolePlay)
            {
                Fighter = team.AddMember(this);
                Context = team.Fight;

                NotifyEntityEnterFight(team.Fight);

                return true;
            }
            return false;
        }

        internal bool LeaveFight()
        {
            if (Context.ContextType == ContextType.Fight)
            {
                Fight instance = Fight;

                Fighter = null;
                Context = Map;

                NotifyEntityLeaveFight(instance);

                return true;
            }

            return false;
        }

        private void OnMapChanged(LivingEntity entity, Map lastmap)
        {
            Context = Map;
        }

        private void OnEnterFight(LivingEntity entity, Fight fight)
        {
            StopEmote();
        }

        private void OnMove(LivingEntity entity, MovementPath movementPath)
        {
            StopEmote();
        }

        public abstract FightTeamMemberInformations ToNetworkTeamMember();

        public abstract GameFightFighterInformations ToNetworkFighter();

        #region Movements

        private bool m_isMoving;
        protected MovementPath m_movingPath;

        public bool IsMoving
        {
            get { return m_isMoving && m_movingPath != null; }
            protected set { m_isMoving = value; }
        }

        // todo : canMove()
        public virtual bool CanMove()
        {
            return true;
        }

        public void Move(MovementPath movementPath)
        {
            if (!CanMove())
                return;

            NotifyEntityMovingStart(movementPath);

            IsMoving = true;
            m_movingPath = movementPath;
        }

        public virtual void MoveInstant(ObjectPosition to)
        {
            if (!CanMove())
                return;

            NotifyEntityTeleport(to);
        }

        public virtual void MovementEnded()
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(m_movingPath.End);
            NotifyEntityMovingEnd(m_movingPath);

            IsMoving = false;
            m_movingPath = null;
        }

        public virtual void StopMove(ObjectPosition currentObjectPosition)
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(currentObjectPosition);
            NotifyEntityMovingCancel(currentObjectPosition);

            IsMoving = false;
            m_movingPath = null;
        }

        #endregion
    }
}