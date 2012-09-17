using System;
using System.Data;

namespace Stump.ORM.Types
{
    public abstract class PrimitiveType : IValue
    {
        public abstract DbType DbType
        {
            get;
        }

        public abstract Type Type
        {
            get;
        }
    }
}