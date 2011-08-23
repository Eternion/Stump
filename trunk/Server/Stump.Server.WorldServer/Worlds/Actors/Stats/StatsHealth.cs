using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;

namespace Stump.Server.WorldServer.Worlds.Actors.Stats
{
    public class StatsHealth : StatsData
    {
        private static readonly StatsFormulasHandler FormuleLife =
            (owner, valueBase, valueEquiped, valueGiven, damageTaken) => valueBase + valueEquiped  + owner.Stats[CaracteristicsEnum.Vitality] - damageTaken;

        private readonly StatsFormulasHandler m_formule;

        public StatsHealth(IStatsOwner owner, short valueBase, short damageTaken)
            : base(owner, CaracteristicsEnum.Health, valueBase)
        {
            m_formule = FormuleLife;
            DamageTaken = damageTaken;
        }

        public short DamageTaken
        {
            get;
            set;
        }

        public override short Context
        {
            get
            {
                return DamageTaken;
            }
            set
            {
                DamageTaken = value;
            }
        }

        /// <summary>
        ///   Addition of values
        /// </summary>
        public override int Total
        {
            get
            {
                if (m_formule != null)
                {
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, DamageTaken);

                    return result;
                }

                return 0;
            }
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
                if (m_formule != null)
                {
                    int result = m_formule(Owner, Base, Equiped, Given, DamageTaken);

                    return result < 0 ? 0 : result;
                }

                return 0;
            }
        }

        /// <summary>
        ///   Additions of values without using damages taken;
        /// </summary>
        public int TotalMax
        {
            get
            {
                if (m_formule != null)
                {
                    int result = m_formule(Owner, Base, Equiped, Given, 0);

                    return result;
                }

                return 0;
            }
        }
    }
}