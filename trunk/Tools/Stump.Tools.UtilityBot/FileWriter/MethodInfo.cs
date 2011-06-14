
using System.Collections.Generic;

namespace Stump.Tools.UtilityBot.FileWriter
{
    public class MethodInfo
    {
        #region MethodModifiers enum

        public enum MethodModifiers
        {
            NONE,
            ABSTRACT,
            CONSTANT,
            STATIC,
            NEW,
            OVERRIDE,
            VIRTUAL
        } ;

        #endregion

        public MethodInfo()
        {
            Modifiers = new List<MethodModifiers>();
        }

        public List<MethodModifiers> Modifiers
        {
            get;
            set;
        }

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool ReturnsArray
        {
            get;
            set;
        }

        public string[] Args
        {
            get;
            set;
        }

        public string[] ArgsType
        {
            get;
            set;
        }

        public string[] ArgsDefaultValue
        {
            get;
            set;
        }
    }
}