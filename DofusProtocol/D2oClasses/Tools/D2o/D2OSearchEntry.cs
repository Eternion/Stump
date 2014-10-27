namespace Stump.DofusProtocol.D2oClasses.Tools.D2o
{
    public class D2OSearchEntry
    {
        public D2OSearchEntry()
        {
            
        }

        public D2OSearchEntry(string fieldName, int fieldIndex, D2OFieldType fieldType, int fieldCount)
        {
            FieldName = fieldName;
            FieldIndex = fieldIndex;
            FieldType = fieldType;
            FieldCount = fieldCount;
        }

        public string FieldName
        {
            get;
            set;
        }

        public int FieldIndex
        {
            get;
            set;
        }

        public D2OFieldType FieldType
        {
            get;
            set;
        }

        public int FieldCount
        {
            get;
            set;
        }
    }
}