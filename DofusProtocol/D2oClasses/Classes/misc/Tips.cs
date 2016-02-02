

// Generated on 02/02/2016 14:15:04
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint DescId
        {
            get { return this.descId; }
            set { this.descId = value; }
        }
    }
}