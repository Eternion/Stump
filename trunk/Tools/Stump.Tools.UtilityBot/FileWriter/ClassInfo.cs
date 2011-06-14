
namespace Stump.Tools.UtilityBot.FileWriter
{
    public class ClassInfo
    {
        #region ClassModifiers enum

        public enum ClassModifiers
        {
            NONE,
            ABSTRACT
        } ;

        #endregion

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public ClassModifiers ClassModifier
        {
            get;
            set;
        }

        public string Namespace
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Heritage
        {
            get;
            set;
        }

        public string CustomAttribute
        {
            get;
            set;
        }
    }
}