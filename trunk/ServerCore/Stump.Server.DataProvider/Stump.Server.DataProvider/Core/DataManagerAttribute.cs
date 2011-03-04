using System;

namespace Stump.Server.DataProvider.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DataManagerAttribute : Attribute
    {

        public readonly string Name;

        public DataManagerAttribute(string name)
        {
            this.Name = name;
        }

    }
}
