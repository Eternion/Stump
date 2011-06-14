
using System;

namespace Stump.Server.WorldServer.Entities
{
    public class StatsHealth : StatsData
    {
        private static readonly Func<LivingEntity, int, int, int, int, int, int> FormuleLife =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus, damageTaken) => valueBase + valueEquiped + valueBonus + owner.Stats["Vitality"] - damageTaken;

        protected new Func<LivingEntity, int, int, int, int, int, int> m_formule;

        public StatsHealth(LivingEntity owner, int valueBase, int damageTaken)
            : base(owner, "Health", valueBase)
        {
            m_formule = FormuleLife;
            DamageTaken = damageTaken;
        }

        public int DamageTaken
        {
            get;
            set;
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
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, DamageTaken);

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
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, DamageTaken);

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
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, 0);

                    return result;
                }

                return 0;
            }
        }
    }
}