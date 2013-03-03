
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentEffect")]
    [Serializable]
    public class AlignmentEffect : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
        public uint descriptionId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint CharacteristicId
        {
            get { return characteristicId; }
            set { characteristicId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

    }
}