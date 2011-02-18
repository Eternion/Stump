using System;

namespace Stump.Server.DataProvider.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DataProviderAttribute : Attribute
    {
    }
}
