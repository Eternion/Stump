using System;

namespace Stump.DofusProtocol.D2oClasses.Tool
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