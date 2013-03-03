
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CensoredContents")]
    [Serializable]
    public class CensoredContent : IDataObject, IIndexedData
    {
        public const String MODULE = "CensoredContents";
        public int type;
        public int oldValue;
        public int newValue;
        public String lang;

        int IIndexedData.Id
        {
            get { return (int)type; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        public int NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        public String Lang
        {
            get { return lang; }
            set { lang = value; }
        }

    }
}