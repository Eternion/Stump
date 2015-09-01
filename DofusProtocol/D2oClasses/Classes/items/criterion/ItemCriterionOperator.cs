

// Generated on 09/01/2015 11:16:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ItemCriterionOperator", "com.ankamagames.dofus.datacenter.items.criterion")]
    [Serializable]
    public class ItemCriterionOperator : IDataObject
    {
        public const String SUPERIOR = ">";
        public const String INFERIOR = "<";
        public const String EQUAL = "";
        public const String DIFFERENT = "!";
    }
}