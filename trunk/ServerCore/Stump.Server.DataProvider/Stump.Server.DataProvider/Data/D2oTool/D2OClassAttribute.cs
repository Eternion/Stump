using System;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class D2OClassAttribute : Attribute
    {
        public D2OClassAttribute(string packageName)
        {
            PackageName = packageName;
        }

        public string PackageName
        {
            get;
            set;
        }
    }
}