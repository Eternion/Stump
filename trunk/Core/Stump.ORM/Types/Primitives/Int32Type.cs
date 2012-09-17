using System;
using System.Data;

namespace Stump.ORM.Types.Primitives
{
    public class Int32Type : PrimitiveType
    {
        public override DbType DbType
        {
            get
            {
                return DbType.Int32;
            }
        }

        public override Type Type
        {
            get
            {
                return typeof(int);
            }
        }
    }
}