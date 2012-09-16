namespace Stump.ORM.Mapping
{
    public class Column
    {
        public const int DefaultLength = 255;
        public const int DefaultPrecision = 19;
        public const int DefaultScale = 2;

        private int? m_length;
        private int? m_precision;
        private int? m_scale;
        private int m_uniqueId;

        public string Name
        {
            get;
            set;
        }

        public int Length
        {
            get
            {
                return m_length.GetValueOrDefault(DefaultLength);
            }
            set
            {
                m_length = value;
            }
        }

        public int Precision
        {
            get
            {
                return m_precision.GetValueOrDefault(DefaultPrecision);
            }
            set
            {
                m_precision = value;
            }
        }

        public int Scale
        {
            get
            {
                return m_scale.GetValueOrDefault(DefaultScale);
            }
            set
            {
                m_scale = value;
            }
        }

        public bool IsNullable
        {
            get;
            set;
        }

        public int TypeIndex
        {
            get;
            set;
        }

        public string GetAlias()
        {
            return Name + "_" + m_uniqueId++;
        }
    }
}