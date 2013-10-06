

// Generated on 10/06/2013 17:58:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AbuseReasons", "com.ankamagames.dofus.datacenter.abuse")]
    [Serializable]
    public class AbuseReasons : IDataObject
    {
        private const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;
        [D2OIgnore]
        public uint AbuseReasonId
        {
            get { return _abuseReasonId; }
            set { _abuseReasonId = value; }
        }
        [D2OIgnore]
        public uint Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
        [D2OIgnore]
        public int ReasonTextId
        {
            get { return _reasonTextId; }
            set { _reasonTextId = value; }
        }
    }
}