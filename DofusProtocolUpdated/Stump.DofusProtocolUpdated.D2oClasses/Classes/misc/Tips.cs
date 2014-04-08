

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Tips", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class Tips : IDataObject, IIndexedData
    {
        public const String MODULE = "Tips";
        public int id;
        [I18NField]
        public uint descId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }
    }
}