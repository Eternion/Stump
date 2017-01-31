using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsHealth : StatsData
    {
        private int m_damageTaken;
        private int m_realDamageTaken;
        private int m_permanentDamages;

        public StatsHealth(IStatsOwner owner, int valueBase, int damageTaken)
            : base(owner, PlayerFields.Health, valueBase)
        {
            DamageTaken = damageTaken;

            Owner.Stats[PlayerFields.Vitality].Modified += OnVitalityModified;
        }

        private void OnVitalityModified(StatsData vitality, int value)
        {
            AdjustTakenDamage();
        }

        public override int Base
        {
            get { return ValueBase; }
            set
            {
                ValueBase = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override int Equiped
        {
            get { return ValueEquiped; }
            set
            {
                ValueEquiped = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override int Given
        {
            get { return ValueGiven; }
            set
            {
                ValueGiven = value;
                AdjustTakenDamage();
                OnModified();
            }
        }

        public override int Context
        {
            get { return ValueContext; }
            set
            {
                ValueContext = value;
                OnModified();
            }
        }

        public int DamageTaken
        {
            get { return m_damageTaken; }
            set
            {
                m_realDamageTaken = value;
                m_damageTaken = value > TotalMaxWithoutPermanentDamages ? TotalMaxWithoutPermanentDamages : value;
                OnModified();
            }
        }

        public int PermanentDamages
        {
            get { return m_permanentDamages; }
            set {
                if (TotalMaxWithoutPermanentDamages - value < 0)
                    m_permanentDamages = Base + Equiped + Given + Context + ( Owner.Stats != null ? Owner.Stats[PlayerFields.Vitality].Total : 0 ) - 1;
                else
                {
                    m_permanentDamages = value;
                }

                if (TotalSafe > TotalMax)
                {
                    DamageTaken += (TotalSafe - TotalMax);
                }

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
                var result = TotalMaxWithoutPermanentDamages - DamageTaken;

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
                var result = Base + Equiped + Given + Context + ( Owner.Stats != null ? Owner.Stats[PlayerFields.Vitality].Total : 0 ) - PermanentDamages;

                return result < 0 ? 0 : result;
            }
        }

        /// <summary>
        ///   TotalMax without PermanentDamages
        /// </summary>
        public int TotalMaxWithoutPermanentDamages
        {
            get
            {
                return TotalMax + PermanentDamages;
            }
        }

        private void AdjustTakenDamage()
        {
            if (m_damageTaken > TotalMaxWithoutPermanentDamages)
            {
                m_realDamageTaken = m_damageTaken;
                m_damageTaken = (short)TotalMaxWithoutPermanentDamages; // hp cannot be lesser than 0
            }
            else if (m_realDamageTaken > m_damageTaken)
            {
                m_damageTaken = m_realDamageTaken;
            }
        }

        public override StatsData CloneAndChangeOwner(IStatsOwner owner)
        {
            var clone = new StatsHealth(owner, Base, DamageTaken)
            {
                Equiped = Equiped,
                Given = Given,
                Context = Context,
                Additional = Additional
            };

            return clone;
        }
    }
}