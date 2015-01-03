

// Generated on 12/29/2014 16:35:05
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentEffect", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentEffect : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
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
        public uint CharacteristicId
        {
            get { return this.characteristicId; }
            set { this.characteristicId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
    }
}