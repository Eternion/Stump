
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AbuseReasons")]
    [Serializable]
    public class AbuseReasons : IDataObject, IIndexedData
    {
        private const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;

        int IIndexedData.Id
        {
            get { return (int)_abuseReasonId; }
        }

        public uint AbuseReasonId
        {
            get { return _abuseReasonId; }
            set { _abuseReasonId = value; }
        }

        public uint Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        public int ReasonTextId
        {
            get { return _reasonTextId; }
            set { _reasonTextId = value; }
        }

    }
}