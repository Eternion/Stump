using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsHealth : StatsData
    {
        private short m_damageTaken;
        private short m_realDamageTaken;

        public StatsHealth(IStatsOwner owner, short valueBase, short damageTaken)
            : base(owner, PlayerFields.Health, valueBase)
        {
            DamageTaken = damageTaken;

            Owner.Stats[PlayerFields.Vitality].Modified += OnVitalityModified;
        }

        private void OnVitalityModified(StatsData vitality, int value)
        {
            AdjustTakenDamage();
        }

        public override short Base
        {
            get { return ValueBase; }
            set
            {
                ValueBase = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override short Equiped
        {
            get { return ValueEquiped; }
            set
            {
                ValueEquiped = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override short Given
        {
            get { return ValueGiven; }
            set
            {
                ValueGiven = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override short Context
        {
            get { return ValueContext; }
            set
            {
                ValueContext = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public short DamageTaken
        {
            get { return m_damageTaken; }
            set
            {
                m_realDamageTaken = value;
                m_damageTaken = (short) (value > TotalMax ? TotalMax : value);
                OnModified();
            }
        }

        /// <summary>
        ///   Addition of values
        /// </summary>
        public override int Total
        {
            get { return TotalSafe; }
        }

        /// <summary>
        ///   Addition of values
        /// </summary>
        /// <remarks>
        ///   Value can't be lesser than 0
        /// </remarks>
        public override int TotalSafe
        {
            get
            {
                int result = Base + Equiped + Given + Context + Owner.Stats[PlayerFields.Vitality] - DamageTaken;

                return result < 0 ? 0 : result;
            }
        }

        /// <summary>
        ///   Additions of values without using damages taken;
        /// </summary>
        public int TotalMax
        {
            get
            {
                int result = Base + Equiped + Given + Context + ( Owner.Stats != null ? Owner.Stats[PlayerFields.Vitality].Total : 0 );

                return result < 0 ? 0 : result;
            }
        }

        private void AdjustTakenDamage()
        {
            if (m_damageTaken > TotalMax)
            {
                m_realDamageTaken = m_damageTaken;
                m_damageTaken = (short) TotalMax; // hp cannot be lesser than 0
            }
            else if (m_realDamageTaken > m_damageTaken)
            {
                m_damageTaken = m_realDamageTaken;
            }
        }
    }
}