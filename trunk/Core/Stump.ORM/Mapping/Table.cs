using System;

namespace Stump.ORM.Mapping
{
    public class Table
    {
        public string Name
        {
            get;
            set;
        }

        public string Engine
        {
            get;
            set;
        }

        public object[] Columns
        {
            get;
            set;
        }

        public int AutoIncrement
        {
            get;
            set;
        }

        public DateTime Createtime
        {
            get;
            set;
        }

        public DateTime UpdateTime
        {
            get;
            set;
        }
    }
}