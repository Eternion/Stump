
using System;

namespace Stump.BaseCore.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Cyclic : Attribute
    {
        public Cyclic(int time)
        {
            Time = time;
        }

        public int Time
        {
            get;
            set;
        }
    }
}