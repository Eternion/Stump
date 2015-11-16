

// Generated on 11/16/2015 14:26:28
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AbuseReasons", "com.ankamagames.dofus.datacenter.abuse")]
    [Serializable]
    public class AbuseReasons : IDataObject, IIndexedData
    {
        public const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        [I18NField]
        public int _reasonTextId;
        int IIndexedData.Id
        {
            get { return (int)_abuseReasonId; }
        }
        [D2OIgnore]
        public uint AbuseReasonId
        {
            get { return this._abuseReasonId; }
            set { this._abuseReasonId = value; }
        }
        [D2OIgnore]
        public uint Mask
        {
            get { return this._mask; }
            set { this._mask = value; }
        }
        [D2OIgnore]
        public int ReasonTextId
        {
            get { return this._reasonTextId; }
            set { this._reasonTextId = value; }
        }
    }
}