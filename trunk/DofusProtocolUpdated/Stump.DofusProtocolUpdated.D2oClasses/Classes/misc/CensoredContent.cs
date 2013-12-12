

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CensoredContent", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class CensoredContent : IDataObject
    {
        public const String MODULE = "CensoredContents";
        public String lang;
        public int type;
        public int oldValue;
        public int newValue;
        [D2OIgnore]
        public String Lang
        {
            get { return lang; }
            set { lang = value; }
        }
        [D2OIgnore]
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        [D2OIgnore]
        public int OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }
        [D2OIgnore]
        public int NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }
    }
}