
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("NpcMessages")]
    [Serializable]
    public class NpcMessage : IDataObject, IIndexedData
    {
        private const String MODULE = "NpcMessages";
        public int id;
        public uint messageId;
        public String messageParams;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public String MessageParams
        {
            get { return messageParams; }
            set { messageParams = value; }
        }

    }
}