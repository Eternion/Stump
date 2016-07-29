using System.Collections.Generic;

namespace Stump.ORM.Relator
{
    public interface IOneToManyRecord1<T> : IJoined
    {
        List<T> ManyProperty1
        {
            get;
        }
    }

    public interface IOneToManyRecord2<T1,T2> : IJoined
    {
        List<T1> ManyProperty1
        {
            get;
        }

        List<T2> ManyProperty2
        {
            get;
        }
    }
}