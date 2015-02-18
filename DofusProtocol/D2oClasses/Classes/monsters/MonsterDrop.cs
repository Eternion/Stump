

// Generated on 02/18/2015 11:06:01
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
            get { return this.dropId; }
            set { this.dropId = value; }
        }
        [D2OIgnore]
        public int MonsterId
        {
            get { return this.monsterId; }
            set { this.monsterId = value; }
        }
        [D2OIgnore]
        public int ObjectId
        {
            get { return this.objectId; }
            set { this.objectId = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade1
        {
            get { return this.percentDropForGrade1; }
            set { this.percentDropForGrade1 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade2
        {
            get { return this.percentDropForGrade2; }
            set { this.percentDropForGrade2 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade3
        {
            get { return this.percentDropForGrade3; }
            set { this.percentDropForGrade3 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade4
        {
            get { return this.percentDropForGrade4; }
            set { this.percentDropForGrade4 = value; }
        }
        [D2OIgnore]
        public double PercentDropForGrade5
        {
            get { return this.percentDropForGrade5; }
            set { this.percentDropForGrade5 = value; }
        }
        [D2OIgnore]
        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }
        [D2OIgnore]
        public int FindCeil
        {
            get { return this.findCeil; }
            set { this.findCeil = value; }
        }
        [D2OIgnore]
        public Boolean HasCriteria
        {
            get { return this.hasCriteria; }
            set { this.hasCriteria = value; }
        }
    }
}