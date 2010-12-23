using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Actions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionTypeAttribute : Attribute
    {
        public ActionsEnum Action
        {
            get;
            set;
        }

        public ActionTypeAttribute(ActionsEnum actionEnum)
        {
            Action = actionEnum;
        }
    }
}