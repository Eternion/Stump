using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;

namespace Stump.Server.WorldServer.Worlds.Actors.Stats
{
    public class StatsData
    {
        protected StatsFormulasHandler m_formulas;
        protected short m_valueBase;
        protected short m_valueContext;
        protected short m_valueEquiped;
        protected short m_valueGiven;

        public StatsData(IStatsOwner owner, CaracteristicsEnum name, short valueBase)
            : this(
                owner, name, valueBase,
                delegate(IStatsOwner _owner, int valuebase, int valueequiped, int valuegiven, int valuecontext)
                    {
                        if (_owner == null)
                            throw new ArgumentNullException("_owner");

                        return valuebase + valueequiped + valuegiven + valuecontext;
                    })
        {
        }

        public StatsData(IStatsOwner owner, CaracteristicsEnum name, short valueBase, StatsFormulasHandler formulas)
        {
            m_valueBase = valueBase;
            m_formulas = formulas;
            Name = name;
            Owner = owner;
        }

        public IStatsOwner Owner
        {
            get;
            protected set;
        }

        public CaracteristicsEnum Name
        {
            get;
            protected set;
        }

        public virtual short Base
        {
            get { return m_valueBase; }
            set { m_valueBase = value; }
        }

        public virtual short Equiped
        {
            get { return m_valueEquiped; }
            set { m_valueEquiped = value; }
        }

        public virtual short Given
        {
            get { return m_valueGiven; }
            set { m_valueGiven = value; }
        }

        public virtual short Context
        {
            get
            {
                return m_valueContext;
            }
            set
            {
                m_valueContext = value;
            }
        }

        public virtual int Total
        {
            get
            {
                if (m_formulas != null)
                {
                    int result = m_formulas(Owner, Base, Equiped, Given, Context);

                    return result;
                }

                return 0;
            }
        }

        /// <summary>
        ///   TotalSafe can't be lesser than 0
        /// </summary>
        public virtual int TotalSafe
        {
            get
            {
                var total = Total;

                return total > 0 ? total : 0;
            }
        }

        public static int operator +(int i1, StatsData s1)
        {
            return i1 + s1.Total;
        }

        public static int operator +(StatsData s1, StatsData s2)
        {
            return s1.Total + s2.Total;
        }

        public static int operator -(int i1, StatsData s1)
        {
            return i1 - s1.Total;
        }

        public static int operator -(StatsData s1, StatsData s2)
        {
            return s1.Total - s2.Total;
        }

        public static int operator *(int i1, StatsData s1)
        {
            return i1*s1.Total;
        }

        public static int operator *(StatsData s1, StatsData s2)
        {
            return s1.Total*s2.Total;
        }

        public static double operator /(StatsData s1, double d1)
        {
            return s1.Total/d1;
        }

        public static double operator /(StatsData s1, StatsData s2)
        {
            return s1.Total/(double) s2.Total;
        }

        public static implicit operator CharacterBaseCharacteristic(StatsData s1)
        {
            return new CharacterBaseCharacteristic(s1.Base, s1.Equiped, s1.Given, s1.Context);
        }
    }
}