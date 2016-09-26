

// Generated on 09/26/2016 01:50:27
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("DareCriteria", "com.ankamagames.dofus.datacenter.dare")]
    [Serializable]
    public class DareCriteria : IDataObject, IIndexedData
    {
        public const String MODULE = "DareCriterias";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint maxOccurence;
        public uint maxParameters;
        public int minParameterBound;
        public int maxParameterBound;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
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
        public uint MaxOccurence
        {
            get { return this.maxOccurence; }
            set { this.maxOccurence = value; }
        }
        [D2OIgnore]
        public uint MaxParameters
        {
            get { return this.maxParameters; }
            set { this.maxParameters = value; }
        }
        [D2OIgnore]
        public int MinParameterBound
        {
            get { return this.minParameterBound; }
            set { this.minParameterBound = value; }
        }
        [D2OIgnore]
        public int MaxParameterBound
        {
            get { return this.maxParameterBound; }
            set { this.maxParameterBound = value; }
        }
    }
}