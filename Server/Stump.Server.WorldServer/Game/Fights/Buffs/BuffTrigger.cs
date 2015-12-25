using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class BuffTrigger
    {
        public BuffTrigger(BuffTriggerType type)
        {
            Type = type;
        }

        public BuffTrigger(BuffTriggerType type, object parameter)
            : this (type)
        {
            Parameter = parameter;
        }

        public BuffTriggerType Type
        {
            get;
            set;
        }

        public object Parameter
        {
            get;
            set;
        }
    }
}
