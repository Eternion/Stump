
namespace Stump.Server.WorldServer.Entities
{
    public sealed class MonsterGrade
    {
        #region Properties

        public uint Grade
        {
            get;
            set;
        }

        public int MonsterId
        {
            get;
            set;
        }

        public uint Level
        {
            get;
            set;
        }

        public int PaDodge
        {
            get;
            set;
        }

        public int PmDodge
        {
            get;
            set;
        }

        public int EarthResistance
        {
            get;
            set;
        }

        public int AirResistance
        {
            get;
            set;
        }

        public int FireResistance
        {
            get;
            set;
        }

        public int WaterResistance
        {
            get;
            set;
        }

        public int NeutralResistance
        {
            get;
            set;
        }

        public int LifePoints
        {
            get;
            set;
        }

        public int ActionPoints
        {
            get;
            set;
        }

        public int MovementPoints
        {
            get;
            set;
        }

        #endregion
    }
}