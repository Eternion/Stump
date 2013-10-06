

// Generated on 10/06/2013 17:58:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentBalance", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentBalance : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentBalance";
        public int id;
        public int startValue;
        public int endValue;
        public uint nameId;
        public uint descriptionId;
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
        public int StartValue
        {
            get { return startValue; }
            set { startValue = value; }
        }
        [D2OIgnore]
        public int EndValue
        {
            get { return endValue; }
            set { endValue = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
    }
}