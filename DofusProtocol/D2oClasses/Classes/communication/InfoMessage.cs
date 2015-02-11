

// Generated on 02/11/2015 10:21:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("InfoMessage", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class InfoMessage : IDataObject
    {
        public const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        [I18NField]
        public uint textId;
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public uint MessageId
        {
            get { return this.messageId; }
            set { this.messageId = value; }
        }
        [D2OIgnore]
        public uint TextId
        {
            get { return this.textId; }
            set { this.textId = value; }
        }
    }
}