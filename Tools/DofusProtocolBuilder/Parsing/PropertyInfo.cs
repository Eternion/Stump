using System.Collections.Generic;

namespace DofusProtocolBuilder.Parsing
{
    public class PropertyInfo
    {
        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public List<MethodInfo.MethodModifiers> MethodModifiers
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string PropertyType
        {
            get;
            set;
        }

        public MethodInfo MethodGet
        {
            get;
            set;
        }

        public MethodInfo MethodSet
        {
            get;
            set;
        }
    }
}