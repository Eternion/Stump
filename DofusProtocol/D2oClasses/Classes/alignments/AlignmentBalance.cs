

// Generated on 04/19/2016 10:17:55
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
        public const String MODULE = "AlignmentBalance";
        public int id;
        public int startValue;
        public int endValue;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
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
        public int StartValue
        {
            get { return this.startValue; }
            set { this.startValue = value; }
        }
        [D2OIgnore]
        public int EndValue
        {
            get { return this.endValue; }
            set { this.endValue = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
    }
}