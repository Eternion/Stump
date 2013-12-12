

// Generated on 12/12/2013 16:57:36
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint CharacteristicId
        {
            get { return characteristicId; }
            set { characteristicId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
    }
}