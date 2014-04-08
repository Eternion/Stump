

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterDrop", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterDrop : IDataObject, IIndexedData
    {
        public uint dropId;
        public int monsterId;
        public int objectId;
        public double percentDropForGrade1;
        public double percentDropForGrade2;
        public double percentDropForGrade3;
        public double percentDropForGrade4;
        public double percentDropForGrade5;
        public int count;
        public int findCeil;
        public Boolean hasCriteria;
        int IIndexedData.Id
        {
            get { return (int)dropId; }
        }
        [D2OIgnore]
        public uint DropId
        {
            get { return dropId; }
            set { dropId = value; }
        }
        [D2OIgnore]
        public int MonsterId
        {
            get { return monsterId; }
            set { monsterId = value; }
        }
        [D2OIgnore]
        public int ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade1
        {
            get { return percentDropForGrade1; }
            set { percentDropForGrade1 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade2
        {
            get { return percentDropForGrade2; }
            set { percentDropForGrade2 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade3
        {
            get { return percentDropForGrade3; }
            set { percentDropForGrade3 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade4
        {
            get { return percentDropForGrade4; }
            set { percentDropForGrade4 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade5
        {
            get { return percentDropForGrade5; }
            set { percentDropForGrade5 = value; }
        }
        [D2OIgnore]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        [D2OIgnore]
        public int FindCeil
        {
            get { return findCeil; }
            set { findCeil = value; }
        }
        [D2OIgnore]
        public Boolean HasCriteria
        {
            get { return hasCriteria; }
            set { hasCriteria = value; }
        }
    }
}