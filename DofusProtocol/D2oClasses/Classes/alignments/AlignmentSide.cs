

// Generated on 04/24/2015 03:38:24
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentSide", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentSide : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentSides";
        public int id;
        [I18NField]
        public uint nameId;
        public Boolean canConquest;
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
        public Boolean CanConquest
        {
            get { return this.canConquest; }
            set { this.canConquest = value; }
        }
    }
}