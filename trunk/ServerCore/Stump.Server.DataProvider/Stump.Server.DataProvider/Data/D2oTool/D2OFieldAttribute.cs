using System;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class D2OFieldAttribute : Attribute
    {
        public string FieldName
        {
            get;
            set;
        }

        public D2OFieldAttribute()
        {
        }

        public D2OFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}