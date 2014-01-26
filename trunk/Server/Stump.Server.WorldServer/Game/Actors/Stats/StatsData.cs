using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsData
    {
        protected StatsFormulasHandler m_formulas;
        protected int ValueBase;
        protected int ValueContext;
        protected int ValueEquiped;
        protected int ValueGiven;

        public StatsData(IStatsOwner owner, PlayerFields name, int valueBase, StatsFormulasHandler formulas = null)
        {
            ValueBase = valueBase;
            m_formulas = formulas;
            Name = name;
            Owner = owner;
        }

        public IStatsOwner Owner
        {
            get;
            protected set;
        }

        public PlayerFields Name
        {
            get;
            protected set;
        }

        public virtual int Base
        {
            get
            {
                return m_formulas != null ? m_formulas(Owner) + ValueBase : ValueBase;
            }
            set
            {
                ValueBase = value;
                OnModified();
            }
        }

        public virtual int Equiped
        {
            get { return ValueEquiped; }
            set
            {
                ValueEquiped = value;
                OnModified();
            }
        }

        public virtual int Given
        {
            get { return ValueGiven; }
            set
            {
                ValueGiven = value;
                OnModified();
            }
        }

        public virtual int Context
        {
            get { return ValueContext; }
            set
            {
                ValueContext = value;
                OnModified();
            }
        }

        public virtual int Total
        {
            get
            {
                var result = Base + Equiped + Context + Given;

                return result;
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

        public event Action<StatsData, int> Modified;

        protected virtual void OnModified()
        {
            var handler = Modified;
            if (handler != null) handler(this, Total);
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
            return new CharacterBaseCharacteristic(
                (short)( s1.Base > short.MaxValue ? short.MaxValue : s1.Base ),
                (short)( s1.Equiped > short.MaxValue ? short.MaxValue : s1.Equiped ),
                (short)( s1.Given > short.MaxValue ? short.MaxValue : s1.Given ),
                (short)( s1.Context > short.MaxValue ? short.MaxValue : s1.Context ));
        }

        

        public override string ToString()
        {
            return string.Format("{0}({1}+{2}+{3})", Total, Base, Equiped, Context);
        }
    }
}