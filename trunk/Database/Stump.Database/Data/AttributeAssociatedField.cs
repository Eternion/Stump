
using System;

namespace Stump.Database.Data
{
    public class AttributeAssociatedField : Attribute
    {
        public AttributeAssociatedField(string fieldName)
        {
            FieldsName = new[] {fieldName};
        }

        public AttributeAssociatedField(params string[] fieldsname)
        {
            FieldsName = fieldsname;
        }

        public string[] FieldsName
        {
            get;
            set;
        }
    }
}