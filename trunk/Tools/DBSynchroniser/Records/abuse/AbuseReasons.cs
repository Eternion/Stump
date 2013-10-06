 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AbuseReasons")]
    [D2OClass("AbuseReasons")]
    public class AbuseReasonsRecord : ID2ORecord
    {
        private const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AbuseReasons)obj;
            
            AbuseReasonId = castedObj._abuseReasonId;
            Mask = castedObj._mask;
            ReasonTextId = castedObj._reasonTextId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AbuseReasons();
            obj._abuseReasonId = AbuseReasonId;
            obj._mask = Mask;
            obj._reasonTextId = ReasonTextId;
            return obj;
        
        }
    }
}