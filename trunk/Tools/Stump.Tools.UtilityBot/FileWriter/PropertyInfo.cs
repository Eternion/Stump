
namespace Stump.Tools.UtilityBot.FileWriter
{
    public class PropertyInfo
    {
        public AccessModifiers AccessModifier
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