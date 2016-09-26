

// Generated on 09/26/2016 01:50:25
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentOrder", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentOrder : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentOrder";
        public int id;
        [I18NField]
        public uint nameId;
        public uint sideId;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint SideId
        {
            get { return this.sideId; }
            set { this.sideId = value; }
        }
    }
}