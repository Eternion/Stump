using System;

namespace Stump.DofusProtocol.D2oClasses
{
    public class D2OClassAttribute : Attribute
    {
        public D2OClassAttribute(string name)
        {
            Name = name;
        }

        public D2OClassAttribute(string name, string packageName)
        {
            Name = name;
            PackageName = packageName;
        }

        public string Name
        {
            get;
            set;
        }

        public string PackageName
        {
            get;
            set;
        }
    }
}