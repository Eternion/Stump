using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class BrainIdentifierAttribute : Attribute
    {
       public BrainIdentifierAttribute(params int[] identifiers)
       {
           Identifiers = identifiers;
       }

       public int[] Identifiers
       {
           get;
           set;
       }
    }
}
